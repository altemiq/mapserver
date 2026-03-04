// -----------------------------------------------------------------------
// <copyright file="LabelValidator.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Validation;

/// <summary>
/// The <see cref="Label"/> <see cref="FluentValidation.IValidator"/>.
/// </summary>
public sealed class LabelValidator : AbstractValidator<Label>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="LabelValidator"/> class.
    /// </summary>
    public LabelValidator()
    {
        this.RuleFor(static l => l.Size)
            .Must(s => int.Parse(s!, System.Globalization.CultureInfo.InvariantCulture) > 0)
            .When(static l => int.TryParse(l.Size, System.Globalization.CultureInfo.InvariantCulture, out _))
            .WithMessage("LABEL SIZE must be > 0 when set.");

        this.RuleFor(static l => l.MinDistance)
            .GreaterThanOrEqualTo(0)
            .When(static l => l.MinDistance.HasValue)
            .WithMessage("LABEL MINDISTANCE must be >= 0.");

        this.RuleFor(static l => l.MinFeatureSize)
            .GreaterThanOrEqualTo(0)
            .When(static l => l.MinFeatureSize.HasValue)
            .WithMessage("LABEL MINFEATURESIZE must be >= 0.");
    }
}