using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EmaileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.EmailService.Business.Commands.ParseEntity.Interface;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Requests;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EmailService.Business.Commands.ParseEntity
{
  public class FindKeywordCommand : IFindKeywordCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IBaseFindFilterValidator _baseFindValidator;
    private readonly IKeywordRepository _repository;
    private readonly IKeywordInfoMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FindKeywordCommand(
      IAccessValidator accessValidator,
      IBaseFindFilterValidator baseFindValidator,
      IKeywordRepository repository,
      IKeywordInfoMapper mapper,
      IHttpContextAccessor httpContextAccessor)
    {
      _accessValidator = accessValidator;
      _baseFindValidator = baseFindValidator;
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<FindResultResponse<KeywordInfo>> ExecuteAsync(BaseFindFilter filter)
    {
      if (!(await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailTemplates)))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new()
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new()
        {
          Status = OperationResultStatusType.Failed,
          Errors = errors
        };
      }

      (List<DbKeyword> dbKeywords, int totalCount) repositoryResponse = await _repository.FindAsync(filter);

      return new()
      {
        Status = OperationResultStatusType.FullSuccess,
        Body = repositoryResponse.dbKeywords?.Select(_mapper.Map).ToList(),
        TotalCount = repositoryResponse.totalCount
      };
    }
  }
}
