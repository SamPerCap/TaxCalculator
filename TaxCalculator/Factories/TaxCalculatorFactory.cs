using Microsoft.Extensions.Caching.Memory;
using TaxCalculator.Contracts;
using TaxCalculator.Enums;
using TaxCalculator.Services.FantasyCountries;

namespace TaxCalculator.Factories
{
    public static class TaxCalculatorFactory
    {
        public static ITaxCalculator GetTaxCalculator(Country country, IMemoryCache memoryCache)
        {
            return country switch
            {
                Country.Imaginaria => new ImaginariaTaxCalculator(memoryCache),
                _ => throw new NotImplementedException("Tax calculation for the specified country is not implemented."),
            };
        }
    }
}
