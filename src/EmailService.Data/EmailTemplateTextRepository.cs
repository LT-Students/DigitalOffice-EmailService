using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Data.Provider;
using LT.DigitalOffice.EmailService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EmailService.Data
{
  public class EmailTemplateTextRepository : IEmailTemplateTextRepository
  {
    private readonly IDataProvider _provider;

    public EmailTemplateTextRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<Guid?> CreateAsync(DbEmailTemplateText request)
    {
      if (request == null)
      {
        return null;
      }

      _provider.EmailTemplateTexts.Add(request);
      await _provider.SaveAsync();

      return request.Id;
    }

    public async Task<bool> EditAsync(Guid emailTemplateTextId, JsonPatchDocument<DbEmailTemplateText> patch)
    {
      if (patch == null)
      {
        return false;
      }

      DbEmailTemplateText dbEmailTemplateText = await _provider.EmailTemplateTexts
        .FirstOrDefaultAsync(et => et.Id == emailTemplateTextId);

      if (dbEmailTemplateText == null)
      {
        return false;
      }

      patch.ApplyTo(dbEmailTemplateText);
      await _provider.SaveAsync();

      return true;
    }
  }
}
