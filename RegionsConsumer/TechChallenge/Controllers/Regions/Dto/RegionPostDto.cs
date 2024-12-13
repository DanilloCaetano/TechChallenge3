using FluentValidation;
using System.Text.Json.Serialization;

namespace TechChallenge.Controllers.Regions.Dto
{
    public class RegionPostDto
    {
        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// ddd
        /// </summary>
        [JsonPropertyName("ddd")]
        public string DDD { get; set; }

        /// <summary>
        /// User status
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }

    public class RegionPostValidate : AbstractValidator<RegionPostDto>
    {
        public RegionPostValidate()
        {
            RuleFor(x => x.Description)
                .NotNull().WithMessage("description must not be null")
                .NotEmpty().WithMessage("description must not be empty");

            RuleFor(x => x.DDD)
                .NotNull().WithMessage("ddd must not be null")
                .NotEmpty().WithMessage("ddd must not be empty");

            RuleFor(x => x.Active)
                .NotNull().WithMessage("status must not be null");
        }
    }
}
