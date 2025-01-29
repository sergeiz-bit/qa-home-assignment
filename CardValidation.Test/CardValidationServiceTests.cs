using CardValidation.Core.Enums;
using CardValidation.Core.Services;

namespace CardValidation.Test
{
    public class CardValidationServiceTests
    {
        private readonly CardValidationService _cardValidationService;

        public CardValidationServiceTests()
        {
            _cardValidationService = new CardValidationService();
        }

        [Theory]
        [InlineData("smith", true)]                     // Valid single word name (case insensitive)
        [InlineData("Will Smith", true)]                // Valid two words name
        [InlineData("Will Smith Second", true)]         // Valid three words name
        [InlineData("Will Smith Second First", false)]  // More than 3 words not allowed      
        [InlineData("Will  ", false)]                   // Trailing space are not allowed.
        [InlineData("Will123", false)]                  // Numbers are not allowed
        [InlineData("Will-Smith", false)]               // Special character are not allowed
        [InlineData("  ", false)]                       // Only spaces are not allowed
        [InlineData("", false)]                         // Empty strings are not allowed

        public void ShouldValidateCardOwnerCorrectly(string owner, bool expected)
        {
            var result = _cardValidationService.ValidateOwner(owner);

            Assert.Equal(result, expected);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenOwnerIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _cardValidationService.ValidateOwner(null));

            Assert.Equal("Value cannot be null. (Parameter 'input')", exception.Message);

        }

        [Theory]
        [InlineData("123", true)]    // Valid 3-digit CVC
        [InlineData("1234", true)]   // Valid 4-digit CVC
        [InlineData("12", false)]    // Too short
        [InlineData("12345", false)] // Too long
        [InlineData("abc", false)]   // Non-numeric characters
        [InlineData("12a4", false)]  // Mixed characters
        [InlineData("", false)]      // empty CVC

        public void ShouldValidateCvcNumberCorrectly(string cvc, bool expected)
        {
            var result = _cardValidationService.ValidateCvc(cvc);

            Assert.Equal(expected, result);
        }


        [Theory]
        //Visa
        [InlineData("4321432143214321", true)]      // Valid Visa (16 digits starts with 4)
        [InlineData("1111111111111111", false)]     // Invalid Visa (16 digits)
        [InlineData("4321432143211", true)]         // Valid Visa (13 digits  starts with 4)
                                                    // [InlineData("4333 3333 3333 3333 333", true)]  Valid 19 digits Visa is not accepted, potentialy bug?        
        [InlineData("43214321432111", false)]       // Invalid length (14 digits)
        [InlineData("432143214321", false)]          // Invalid length (12 digits)
        [InlineData("abcdabcdabcdabcd", false)]     // Non-numeric card nunber 16 chars

        //MasterCard
        [InlineData("5555555555554444", true)]      // Valid MasterCard (16 digits 51-55 range)
        [InlineData("5105105105105100", true)]      // Valid MasterCard (16 digits 51-55 range)
        [InlineData("5305105105105100", true)]      // Valid MasterCard (16 digits 51-55 range)
        [InlineData("530510510510510", false)]      // Invalid lenght (15 digits 51-55 range)
        [InlineData("53051051051051000", false)]    // Invalid lenght (17 digits 51-55 range)
        [InlineData("2223000048400011", true)]      // Valid MasterCard (16 digits 2221-2720 range)
        [InlineData("2720992716510043", true)]      // Valid MasterCard (16 digits 2221-2720 range)
        [InlineData("2221000000000009", true)]      // Valid MasterCard (16 digits 2221-2720 range)
        [InlineData("222100000000000", false)]      // Invalid lenght (15 digits 2221-2720 range)
        [InlineData("22210000000000000", false)]    // Invalid lenght (17 digits 2221-2720 range)
        [InlineData("abcdabcdabcdabcde", false)]    // Non-numeric card nunber 16 chars

        //AMEX
        [InlineData("371449635398431", true)]       // Valid AMEX card (15 digits starts with 34 or 37)
        [InlineData("348774081201057", true)]       // Valid AMEX card (15 digits starts with 34 or 37)
        [InlineData("34877408120105", false)]       // Invalid length (14 digits)
        [InlineData("3487740812010555", false)]     // Invalid length (16 digits)
        [InlineData("abcdabcdabcdabc", false)]      // Non-numeric card nunber 15 chars

        [InlineData("", false)]                     // Empty input

        public void ShouldValidateVisaCardNumberCorrectly(string cardNumber, bool expected)
        {
            var result = _cardValidationService.ValidateNumber(cardNumber);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenCardNumberIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _cardValidationService.ValidateNumber(null));

            Assert.Equal("Value cannot be null. (Parameter 'input')", exception.Message);

        }

        [Theory]
        [InlineData("06/2025", true)]         // Valid future date with full year
        [InlineData("12/25", true)]           // Valid future date with short year
        [InlineData("02/2023", false)]        // Past date with full year
        [InlineData("03/23", false)]          // Past date with short year
        [InlineData("13/2025", false)]        // Invalid month
        [InlineData("00/2025", false)]        // Invalid month
        [InlineData("01/abc", false)]         // Invalid year
        [InlineData("01", false)]             // Incomplete date
        [InlineData("01/2025extra", false)]   // Extra characters after valid date
        [InlineData("", false)]               // Empty string


        public void ShouldValidateIssueDateCorrectly(string issueDate, bool expected)
        {
            var result = _cardValidationService.ValidateIssueDate(issueDate);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenIssueDateIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _cardValidationService.ValidateIssueDate(null));

            Assert.Equal("Value cannot be null. (Parameter 'input')", exception.Message);

        }

        [Theory]
        [InlineData("4321432143214321", PaymentSystemType.Visa)]
        [InlineData("5555555555444444", PaymentSystemType.MasterCard)] 
        [InlineData("371449635398431", PaymentSystemType.AmericanExpress)] 
        public void ShouldReturnCorrectType(string cardNumber, PaymentSystemType expectedType)
        {
           
            var result = _cardValidationService.GetPaymentSystemType(cardNumber);
         
            Assert.Equal(expectedType, result);
        }

        [Fact]
        public void ShouldThrowExceptionForInvalidCard()
        {
            
            string invalidCard = "1111111111111111";
            Assert.Throws<NotImplementedException>(() => _cardValidationService.GetPaymentSystemType(invalidCard));

        }

    }
}
