using Microsoft.Extensions.Caching.Memory;
using Moq;
using TaxCalculator.Services;

namespace TaxCalculatorTest
{
    public class BaseTaxCalculatorTests
    {
        private readonly BaseTaxCalculator _taxCalculator;
        public BaseTaxCalculatorTests()
        {
            _taxCalculator = new BaseTaxCalculatorMock();
        }

        [Fact]
        public void CalculateTaxes_GrossIncomeBelowThreshold_Returns_GrossIncome()
        {
            // Arrange
            var memoryCacheMock = new Mock<IMemoryCache>();
            var taxPayer = new TaxPayer
            {
                GrossIncome = 980
            };

            // Act
            var result = _taxCalculator.CalculateTaxes(taxPayer);

            // Assert
            Assert.Equal(980, result.GrossIncome);
            Assert.Equal(0, result.CharitySpent);
            Assert.Equal(0, result.IncomeTax);
            Assert.Equal(0, result.SocialTax);
            Assert.Equal(0, result.TotalTax);
            Assert.Equal(980, result.NetIncome);
        }

        [Fact]
        public void CalculateTaxes_GrossIncomeAboveMaximumThreshold()
        {
            // Arrange
            var taxPayer = new TaxPayer
            {
                GrossIncome = 3400
            };

            // Act
            var result = _taxCalculator.CalculateTaxes(taxPayer);

            // Assert
            Assert.Equal(3400, result.GrossIncome);
            Assert.Equal(0, result.CharitySpent);
            Assert.Equal(240, result.IncomeTax);
            Assert.Equal(300, result.SocialTax);
            Assert.Equal(540, result.TotalTax);
            Assert.Equal(2860, result.NetIncome);
        }

        [Fact]
        public void CalculateTaxes_GrossIncomeAboveMinimum_With_Charity()
        {
            // Arrange
            var taxPayer = new TaxPayer
            {
                GrossIncome = 2500,
                CharitySpent = 150
            };

            // Act
            var result = _taxCalculator.CalculateTaxes(taxPayer);

            // Assert
            Assert.Equal(2500, result.GrossIncome);
            Assert.Equal(150, result.CharitySpent);
            Assert.Equal(135, result.IncomeTax);
            Assert.Equal(202.5m, result.SocialTax);
            Assert.Equal(337.5m, result.TotalTax);
            Assert.Equal(2162.5m, result.NetIncome);
        }

        [Fact]
        public void CalculateTaxes_GrossIncomeAboveMaximumThrehsold_With_Charity()
        {
            // Arrange
            var taxPayer = new TaxPayer
            {
                GrossIncome = 3600,
                CharitySpent = 520
            };

            // Act
            var result = _taxCalculator.CalculateTaxes(taxPayer);

            // Assert
            Assert.Equal(3600, result.GrossIncome);
            Assert.Equal(520, result.CharitySpent);
            Assert.Equal(224, result.IncomeTax);
            Assert.Equal(300, result.SocialTax);
            Assert.Equal(524, result.TotalTax);
            Assert.Equal(3076, result.NetIncome);
        }

        [Fact]
        public void CalculateTaxes_GrossIncomeAboveMinimumThreshold()
        {
            // Arrange
            var taxPayer = new TaxPayer
            {
                GrossIncome = 2900
            };

            // Act
            var result = _taxCalculator.CalculateTaxes(taxPayer);

            // Assert
            Assert.Equal(2900, result.GrossIncome);
            Assert.Equal(0, result.CharitySpent);
            Assert.Equal(190, result.IncomeTax);
            Assert.Equal(285, result.SocialTax);
            Assert.Equal(475, result.TotalTax);
            Assert.Equal(2425, result.NetIncome);
        }

        private class BaseTaxCalculatorMock : BaseTaxCalculator
        {
            protected override decimal MinimumIncomeThreshold => 1000m;
            protected override decimal MaximumThresholdForSocialTax => 3000m;
            protected override decimal IncomeTaxPercentage => 0.1m;
            protected override decimal SocialTaxPercentage => 0.15m;
            protected override decimal MaxCharityPercentage => 0.1m;
        }
    }
}