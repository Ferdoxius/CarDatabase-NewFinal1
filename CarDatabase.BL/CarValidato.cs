using CarDatabase.Models;
using FluentValidation;

namespace CarDatabase.BL
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(car => car.Brand)
                .NotEmpty().WithMessage("Марка е задължителна")
                .MaximumLength(50).WithMessage("Марка не може да надвишава 50 символа");

            RuleFor(car => car.Model)
                .NotEmpty().WithMessage("Моделът е задължителен")
                .MaximumLength(50).WithMessage("Моделът не може да надвишава 50 символа");

            RuleFor(car => car.Year)
                .InclusiveBetween(1886, DateTime.Now.Year).WithMessage("Годината трябва да е валидна");

            RuleFor(car => car.Price)
                .GreaterThan(0).WithMessage("Цената трябва да бъде положителна стойност");
        }
    }
}
