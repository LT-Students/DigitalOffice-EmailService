using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Data.Provider;
using LT.DigitalOffice.EmailService.Models.Db;

namespace LT.DigitalOffice.EmailService.Data
{
  public class EmailRepository : IEmailRepository
  {
    private readonly IDataProvider _provider;

    public EmailRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task SaveEmailAsync(DbEmail dbEmail)
    {
      _provider.Emails.Add(dbEmail);
      await _provider.SaveAsync();
    }
  }
}
