using TaxCalculator.Data;
using TaxCalculator.Models;

namespace TaxCalculator.Contracts
{
    public interface ITaxCalculator
    {
        Taxes CalculateTaxes(TaxPayer tax);
    }
}
