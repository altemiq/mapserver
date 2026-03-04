// -----------------------------------------------------------------------
// <copyright file="StyleValidator.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Validation;

/// <summary>
/// The <see cref="Style"/> <see cref="FluentValidation.IValidator"/>.
/// </summary>
public sealed class StyleValidator : AbstractValidator<Style>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="StyleValidator"/> class.
    /// </summary>
    public StyleValidator()
    {
        this.RuleFor(static s => s.Size)
            .GreaterThanOrEqualTo(0)
            .When(static s => s.Size.HasValue)
            .WithMessage("STYLE SIZE must be >= 0.");
        this.RuleFor(static s => s.Width)
            .GreaterThanOrEqualTo(0)
            .When(static s => s.Width.HasValue)
            .WithMessage("STYLE WIDTH must be >= 0.");
        this.RuleFor(static s => s.Opacity)
            .InclusiveBetween(0, 100)
            .When(static s => s.Opacity.HasValue)
            .WithMessage("STYLE OPACITY must be between 0 and 100.");

        // GEOMTRANSFORM is free-form, so no parsing here.
    }
}