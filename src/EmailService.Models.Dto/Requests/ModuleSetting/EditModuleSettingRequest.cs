namespace LT.DigitalOffice.EmailService.Models.Dto.Requests.ModuleSetting
{
  public record EditModuleSettingRequest
  {
    public string Host { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
  }
}
