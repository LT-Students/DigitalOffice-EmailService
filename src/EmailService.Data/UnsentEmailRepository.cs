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
  public class UnsentEmailRepository : IUnsentEmailRepository
  {
    private readonly IDataProvider _provider;

    public UnsentEmailRepository(
      IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task AddAsync(DbUnsentEmail email)
    {
      _provider.UnsentEmails.Add(email);
      await _provider.SaveAsync();
    }

    public async Task<DbUnsentEmail> GetAsync(Guid id)
    {
      return await _provider.UnsentEmails
        .Include(x => x.Email)
        .FirstOrDefaultAsync(eu => eu.Id == id);
    }

    public async Task<List<DbUnsentEmail>> GetAllAsync(int totalSendingCountIsLessThen)
    {
      return await _provider.UnsentEmails
        .Where(u => u.TotalSendingCount < totalSendingCountIsLessThen)
        .Include(u => u.Email)
        .ToListAsync();
    }

    public async Task<(List<DbUnsentEmail> unsentEmailes, int totalCount)> FindAsync(BaseFindFilter filter)
    {
      return (
        await _provider.UnsentEmails
          .Include(u => u.Email)
          .Skip(filter.SkipCount)
          .Take(filter.TakeCount)
          .ToListAsync(),
        await _provider.UnsentEmails.CountAsync());
    }

    public async Task<bool> RemoveAsync(DbUnsentEmail email)
    {
      if (email == null)
      {
        return false;
      }

      _provider.UnsentEmails.Remove(email);
      await _provider.SaveAsync();

      return true;
    }

    public async Task IncrementTotalCountAsync(DbUnsentEmail email)
    {
      email.TotalSendingCount++;
      email.LastSendAtUtc = DateTime.UtcNow;
      await _provider.SaveAsync();
    }
  }
}
