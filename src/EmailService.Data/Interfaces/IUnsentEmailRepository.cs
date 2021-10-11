using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Requests;

namespace LT.DigitalOffice.EmailService.Data.Interfaces
{
  [AutoInject]
  public interface IUnsentEmailRepository
  {
    Task AddAsync(DbUnsentEmail email);

    Task<DbUnsentEmail> GetAsync(Guid id);

    Task<List<DbUnsentEmail>> GetAllAsync(int totalSendingCountIsLessThen);

    Task<(List<DbUnsentEmail> unsentEmailes, int totalCount)> FindAsync(BaseFindFilter filter);

    Task<bool> RemoveAsync(DbUnsentEmail email);

    Task IncrementTotalCountAsync(DbUnsentEmail email);
  }
}
