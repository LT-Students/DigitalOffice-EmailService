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
using LT.DigitalOffice.Kernel.Requests;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EmailService.Business.Commands.ParseEntity
{
  public class FindKeywordCommand : IFindKeywordCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IKeywordRepository _repository;
    private readonly IKeywordInfoMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FindKeywordCommand(
      IAccessValidator accessValidator,
      IKeywordRepository repository,
      IKeywordInfoMapper mapper,
      IHttpContextAccessor httpContextAccessor)
    {
      _accessValidator = accessValidator;
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<FindResultResponse<KeywordInfo>> Execute(BaseFindFilter filter)
    {
      if (!_accessValidator.HasRights(Rights.AddEditRemoveEmailTemplates))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new FindResultResponse<KeywordInfo>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
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
