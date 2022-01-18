using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Data.Provider;
using LT.DigitalOffice.EmailService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EmailService.Data
{
  public class SmtpSettingsRepository : ISmtpSettingsRepository
  {
    private readonly IDataProvider _provider;
    public SmtpSettingsRepository(
      IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<bool> CreateAsync(DbModuleSetting dbModuleSetting)
    {
      if (dbModuleSetting is null)
      {
        return false;
      }

      _provider.ModuleSettings.Add(dbModuleSetting);
      await _provider.SaveAsync();

      return true;
    }

    public async Task<DbModuleSetting> GetAsync()
    {

      return await _provider.ModuleSettings.FirstOrDefaultAsync();
    }
  }
}
