using CardValidation.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;



namespace CardValidation.Test
{
    public class CardValidationControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CardValidationControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        
        }

        [Fact]
        public async Task ValidateCard_WithValidVisaNumber_ShouldReturnOkAsync()
        {           
            var creditCard = new CreditCard
            {
                Owner = "Will Smith",
                Number = "4111111111111111", 
                Date = "03/2025",
                Cvv = "123"
            };
                                   
            var response = await _client.PostAsJsonAsync("/CardValidation/card/credit/validate", creditCard);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("10");
           
        }

        [Fact]
        public async Task ValidateCard_WithValidMasterCardNumber_ShouldReturnOkAsync()
        {
            var creditCard = new CreditCard
            {
                Owner = "Will Smith",
                Number = "5555555555554444",
                Date = "03/2025",
                Cvv = "123"
            };

            var response = await _client.PostAsJsonAsync("/CardValidation/card/credit/validate", creditCard);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("20");

        }

        [Fact]
        public async Task ValidateCard_WithValidAmexNumber_ShouldReturnOkAsync()
        {
            var creditCard = new CreditCard
            {
                Owner = "Will Smith",
                Number = "371449635398431",                
                Date = "03/2025",
                Cvv = "123"
            };

            var response = await _client.PostAsJsonAsync("/CardValidation/card/credit/validate", creditCard);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("30");

        }

        [Fact]
        public async Task ValidateCard_WithInvalidNumber_ShouldReturnBadRequest()
        {
            var creditCard = new CreditCard
            {
                Owner = "Will Smith",
                Number = "411111111111111", 
                Date = "03/2025",
                Cvv = "123"
            };

            var response = await _client.PostAsJsonAsync("/CardValidation/card/credit/validate", creditCard);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("Wrong number");

        }

        [Fact]
        public async Task ValidateCard_WithWrongDate_ShouldReturnBadRequest()
        {
            var creditCard = new CreditCard
            {
                Owner = "Will Smith",
                Number = "371449635398431",
                Date = "03/2024",
                Cvv = "123"
            };

            var response = await _client.PostAsJsonAsync("/CardValidation/card/credit/validate", creditCard);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("Wrong date");

        }

        [Fact]
        public async Task ValidateCard_WithWrongCvv_ShouldReturnBadRequest()
        {
            var creditCard = new CreditCard
            {
                Owner = "Will Smith",
                Number = "371449635398431",
                Date = "03/2024",
                Cvv = "abc"
            };

            var response = await _client.PostAsJsonAsync("/CardValidation/card/credit/validate", creditCard);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("Wrong cvv");

        }

        [Fact(Skip = "4 digits CVC is not valid for Visa credit card and should not be accepted")]
        public async Task ValidateVisaCard_WithWrongCvv_ShouldReturnBadRequest()
        {
            var creditCard = new CreditCard
            {
                Owner = "Will Smith",
                Number = "4111111111111111",
                Date = "03/2025",
                Cvv = "1234"
            };

            var response = await _client.PostAsJsonAsync("/CardValidation/card/credit/validate", creditCard);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("Wrong cvv");

        }

        [Fact]
        public async Task ValidateCard_WithEmptyData_ShouldReturnBadReques()
        {
            var creditCard = new CreditCard
            {
                Owner = "",
                Number = "",
                Date = "",
                Cvv = ""
            };

            var response = await _client.PostAsJsonAsync("/CardValidation/card/credit/validate", creditCard);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("Owner is required", "Number is required", "Date is required", "Cvv is required");

        }

    }
}

