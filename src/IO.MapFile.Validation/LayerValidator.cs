// -----------------------------------------------------------------------
// <copyright file="LayerValidator.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Validation;

/// <summary>
/// The <see cref="Layer"/> <see cref="FluentValidation.IValidator"/>.
/// </summary>
public sealed class LayerValidator : AbstractValidator<Layer>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="LayerValidator"/> class.
    /// </summary>
    public LayerValidator()
    {
        this.RuleFor(static l => l.Name)
            .NotEmpty()
            .WithMessage("LAYER NAME is required.");

        // Ensure TYPE is a defined enum value (explicitness)
        this.RuleFor(static l => l.Type)
            .IsInEnum()
            .WithMessage("LAYER TYPE is required.");

        // Require either DATA or (CONNECTIONTYPE + CONNECTION)
        this.RuleFor(static l => l)
            .Must(static l => !string.IsNullOrWhiteSpace(l.Data) || !string.IsNullOrWhiteSpace(l.Connection))
            .WithMessage("Specify DATA or (CONNECTIONTYPE + CONNECTION) for the LAYER.");

        // Scale denominators
        this.RuleFor(static l => l)
            .Must(static l => !l.MinScaleDenom.HasValue || !l.MaxScaleDenom.HasValue || l.MinScaleDenom <= l.MaxScaleDenom)
            .WithMessage("MINSCALEDENOM must be <= MAXSCALEDENOM.");

        // TOLERANCE >= 0
        this.RuleFor(static l => l.Tolerance)
            .GreaterThanOrEqualTo(0)
            .When(static l => l.Tolerance.HasValue)
            .WithMessage("TOLERANCE must be >= 0.");

        // PROCESSING lines should look like key=value (warning)
        this.RuleForEach(static l => l.Processing)
            .Must(static p => string.IsNullOrWhiteSpace(p) || p.Contains('=', StringComparison.Ordinal))
            .WithMessage("PROCESSING entry should be of the form key=value.")
            .WithSeverity(Severity.Warning);

        // PROJECTION (if present) must contain at least one parameter line
        this.RuleFor(static l => l.Projection)
            .Must(static proj => proj is null or { Parameters.Count: not 0 });

        // Children
        this.RuleForEach(static l => l.Classes)
            .SetValidator(new ClassValidator());
    }
}