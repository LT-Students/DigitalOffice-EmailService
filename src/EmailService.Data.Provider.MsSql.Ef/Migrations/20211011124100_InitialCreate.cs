using System;
using LT.DigitalOffice.EmailService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.EmailService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(EmailServiceDbContext))]
  [Migration("20211011124100_InitialCreate")]
  public class InitialTables : Migration
  {
    protected override void Up(MigrationBuilder builder)
    {
      builder.CreateTable(
        name: DbEmail.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          SenderId = table.Column<Guid>(nullable: true),
          Receiver = table.Column<string>(nullable: false),
          Subject = table.Column<string>(nullable: false),
          Body = table.Column<string>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbEmail.TableName}", x => x.Id);
        });

      builder.CreateTable(
        name: DbEmailTemplate.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Name = table.Column<string>(nullable: false),
          Type = table.Column<int>(nullable: false),
          IsActive = table.Column<bool>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbEmailTemplate.TableName}", x => x.Id);
        });

      builder.CreateTable(
        name: DbEmailTemplateText.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          EmailTemplateId = table.Column<Guid>(nullable: false),
          Subject = table.Column<string>(nullable: false),
          Text = table.Column<string>(nullable: false),
          Language = table.Column<string>(nullable: false, maxLength: 2)
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbEmailTemplateText.TableName}", x => x.Id);
        });

      builder.CreateTable(
        name: DbKeyword.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Keyword = table.Column<string>(nullable: false, maxLength: 50),
          ServiceName = table.Column<int>(nullable: false),
          EntityName = table.Column<string>(nullable: false),
          PropertyName = table.Column<string>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Keyword", p => p.Id);
          table.UniqueConstraint("UC_Keyword", p => p.Keyword);
        });

      builder.CreateTable(
        name: DbUnsentEmail.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          EmailId = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          LastSendAtUtc = table.Column<DateTime>(nullable: false),
          TotalSendingCount = table.Column<uint>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbUnsentEmail.TableName}", x => x.Id);
        });

      builder.CreateTable(
        name: DbModuleSetting.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Host = table.Column<string>(nullable: false),
          Post = table.Column<int>(nullable: false),
          EnableSsl = table.Column<bool>(nullable: false),
          Email = table.Column<string>(nullable: false),
          Password = table.Column<string>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_DbModuleSetting.TableName", x => x.Id);
        }
        );
    }

    protected override void Down(MigrationBuilder builder)
    {
      builder.DropTable(
        name: DbEmail.TableName);

      builder.DropTable(
        name: DbEmailTemplate.TableName);

      builder.DropTable(
        name: DbEmailTemplateText.TableName);

      builder.DropTable(
        name: DbKeyword.TableName);

      builder.DropTable(
        name: DbUnsentEmail.TableName);

      builder.DropTable(
        name: DbModuleSetting.TableName);
    }
  }
}
