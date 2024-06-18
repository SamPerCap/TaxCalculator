using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Data
{
    public class TaxPayer
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+(?: [a-zA-Z]+)+$", ErrorMessage = "Name must be composed of at least two words separated by space and only containing letters.")]
        public string FullName { get; set; }

        [Required]
        [Range(10000, 9999999999, ErrorMessage = "Invalid SSN number. It must has between 5} up to 10 digits.")]
        public long SSN { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Gross income must be a valid quantity.")]
        public decimal GrossIncome { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Charity spent must be a valid quantity.")]
        public decimal CharitySpent { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
