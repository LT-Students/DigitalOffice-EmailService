﻿using System;
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
    }

    protected override void Down(MigrationBuilder builder)
    {
      builder.DropTable(
        name: DbEmail.TableName);

      builder.DropTable(
        name: DbUnsentEmail.TableName);
    }
  }
}
