using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EmailService.Models.Db
{
  public class DbModuleSetting
  {
    public const string TableName = "ModuleSettings";

    public Guid Id { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
  }

  public class DbModuleConfiguration : IEntityTypeConfiguration<DbModuleSetting>
  {
    public void Configure(EntityTypeBuilder<DbModuleSetting> builder)
    {
      builder
        .ToTable(DbModuleSetting.TableName);

      builder
        .HasKey(ms => ms.Id);

      builder
        .Property(ms => ms.Host)
        .IsRequired();

      builder
        .Property(ms => ms.Email)
        .IsRequired();

      builder
        .Property(ms => ms.Password)
        .IsRequired();
    }
  }
}
