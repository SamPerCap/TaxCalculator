using Microsoft.Extensions.Caching.Memory;
using TaxCalculator.Data;
using TaxCalculator.Models;

namespace TaxCalculator.Services.FantasyCountries
{
    public class ImaginariaTaxCalculator : BaseTaxCalculator
    {
        private readonly IMemoryCache _memoryCache;

        public ImaginariaTaxCalculator(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        protected override decimal MinimumIncomeThreshold => 1000m;
        protected override decimal MaximumThresholdForSocialTax => 3000m;
        protected override decimal IncomeTaxPercentage => 0.1m;
        protected override decimal SocialTaxPercentage => 0.15m;
        protected override decimal MaxCharityPercentage => 0.1m;

        public override Taxes CalculateTaxes(TaxPayer taxPayer)
        {
            string cacheKey = $"Imaginaria-{taxPayer.SSN}-{taxPayer.FullName}";

            if (_memoryCache.TryGetValue(cacheKey, out Taxes cachedResult))
            {
                return cachedResult;
            }

            Taxes result = base.CalculateTaxes(taxPayer);

            _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(30));

            return result;
        }
    }
}
