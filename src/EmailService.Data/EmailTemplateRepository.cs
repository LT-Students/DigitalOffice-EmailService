using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Data.Provider;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EmailService.Data
{
  public class EmailTemplateRepository : IEmailTemplateRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmailTemplateRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Guid?> CreateAsync(DbEmailTemplate request)
    {
      if (request == null)
      {
        return null;
      }

      _provider.EmailTemplates.Add(request);
      await _provider.SaveAsync();

      return request.Id;
    }

    public async Task<bool> EditAsync(Guid emailTemplateId, JsonPatchDocument<DbEmailTemplate> patch)
    {
      if (patch == null)
      {
        return false;
      }

      DbEmailTemplate dbEmailTemplate = await _provider.EmailTemplates
        .FirstOrDefaultAsync(et => et.Id == emailTemplateId);

      if (dbEmailTemplate == null)
      {
        return false;
      }

      patch.ApplyTo(dbEmailTemplate);
      dbEmailTemplate.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbEmailTemplate.ModifiedAtUtc = DateTime.UtcNow;
      await _provider.SaveAsync();

      return true;
    }

    public async Task<DbEmailTemplate> GetAsync(Guid emailTemplateId)
    {
      return await _provider.EmailTemplates
        .Include(et => et.EmailTemplateTexts)
        .FirstOrDefaultAsync(et => et.Id == emailTemplateId);
    }

    public async Task<DbEmailTemplate> GetAsync(int type)
    {
      return await _provider.EmailTemplates
        .Include(et => et.EmailTemplateTexts)
        .FirstOrDefaultAsync(et => et.Type == type && et.IsActive);
    }

    public async Task<(List<DbEmailTemplate> dbEmailTempates, int totalCount)> FindAsync(FindEmailTemplateFilter filter)
    {
      IQueryable<DbEmailTemplate> dbEmailTemplates = _provider.EmailTemplates.AsQueryable();

      if (!filter.IncludeDeactivated)
      {
        dbEmailTemplates = dbEmailTemplates.Where(e => e.IsActive);
      }

      return (
        await dbEmailTemplates
          .Skip(filter.SkipCount)
          .Take(filter.TakeCount)
          .Include(e => e.EmailTemplateTexts)
          .ToListAsync(),
        await dbEmailTemplates.CountAsync());
    }
  }
}
