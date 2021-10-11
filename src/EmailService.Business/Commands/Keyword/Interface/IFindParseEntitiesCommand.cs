using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EmailService.Business.Commands.ParseEntity.Interface
{
  [AutoInject]
  public interface IFindParseEntitiesCommand
  {
    Task<OperationResultResponse<Dictionary<string, Dictionary<string, List<string>>>>> Execute();
  }
}
