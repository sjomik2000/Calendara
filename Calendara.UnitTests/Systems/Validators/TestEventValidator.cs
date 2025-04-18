using Calendara.Application.Models;
using Calendara.Application.Validators;
using Calendara.Contracts.Responses;
using Calendara.UnitTests.Fixtures;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Calendara.UnitTests.Systems.Validators
{
    public class TestEventValidator
    {
        private readonly EventValidator _validator;

        public TestEventValidator()
        {
            _validator = new EventValidator();
        }

        #region Title Validation Tests
        [Fact]
        public void Validator_Fails_When_Title_Is_Empty()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventEmptyTitleFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.Title)
                .WithErrorMessage("Title must be provided.");
        }

        [Fact]
        public void Validator_Fails_When_Title_Exceeds_100_Characters()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventLongTitleFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.Title)
                .WithErrorMessage("Title must not exceed 100 characters.");
        }
        #endregion

        #region DateOnly Validation Tests
        [Fact]
        public void Validator_Fails_When_AllDay_Is_True_And_DateOnly_Is_Not_Provided()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventMissingDateOnlyFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.DateOnly)
                .WithErrorMessage("Event date must be provided.");
        }

        [Fact]
        public void Validator_Fails_When_AllDay_Is_True_And_DateOnly_Is_In_Past()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventPastDateOnlyFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.DateOnly)
                .WithErrorMessage("Event date and time must not be in the past.");
        }

        [Fact]
        public void Validator_Fails_When_AllDay_Is_False_And_DateOnly_Is_Provided()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventDateOnlyForNonAllDayFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.DateOnly)
                .WithErrorMessage("DateOnly must not be provided for non all day event.");
        }

        [Fact]
        public void Validator_Fails_When_AllDay_Is_True_And_DateOnly_Is_More_Than_5_Years_In_Future()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventFarFutureDateOnlyFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.DateOnly)
                .WithErrorMessage("Event date must not be more than 5 years in the future.");
        }
        #endregion

        #region StartDateTime Validation Tests
        [Fact]
        public void Validator_Fails_When_AllDay_Is_False_And_StartDateTime_Is_Not_Provided()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventMissingStartDateTimeFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.StartDateTime)
                .WithErrorMessage("Event start date must be provided.");
        }

        [Fact]
        public void Validator_Fails_When_AllDay_Is_False_And_StartDateTime_Is_In_Past()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventPastStartDateTimeFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.StartDateTime)
                .WithErrorMessage("Event date and time must not be in the past.");
        }

        [Fact]
        public void Validator_Fails_When_AllDay_Is_True_And_StartDateTime_Is_Provided()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventStartDateTimeForAllDayFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.StartDateTime)
                .WithErrorMessage("Event start date and time must not be provided for all day event.");
        }
        #endregion

        #region EndDateTime Validation Tests
        [Fact]
        public void Validator_Fails_When_AllDay_Is_False_And_EndDateTime_Is_Not_Provided()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventMissingEndDateTimeFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.EndDateTime)
                .WithErrorMessage("Event end date must be provided.");
        }

        [Fact]
        public void Validator_Fails_When_AllDay_Is_True_And_EndDateTime_Is_Provided()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventEndDateTimeForAllDayFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.EndDateTime)
                .WithErrorMessage("Event end date and time must not be provided for all day event.");
        }
        #endregion

        #region DateTime Relationship Validation Tests
        [Fact]
        public void Validator_Fails_When_StartDateTime_Is_After_Or_Equal_To_EndDateTime()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventEndBeforeStartFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e)
                .WithErrorMessage("Event start date must be earlier than event end date.");
        }

        [Fact]
        public void Validator_Fails_When_Event_Duration_Exceeds_7_Days()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventTooLongDurationFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e)
                .WithErrorMessage("Event duration must not exceed 7 days.");
        }
        #endregion

        #region Description Validation Tests
        [Fact]
        public void Validator_Fails_When_Description_Exceeds_500_Characters()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventLongDescriptionFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.Description)
                .WithErrorMessage("Description must not exceed 500 characters.");
        }
        #endregion

        #region Location Validation Tests
        [Fact]
        public void Validator_Fails_When_Location_Has_Invalid_Coordinates()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetInvalidEventLocationFixture();

            // Act
            var result = _validator.TestValidate(invalidEvent);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.Location)
                .WithErrorMessage("Location must have valid latitude and longitude values.");
        }
        #endregion

        #region Integration Tests with ValidationMappingMiddleware
        [Fact]
        public async Task ValidationMiddleware_Returns_ValidationFailureResponse_For_InvalidEvent()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetMultipleValidationFailuresFixture();
            var validationResult = await _validator.ValidateAsync(invalidEvent);
            var exception = new ValidationException(validationResult.Errors);

            var httpContext = new DefaultHttpContext();
            var responseStream = new System.IO.MemoryStream();
            httpContext.Response.Body = responseStream;
            var mockNextMiddleware = new Mock<RequestDelegate>();
            mockNextMiddleware
                .Setup(next => next(It.IsAny<HttpContext>()))
                .ThrowsAsync(exception);

            var middleware = new Calendara.Api.Mapping.ValidationMappingMiddleware(mockNextMiddleware.Object);

            // Act
            await middleware.InvokeAsync(httpContext);

            // Assert
            httpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            responseStream.Position = 0;
            var reader = new System.IO.StreamReader(responseStream);
            var responseBody = await reader.ReadToEndAsync();
            responseBody.Should().Contain("property_name");
            responseBody.Should().Contain("message");
        }

        [Fact]
        public void ValidationFailureResponse_Should_Contain_All_Validation_Errors()
        {
            // Arrange
            var invalidEvent = EventsFixtures.GetMultipleValidationFailuresFixture();

            // Act
            var result = _validator.Validate(invalidEvent);
            var errors = result.Errors;

            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = errors.Select(x => new ValidationResponse
                {
                    property_name = x.PropertyName,
                    message = x.ErrorMessage
                })
            };

            // Assert
            validationFailureResponse.Errors.Should().NotBeEmpty();
            validationFailureResponse.Errors.Count().Should().BeGreaterThan(1);
            validationFailureResponse.Errors.Should().Contain(e => e.property_name == "Title");
            validationFailureResponse.Errors.Should().Contain(e => e.property_name == "DateOnly");
            validationFailureResponse.Errors.Should().Contain(e => e.property_name == "StartDateTime");
            validationFailureResponse.Errors.Should().Contain(e => e.property_name == "EndDateTime");
            validationFailureResponse.Errors.Should().Contain(e => e.property_name == "Description");
            validationFailureResponse.Errors.Should().Contain(e => e.property_name == "Location");
        }
        #endregion

        #region Valid Event Tests
        [Fact]
        public void Validator_Succeeds_When_AllDay_Event_Is_Valid()
        {
            // Arrange
            var validEvent = EventsFixtures.GetTestFixture2(); 

            // Act
            var result = _validator.TestValidate(validEvent);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_Succeeds_When_NonAllDay_Event_Is_Valid()
        {
            // Arrange
            var validEvent = EventsFixtures.GetTestFixture(); 

            // Act
            var result = _validator.TestValidate(validEvent);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
        #endregion
    }
}
