﻿using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.ParseEntity;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EmailService.Business.Commands.ParseEntity.Interface
{
  [AutoInject]
  public interface IAddKeywordCommand
  {
    Task<OperationResultResponse<Guid?>> ExecuteAsync(AddKeywordRequest request);
  }
}