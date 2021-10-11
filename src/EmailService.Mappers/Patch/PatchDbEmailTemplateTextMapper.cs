using LT.DigitalOffice.EmailService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplateText;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.EmailService.Mappers.Db
{
  public class PatchDbEmailTemplateTextMapper : IPatchDbEmailTemplateTextMapper
  {
    public JsonPatchDocument<DbEmailTemplateText> Map(
      JsonPatchDocument<EditEmailTemplateTextRequest> request)
    {
      if (request == null)
      {
        return null;
      }

      JsonPatchDocument<DbEmailTemplateText> dbPatch = new();

      foreach (var item in request.Operations)
      {
        dbPatch.Operations.Add(new Operation<DbEmailTemplateText>(item.op, item.path, item.from, item.value));
      }

      return dbPatch;
    }
  }
}
