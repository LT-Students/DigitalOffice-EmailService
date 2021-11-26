using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EmailService.Data.Interfaces
{
  [AutoInject]
  public interface ISmtpSettingsRepository
  {
    Task<bool> CreateAsync(DbModuleSetting dbModuleSetting);
  }
}
