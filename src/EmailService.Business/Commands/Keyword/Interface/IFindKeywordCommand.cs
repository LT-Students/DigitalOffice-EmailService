using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Dto;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Requests;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EmailService.Business.Commands.ParseEntity.Interface
{
  [AutoInject]
  public interface IFindKeywordCommand
  {
    Task<FindResultResponse<KeywordInfo>> ExecuteAsync(BaseFindFilter filter);
  }
}
