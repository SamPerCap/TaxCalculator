using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TaxCalculator.Contracts;
using TaxCalculator.Data;
using TaxCalculator.Enums;
using TaxCalculator.Factories;
using TaxCalculator.Models;

namespace TaxCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        public CalculatorController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpPost]
        public ActionResult Calculate([FromBody] TaxPayer taxPayer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Country countryTaxRules = DetermineCountryTaxRules(taxPayer.SSN);
            ITaxCalculator taxCalculator = TaxCalculatorFactory.GetTaxCalculator(countryTaxRules, _memoryCache);

            Taxes taxes = taxCalculator.CalculateTaxes(taxPayer);

            return Ok(taxes);
        }

        private static Country DetermineCountryTaxRules(long SSN)
        {
            return Country.Imaginaria;
        }
    }
}
