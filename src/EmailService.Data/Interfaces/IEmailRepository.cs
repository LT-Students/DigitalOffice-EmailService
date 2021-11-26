using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EmailService.Data.Interfaces
{
  [AutoInject]
  public interface IEmailRepository
  {
    Task SaveEmailAsync(DbEmail dbEmail);
  }
}
