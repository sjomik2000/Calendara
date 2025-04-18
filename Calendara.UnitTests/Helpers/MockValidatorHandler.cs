using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Threading;

namespace Calendara.UnitTests.Helpers
{
    public static class ValidatorMockSetup
    {
        public static void SetupValidationSuccess<T>(Mock<IValidator<T>> mockValidator) where T : class
        {
            mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<T>>(), default))
                .ReturnsAsync(new ValidationResult());

            mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<T>(), default))
                .ReturnsAsync(new ValidationResult());
        }

        public static void SetupValidationFailure<T>(Mock<IValidator<T>> mockValidator, List<ValidationFailure> failures) where T : class
        {
            var failureResult = new ValidationResult(failures);

            mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<T>>(), default))
                .ReturnsAsync(failureResult);

            mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<T>(), default))
                .ReturnsAsync(failureResult);

            mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<T>(), default))
                .Callback<T, CancellationToken>((obj, token) => {
                    throw new ValidationException(failures);
                });

            mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<T>>(), default))
                .Callback<ValidationContext<T>, CancellationToken>((context, token) => {
                    throw new ValidationException(failures);
                });
        }

        public static void SetupThrowValidationException<T>(Mock<IValidator<T>> mockValidator, List<ValidationFailure> failures) where T : class
        {
            mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<T>(), default))
                .ThrowsAsync(new ValidationException(failures));

            mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<T>>(), default))
                .ThrowsAsync(new ValidationException(failures));
        }
    }
}

