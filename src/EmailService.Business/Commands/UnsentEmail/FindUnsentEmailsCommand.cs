using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Business.Commands.UnsentEmail.Interfaces;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Mappers.Models.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Requests;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EmailService.Business.Commands.UnsentEmail
{
  public class FindUnsentEmailsCommand : IFindUnsentEmailsCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IBaseFindFilterValidator _baseFindValidator;
    private readonly IUnsentEmailRepository _repository;
    private readonly IUnsentEmailInfoMapper _unsentEmailMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreater _responseCreater;

    public FindUnsentEmailsCommand(
      IAccessValidator accessValidator,
      IBaseFindFilterValidator baseFindValidator,
      IUnsentEmailRepository repository,
      IUnsentEmailInfoMapper mapper,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreater responseCreater)
    {
      _accessValidator = accessValidator;
      _baseFindValidator = baseFindValidator;
      _repository = repository;
      _unsentEmailMapper = mapper;
      _httpContextAccessor = httpContextAccessor;
      _responseCreater = responseCreater;
    }

    public async Task<FindResultResponse<UnsentEmailInfo>> ExecuteAsync(BaseFindFilter filter)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailTemplates))
      {
        return _responseCreater.CreateFailureFindResponse<UnsentEmailInfo>(HttpStatusCode.Forbidden);
      }

      if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
      {
        return _responseCreater.CreateFailureFindResponse<UnsentEmailInfo>(
          HttpStatusCode.BadRequest,
          errors);
      }

      (List<DbUnsentEmail> unsentEmailes, int totalCount) repositoryResponse = await _repository.FindAsync(filter);

      return new()
      {
        Body = repositoryResponse.unsentEmailes?.Select(_unsentEmailMapper.Map).ToList(),
        TotalCount = repositoryResponse.totalCount,
        Status = OperationResultStatusType.FullSuccess
      };
    }
  }
}
