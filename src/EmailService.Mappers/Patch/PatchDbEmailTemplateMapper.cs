﻿using System;
using LT.DigitalOffice.EmailService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Models.Broker.Enums;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.EmailService.Mappers.Db
{
  public class PatchDbEmailTemplateMapper : IPatchDbEmailTemplateMapper
  {
    public JsonPatchDocument<DbEmailTemplate> Map(
      JsonPatchDocument<EditEmailTemplateRequest> request)
    {
      if (request == null)
      {
        return null;
      }

      JsonPatchDocument<DbEmailTemplate> dbPatch = new();

      foreach (var item in request.Operations)
      {
        if (item.path.EndsWith(nameof(EditEmailTemplateRequest.Type), StringComparison.OrdinalIgnoreCase))
        {
          dbPatch.Operations.Add(new Operation<DbEmailTemplate>(
            item.op, item.path, item.from, (int)Enum.Parse(typeof(EmailTemplateType), item.value.ToString())));
          continue;
        }
        dbPatch.Operations.Add(new Operation<DbEmailTemplate>(item.op, item.path, item.from, item.value));
      }

      return dbPatch;
    }
  }
}