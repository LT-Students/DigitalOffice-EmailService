using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Mappers.Models.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Models;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EmailService.Business.Commands.EmailTemplate
{
  public class FindEmailTemplateCommand : IFindEmailTemplateCommand
  {
    private readonly IEmailTemplateRepository _repository;
    private readonly IEmailTemplateInfoMapper _mapper;

    public FindEmailTemplateCommand(
      IEmailTemplateRepository repository,
      IEmailTemplateInfoMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<FindResultResponse<EmailTemplateInfo>> ExecuteAsync(FindEmailTemplateFilter filter)
    {
      FindResultResponse<EmailTemplateInfo> response = new();

      (List<DbEmailTemplate> dbEmailTempates, int totalCount) repositoryResponse =
        await _repository.FindAsync(filter);

      return new()
      {
        Body = repositoryResponse.dbEmailTempates?.Select(_mapper.Map).ToList(),
        TotalCount = repositoryResponse.totalCount,
        Status = OperationResultStatusType.FullSuccess
      };
    }
  }
}
