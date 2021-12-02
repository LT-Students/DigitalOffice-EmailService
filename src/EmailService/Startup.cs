using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthChecks.UI.Client;
using LT.DigitalOffice.EmailService.Broker.Consumers;
using LT.DigitalOffice.EmailService.Broker.Helpers;
using LT.DigitalOffice.EmailService.Broker.Helpers.ParseEntity;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.EmailService.Models.Dto.Configurations;
using LT.DigitalOffice.Kernel.BrokerSupport.Attributes.ParseEntity.Models.Requests;
using LT.DigitalOffice.Kernel.BrokerSupport.Attributes.ParseEntity.Models.Responses;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Kernel.BrokerSupport.Extensions;
using LT.DigitalOffice.Kernel.BrokerSupport.Middlewares.Token;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers;
using LT.DigitalOffice.Kernel.Middlewares.ApiInformation;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Serilog;

namespace LT.DigitalOffice.EmailService
{
  public class Startup : BaseApiInfo
  {
    public const string CorsPolicyName = "LtDoCorsPolicy";

    private readonly RabbitMqConfig _rabbitMqConfig;
    private readonly BaseServiceInfoConfig _serviceInfoConfig;

    public IConfiguration Configuration { get; }

    #region private methods

    private (string username, string password) GetRabbitMqCredentials()
    {
      static string GetString(string envVar, string formAppsettings, string generated, string fieldName)
      {
        string str = Environment.GetEnvironmentVariable(envVar);
        if (string.IsNullOrEmpty(str))
        {
          str = formAppsettings ?? generated;

          Log.Information(
            formAppsettings == null
              ? $"Default RabbitMq {fieldName} was used."
              : $"RabbitMq {fieldName} from appsetings.json was used.");
        }
        else
        {
          Log.Information($"RabbitMq {fieldName} from environment was used.");
        }

        return str;
      }

      return (GetString("RabbitMqUsername", _rabbitMqConfig.Username, $"{_serviceInfoConfig.Name}_{_serviceInfoConfig.Id}", "Username"),
        GetString("RabbitMqPassword", _rabbitMqConfig.Password, _serviceInfoConfig.Id, "Password"));
    }

    private void ConfigureMassTransit(IServiceCollection services)
    {
      (string username, string password) = GetRabbitMqCredentials();

      services.AddMassTransit(x =>
      {
        x.AddConsumer<SendEmailConsumer>();
        x.AddConsumer<CreateSmtpCredentialsConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
          cfg.Host(_rabbitMqConfig.Host, "/", host =>
          {
            host.Username(username);
            host.Password(password);
          });

          cfg.ReceiveEndpoint(_rabbitMqConfig.SendEmailEndpoint, ep =>
          {
            ep.ConfigureConsumer<SendEmailConsumer>(context);
          });

          cfg.ReceiveEndpoint(_rabbitMqConfig.CreateSmtpCredentialsEndpoint, ep =>
          {
            ep.ConfigureConsumer<CreateSmtpCredentialsConsumer>(context);
          });
        });

        x.AddRequestClients(_rabbitMqConfig);
      });

      services.AddMassTransitHostedService();
    }

    private void UpdateDatabase(IApplicationBuilder app)
    {
      using var serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();

      using var context = serviceScope.ServiceProvider.GetService<EmailServiceDbContext>();

      context.Database.Migrate();
    }

    private void StartResender(IApplicationBuilder app)
    {
      EmailEngineConfig emailEngineConfig = Configuration
        .GetSection(EmailEngineConfig.SectionName)
        .Get<EmailEngineConfig>();

      IServiceProvider serviceProvider = app.ApplicationServices.GetRequiredService<IServiceProvider>();

      var scope = app.ApplicationServices.CreateScope();

      IUnsentEmailRepository repository = scope.ServiceProvider.GetRequiredService<IUnsentEmailRepository>();

      ILoggerFactory loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
      ILogger<EmailResender> logger = loggerFactory.CreateLogger<EmailResender>();

      IRequestClient<IGetSmtpCredentialsRequest> rcGetSmtpCredentials = serviceProvider.CreateRequestClient<IGetSmtpCredentialsRequest>(
        new Uri($"{_rabbitMqConfig.BaseUrl}/{_rabbitMqConfig.GetSmtpCredentialsEndpoint}"), default);

      var resender = new EmailResender(repository, logger, rcGetSmtpCredentials);

      if (!int.TryParse(Environment.GetEnvironmentVariable("MaxResendingCount"), out int maxResendingCount))
      {
        maxResendingCount = emailEngineConfig.MaxResendingCount;
        Log.Information($"Max resending count from appsettings.json was used. Value '{maxResendingCount}'.");
      }
      else
      {
        Log.Information($"Max resending count from environment was used. Value '{maxResendingCount}'.");
      }

      if (!int.TryParse(Environment.GetEnvironmentVariable("ResendIntervalInMinutes"), out int resendIntervalInMinutes))
      {
        resendIntervalInMinutes = emailEngineConfig.ResendIntervalInMinutes;
        Log.Information($"Resen interval in minutes from appsettings.json was used. Value '{resendIntervalInMinutes}'.");
      }
      else
      {
        Log.Information($"Resend interval in minutes from environment was used. Value '{resendIntervalInMinutes}'.");
      }

      Task.Run(() => resender.StartResend(resendIntervalInMinutes, maxResendingCount));
    }

    private void FindParseProperties(IApplicationBuilder app)
    {
      IServiceProvider serviceProvider = app.ApplicationServices.GetRequiredService<IServiceProvider>();

      foreach (KeyValuePair<string, string> pair in _rabbitMqConfig.FindUserParseEntitiesEndpoint)
      {
        IRequestClient<IFindParseEntitiesRequest> rcFindParseEntities = serviceProvider.CreateRequestClient<IFindParseEntitiesRequest>(
          new Uri($"{_rabbitMqConfig.BaseUrl}/{pair.Value}"), default);

        try
        {
          var result = rcFindParseEntities.GetResponse<IOperationResult<IFindParseEntitiesResponse>>(IFindParseEntitiesRequest.CreateObj()).Result.Message;

          if (result.IsSuccess)
          {
            AllParseEntities.Entities.Add(pair.Key, result.Body.Entities);
          }
        }
        catch
        {

        }
      }
    }

    #endregion

    #region public methods

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;

      _serviceInfoConfig = Configuration
        .GetSection(BaseServiceInfoConfig.SectionName)
        .Get<BaseServiceInfoConfig>();

      _rabbitMqConfig = Configuration
        .GetSection(BaseRabbitMqConfig.SectionName)
        .Get<RabbitMqConfig>();

      Version = "1.0.0.0";
      Description = "EmailService, is intended to work with the emails and email templates.";
      StartTime = DateTime.UtcNow;
      ApiName = $"LT Digital Office - {_serviceInfoConfig.Name}";
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(
          CorsPolicyName,
          builder =>
          {
            builder
              .AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
          });
      });

      string connStr = Environment.GetEnvironmentVariable("ConnectionString");
      if (string.IsNullOrEmpty(connStr))
      {
        connStr = Configuration.GetConnectionString("SQLConnectionString");
        Log.Information($"SQL connection string from appsettings.json was used. Value '{HidePasswordHelper.HidePassword(connStr)}'.");
      }
      else
      {
        Log.Information($"SQL connection string from environment was used. Value '{HidePasswordHelper.HidePassword(connStr)}'.");
      }
      services.Configure<TokenConfiguration>(Configuration.GetSection("CheckTokenMiddleware"));
      services.Configure<BaseRabbitMqConfig>(Configuration.GetSection(BaseRabbitMqConfig.SectionName));
      services.Configure<BaseServiceInfoConfig>(Configuration.GetSection(BaseServiceInfoConfig.SectionName));

      services.AddHttpContextAccessor();

      services.AddDbContext<EmailServiceDbContext>(options =>
      {
        options.UseSqlServer(connStr);
      });

      services
        .AddHealthChecks()
        .AddSqlServer(connStr)
        .AddRabbitMqCheck();

      services
        .AddControllers()
        .AddNewtonsoftJson();

      services.AddBusinessObjects();

      services.AddTransient<EmailSender>();

      services.AddMemoryCache();

      ConfigureMassTransit(services);
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
      UpdateDatabase(app);

      FindParseProperties(app);

      StartResender(app);

      app.UseForwardedHeaders();

      app.UseExceptionsHandler(loggerFactory);

      app.UseApiInformation();

      app.UseRouting();

      app.UseMiddleware<TokenMiddleware>();

      app.UseCors(CorsPolicyName);

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers().RequireCors(CorsPolicyName);

        endpoints.MapHealthChecks($"/{_serviceInfoConfig.Id}/hc", new HealthCheckOptions
        {
          ResultStatusCodes = new Dictionary<HealthStatus, int>
          {
            { HealthStatus.Unhealthy, 200 },
            { HealthStatus.Healthy, 200 },
            { HealthStatus.Degraded, 200 },
          },
          Predicate = check => check.Name != "masstransit-bus",
          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
      });
    }

    #endregion
  }
}
