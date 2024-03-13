using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using WebApi.Common;
using WebApi.Models;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services;

namespace WebApi.Controllers
{
    public class HomeController : ApiControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly HttpClient _client;
        private readonly UnitOfWork _uow;
        private readonly AppDbContext _db;
        //private

        public HomeController(UnitOfWork uow,
             IHttpClientFactory clientFactory,
             IMemoryCache cache,
             AppDbContext db)
        {
            _uow = uow;
            _client = clientFactory.CreateClient(AppConstants.ApiExchangeRateClientName);
            _cache = cache;
            _db = db;
        }



        [HttpGet("exchange-rates")]
        public async Task<IActionResult> GetExchangeRateAsync(string baseCurrency, string targetCurrency)
        {
            if (!_cache.TryGetValue("exchange-rates", out Dictionary<string, Currency>? currencies))
            {
                var value = await _client.GetAsync("");

                var responseBody = await value.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(responseBody);

                var baseSalaryObj = jsonObject[baseCurrency];

                currencies = new Dictionary<string, Currency>();
                foreach (var currency in baseSalaryObj.Children<JProperty>())
                {
                    currencies[$"_{currency.Name}"] = new Currency
                    {
                        Code = currency.Name,
                        Rate = (decimal)baseSalaryObj[currency.Name],
                    };
                }

                _cache.Set("exchange-rates", currencies, TimeSpan.FromMinutes(60));
                ExchangeRate.Currencies = currencies;

            }
            if (currencies != null)
                return Ok(currencies[$"_{targetCurrency}"].Rate);
            return Ok(1);
        }
    }
}
