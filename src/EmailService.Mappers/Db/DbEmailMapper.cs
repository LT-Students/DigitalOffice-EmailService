using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.EmailService.Mappers.Db.Email.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using System;

namespace LT.DigitalOffice.EmailService.Mappers.Db.Email
{
    public class DbEmailMapper : IDbEmailMapper
    {
        public DbEmail Map(
            ISendEmailRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbEmail
            {
                Id = Guid.NewGuid(),
                SenderId = request.SenderId,
                Receiver = request.Email,
                CreatedAtUtc = DateTime.UtcNow
            };
        }
    }
}
