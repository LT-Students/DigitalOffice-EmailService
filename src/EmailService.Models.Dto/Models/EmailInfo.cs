using System;

namespace LT.DigitalOffice.EmailService.Models.Dto.Models
{
  public record EmailInfo
  {
    public Guid Id { get; set; }
    public string Receiver { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
  }
}
