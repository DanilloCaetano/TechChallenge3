using FluentValidation;
using System.Text.Json.Serialization;

namespace TechChallenge1.Controllers.Contacts.Dto
{
    public class ContactPostDto
    {
        /// <summary>
        /// User email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// User status
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        /// <summary>
        /// CellphoneNumber
        /// </summary>
        [JsonPropertyName("cellphoneNumber")]
        public string CellphoneNumber { get; set; }


        /// <summary>
        /// RegionId
        /// </summary>
        [JsonPropertyName("regionId")]
        public Guid RegionId { get; set; }

    }

    public class ContactPostValidate : AbstractValidator<ContactPostDto>
    {
        public ContactPostValidate()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("email must not be null")
                .NotEmpty().WithMessage("email must not be empty")
                .EmailAddress().WithMessage("email is not valid");

            RuleFor(x => x.Name)
                .NotNull().WithMessage("name must not be null")
                .NotEmpty().WithMessage("name must not be empty");

            RuleFor(x => x.Active)
                .NotNull().WithMessage("status must not be null");

            RuleFor(x => x.CellphoneNumber)
                .NotNull().WithMessage("cellphone number must not be null")
                .Must(x => x.Length == 9).WithMessage("cellphone number must have 9 digits");

            RuleFor(x => x.RegionId)
                .NotNull().WithMessage("ddd must not be null")
                .Must(x => x != Guid.Empty).WithMessage("RegionId must not be an empty guid");
        }
    }
}
