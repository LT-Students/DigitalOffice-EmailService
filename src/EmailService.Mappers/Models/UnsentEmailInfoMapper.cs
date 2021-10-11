using LT.DigitalOffice.EmailService.Mappers.Models.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Models;

namespace LT.DigitalOffice.EmailService.Mappers.Models
{
  public class UnsentEmailInfoMapper : IUnsentEmailInfoMapper
  {
    private readonly IEmailInfoMapper _mapper;

    public UnsentEmailInfoMapper(
        IEmailInfoMapper mapper)
    {
      _mapper = mapper;
    }

    public UnsentEmailInfo Map(DbUnsentEmail dbUnsentEmail)
    {
      if (dbUnsentEmail == null)
      {
        return null;
      }

      return new UnsentEmailInfo
      {
        Id = dbUnsentEmail.Id,
        Email = _mapper.Map(dbUnsentEmail.Email),
        CreatedAtUtc = dbUnsentEmail.CreatedAtUtc,
        LastSendAtUtc = dbUnsentEmail.LastSendAtUtc,
        TotalSendingCount = dbUnsentEmail.TotalSendingCount
      };
    }
  }
}
