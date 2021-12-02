using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Mappers.Models.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Models;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;

namespace LT.DigitalOffice.EmailService.Business.Commands.EmailTemplate
{
  public class FindEmailTemplateCommand : IFindEmailTemplateCommand
  {
    private readonly IBaseFindFilterValidator _baseFindValidator;
    private readonly IEmailTemplateRepository _repository;
    private readonly IEmailTemplateInfoMapper _mapper;
    private readonly IResponseCreator _responseCreator;

    public FindEmailTemplateCommand(
      IBaseFindFilterValidator baseFindValidator,
      IEmailTemplateRepository repository,
      IEmailTemplateInfoMapper mapper,
      IResponseCreator responseCreator)
    {
      _baseFindValidator = baseFindValidator;
      _repository = repository;
      _mapper = mapper;
      _responseCreator = responseCreator;
    }

    public async Task<FindResultResponse<EmailTemplateInfo>> ExecuteAsync(FindEmailTemplateFilter filter)
    {
      if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
      {
        return _responseCreator.CreateFailureFindResponse<EmailTemplateInfo>(
          HttpStatusCode.BadRequest,
          errors);
      }

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
