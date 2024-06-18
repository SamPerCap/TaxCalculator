using Microsoft.Extensions.Caching.Memory;
using TaxCalculator.Contracts;
using TaxCalculator.Data;
using TaxCalculator.Models;

namespace TaxCalculator.Services
{
    public abstract class BaseTaxCalculator : ITaxCalculator
    {
        protected abstract decimal MinimumIncomeThreshold { get; }
        protected abstract decimal MaximumThresholdForSocialTax { get; }
        protected abstract decimal IncomeTaxPercentage { get; }
        protected abstract decimal SocialTaxPercentage { get; }
        protected abstract decimal MaxCharityPercentage { get; }


        public virtual Taxes CalculateTaxes(TaxPayer taxPayer)
        {
            decimal grossIncome = taxPayer.GrossIncome;
            decimal charitySpent = taxPayer.CharitySpent;

            decimal deductible = CalculateDeductibleAmount(grossIncome, charitySpent);

            decimal taxableGrossIncome = CalculateTaxableGrossIncome(grossIncome, deductible);

            decimal incomeTax = CalculateIncomeTax(taxableGrossIncome);

            decimal socialTax = CalculateSocialTax(taxableGrossIncome);

            decimal totalTax = incomeTax + socialTax;

            decimal netIncome = grossIncome - totalTax;

            Taxes taxes = new()
            {
                GrossIncome = grossIncome,
                CharitySpent = charitySpent,
                IncomeTax = incomeTax,
                SocialTax = socialTax,
                TotalTax = totalTax,
                NetIncome = netIncome
            };

            return taxes;
        }

        private decimal CalculateDeductibleAmount(decimal grossIncome, decimal charitySpent)
        {
            return Math.Min(charitySpent, grossIncome * MaxCharityPercentage);
        }

        private static decimal CalculateTaxableGrossIncome(decimal grossIncome, decimal reductable)
        {
            return grossIncome - reductable;
        }

        private decimal CalculateIncomeTax(decimal taxableGrossIncome)
        {
            decimal incomeTaxableAmount = Math.Max(0, taxableGrossIncome - MinimumIncomeThreshold);
            return incomeTaxableAmount * IncomeTaxPercentage;
        }

        private decimal CalculateSocialTax(decimal taxableGrossIncome)
        {
            if (taxableGrossIncome <= MinimumIncomeThreshold)
                return 0;
            
            decimal socialTaxableAmount = Math.Min(taxableGrossIncome, MaximumThresholdForSocialTax) - MinimumIncomeThreshold;
            return socialTaxableAmount * SocialTaxPercentage;
        }
    }
}
