using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Db;

namespace LT.DigitalOffice.EmailService.Data.Interfaces
{
  public interface ISmtpSettingsRepository
  {
    Task CreateAsync(DbModuleSetting dbModuleSetting);
  }
}
