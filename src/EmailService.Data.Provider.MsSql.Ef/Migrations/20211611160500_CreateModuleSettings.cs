using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.EmailService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(EmailServiceDbContext))]
  [Migration("20211611160500_CreateModuleSettings")]
  class _20211611160500_CreateModuleSettings : Migration
  {
    protected override void Up(MigrationBuilder builder)
    {
      builder.CreateTable(
        name: DbModuleSetting.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Host = table.Column<string>(nullable: false),
          Port = table.Column<int>(nullable: false),
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
          table.PrimaryKey($"PK_{DbModuleSetting.TableName}", x => x.Id);
        });
    }

    protected override void Down(MigrationBuilder builder)
    {
      builder.DropTable(
          name: DbModuleSetting.TableName);
    }
  }
}
