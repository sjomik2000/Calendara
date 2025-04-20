using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Calendara.Application.Models;

namespace Calendara.Application.Validators
{
    public class EventValidator : AbstractValidator<Event>
    {
        public EventValidator()
        {
            // Title validation
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title must be provided.");
            RuleFor(x => x.Title)
                .MaximumLength(100)
                .WithMessage("Title must not exceed 100 characters.");

            // AllDay validation
            RuleFor(x => x.AllDay)
                .Must(value => true)
                .WithMessage("AllDay boolean must be provided.");

            // DateOnly validation
            RuleFor(x => x.DateOnly)
                .NotEmpty()
                .When(x => x.AllDay == true)
                .WithMessage("Event date must be provided.");
            RuleFor(x => x.DateOnly)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .When(x => x.AllDay == true)
                .WithMessage("Event date and time must not be in the past.");
            RuleFor(x => x.DateOnly)
                .Null()
                .When(x => x.AllDay == false)
                .WithMessage("DateOnly must not be provided for non all day event.");
            RuleFor(x => x.DateOnly)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(5)))
                .When(x => x.AllDay == true)
                .WithMessage("Event date must not be more than 5 years in the future.");

            // StartDateTime validation
            RuleFor(x => x.StartDateTime)
                .NotEmpty()
                .When(x => x.AllDay == false)
                .WithMessage("Event start date must be provided.");
            RuleFor(x => x.StartDateTime)
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .When(x => x.AllDay == false)
                .WithMessage("Event date and time must not be in the past.");
            RuleFor(x => x.StartDateTime)
                .Null()
                .When(x => x.AllDay == true)
                .WithMessage("Event start date and time must not be provided for all day event.");

            // EndDateTime validation
            RuleFor(x => x.EndDateTime)
                .NotEmpty()
                .When(x => x.AllDay == false)
                .WithMessage("Event end date must be provided.");
            RuleFor(x => x.EndDateTime)
                .Null()
                .When(x => x.AllDay == true)
                .WithMessage("Event end date and time must not be provided for all day event.");

            // DateTime relationship validation
            RuleFor(x => x)
                .Must(e => e.StartDateTime < e.EndDateTime)
                .When(x => x.AllDay == false)
                .WithMessage("Event start date must be earlier than event end date.");
            RuleFor(x => x)
                .Must(e => e.EndDateTime.Value.Subtract(e.StartDateTime.Value).TotalDays <= 7)
                .When(x => x.AllDay == false && x.StartDateTime.HasValue && x.EndDateTime.HasValue)
                .WithMessage("Event duration must not exceed 7 days.");

            // Description validation
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("Description must not exceed 500 characters.");

            // Location validation
            RuleFor(x => x.Location)
                .Must(location => location.Latitude >= -90 && location.Latitude <= 90 && location.Longitude >= -180 && location.Longitude <= 180)
                .When(x => x.Location != null)
                .WithMessage("Location must have valid latitude and longitude values.");

        }
    }
}
