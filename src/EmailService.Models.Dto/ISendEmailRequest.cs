using System;

namespace LT.DigitalOffice.EmailService.Models.Dto
{
  public interface ISendEmailRequest
  {
    public string Receiver { get; set; }
    public string Subject { get; set; }
    public string Text { get; set; }
    public Guid? SenderId { get; set; }

    public object CreateObj(string receiver, string subject, string text, Guid senderId)
    {
      return new
      {
        Receiver = receiver,
        Subject = subject,
        Text = text,
        SenderId = senderId
      };
    }
  }

}
