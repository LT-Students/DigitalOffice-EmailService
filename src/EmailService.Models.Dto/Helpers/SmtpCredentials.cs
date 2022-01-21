using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LT.DigitalOffice.EmailService.Broker"), 
           InternalsVisibleTo("LT.DigitalOffice.EmailService.Data")]
namespace LT.DigitalOffice.EmailService.Models.Dto.Helpers
{
  internal static class SmtpCredentials
  {
    public static string Host { get; set; }
    public static int Port { get; set; }
    public static bool EnableSsl { get; set; }
    public static string Email { get; set; }
    public static string Password { get; set; }

    public static bool HasValue
    {
      get
      {
        return !(
          string.IsNullOrEmpty(Host) ||
          string.IsNullOrEmpty(Email) ||
          string.IsNullOrEmpty(Password));
      }
    }
  }
}
