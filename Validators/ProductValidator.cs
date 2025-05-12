using FluentValidation;
using ProductAPI.Models;

namespace ProductAPI.Validators
{
    /// <summary>
    /// Validator for the Product model using FluentValidation
    /// </summary>
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            // Validation rules for the Name property
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required") // Name cannot be null or empty
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters"); // Limit name length

            // Validation rules for the Year property
            RuleFor(p => p.Data.Year)
                .GreaterThan(1900).WithMessage("Year must be after 1900") // Reasonable minimum year
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage($"Year cannot be in the future"); // Cannot be future year

            // Validation rules for the Price property
            RuleFor(p => p.Data.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0"); // Price must be positive
        }
    }
}