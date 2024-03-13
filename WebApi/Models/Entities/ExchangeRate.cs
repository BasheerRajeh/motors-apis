namespace WebApi.Models.Entities
{
    public class ExchangeRate
    {
        public static Dictionary<string, Currency> Currencies { get; set; } = new Dictionary<string, Currency>();
    }
}
