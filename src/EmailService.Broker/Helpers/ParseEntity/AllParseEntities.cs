using System.Collections.Generic;

namespace LT.DigitalOffice.EmailService.Broker.Helpers.ParseEntity
{
  public static class AllParseEntities
  {
    public static Dictionary<string, Dictionary<string, List<string>>> Entities { get; set; }

    static AllParseEntities()
    {
      Entities = new();
    }
  }
}
