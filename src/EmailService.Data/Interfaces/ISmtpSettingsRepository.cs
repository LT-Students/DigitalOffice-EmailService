using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EmailService.Data.Interfaces
{
  [AutoInject]
  public interface ISmtpSettingsRepository
  {
    Task<bool> CreateAsync(DbModuleSetting dbModuleSetting);

    Task<DbModuleSetting> GetAsync();

    Task<bool> EditAsync(Guid moduleSettingId, JsonPatchDocument<DbModuleSetting> patch);
  }
}
