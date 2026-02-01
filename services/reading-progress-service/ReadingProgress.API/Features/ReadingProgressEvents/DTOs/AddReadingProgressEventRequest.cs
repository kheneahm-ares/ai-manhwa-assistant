using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace ReadingProgress.API.Features.ReadingProgressEvents.DTOs
{
    public class AddReadingProgressEventValidator : AbstractValidator<AddReadingProgressEventRequest>
    {
        public AddReadingProgressEventValidator()
        {
            RuleFor(x => x.ManhwaId)
                .NotEmpty().WithMessage("ManhwaId is required.");
            RuleFor(x => x.ChapterNumber)
                .GreaterThan(0).WithMessage("Chapter number must be greater than or equal 0.");
        }
    }

    public class AddReadingProgressEventRequest
    {
        public string ManhwaId { get; set; }
        public decimal ChapterNumber { get; set; }
    }
}
