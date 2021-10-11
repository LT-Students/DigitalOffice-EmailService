using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Data.Provider;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.Kernel.Requests;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EmailService.Data
{
  public class KeywordRepository : IKeywordRepository
  {
    private readonly IDataProvider _provider;

    public KeywordRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<Guid?> AddAsync(DbKeyword request)
    {
      if (request == null)
      {
        return null;
      }

      _provider.ParseEntities.Add(request);
      await _provider.SaveAsync();

      return request.Id;
    }

    public async Task<DbKeyword> GetAsync(Guid entityId)
    {
      return await _provider.ParseEntities.FirstOrDefaultAsync(pe => pe.Id == entityId);
    }

    public async Task<bool> DoesKeywordExistAsync(string keyword)
    {
      return await _provider.ParseEntities.AnyAsync(pe => pe.Keyword == keyword);
    }

    public async Task<(List<DbKeyword> dbKeywords, int totalCount)> FindAsync(BaseFindFilter filter)
    {
      return (
        await _provider.ParseEntities
          .Skip(filter.SkipCount)
          .Take(filter.TakeCount)
          .ToListAsync(),
        await _provider.ParseEntities.CountAsync());
    }

    public async Task<bool> RemoveAsync(Guid entityId)
    {
      DbKeyword dbKeyword = await _provider.ParseEntities
        .FirstOrDefaultAsync(pe => pe.Id == entityId);

      if (dbKeyword == null)
      {
        return false;
      }

      _provider.ParseEntities.Remove(dbKeyword);
      await _provider.SaveAsync();

      return true;
    }
  }
}
