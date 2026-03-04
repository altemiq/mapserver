// -----------------------------------------------------------------------
// <copyright file="MapValidator.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Validation;

/// <summary>
/// The <see cref="Map"/> <see cref="FluentValidation.IValidator"/>.
/// </summary>
public class MapValidator : FluentValidation.AbstractValidator<Map>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="MapValidator"/> class.
    /// </summary>
    public MapValidator()
    {
        // Basic size checks
        this.RuleFor(static m => m.Size.Width)
            .GreaterThan(0)
            .WithMessage("MAP SIZE width must be > 0.");
        this.RuleFor(static m => m.Size.Height)
            .GreaterThan(0)
            .WithMessage("MAP SIZE height must be > 0.");

        // EXTENT sanity checks + UNITS recommendation as a WARNING
        this.RuleFor(static m => m.Extent)
            .Must(e => e!.Value.MinX < e.Value.MaxX)
            .Must(e => e!.Value.MinY < e.Value.MaxY)
            .When(static m => m.Extent.HasValue)
            .WithMessage("MAP EXTENT must satisfy (minx < maxx) and (miny < maxy).");

        this.RuleFor(static m => m.Units)
            .IsInEnum()
            .When(static m => m.Extent.HasValue)
            .WithMessage("UNITS is recommended when EXTENT is set.")
            .WithSeverity(Severity.Warning);

        // Recommend IMAGETYPE or OUTPUTFORMAT
        this.RuleFor(static m => m)
            .Must(m => !string.IsNullOrWhiteSpace(m.ImageType) || m.OutputFormats.Count > 0)
            .WithMessage("Neither IMAGETYPE nor OUTPUTFORMAT is set; MapServer will rely on defaults.")
            .WithSeverity(Severity.Warning);

        this.RuleFor(static m => m.Resolution)
            .InclusiveBetween(10, 1000);

        this.RuleFor(static m => m.DefResolution)
            .InclusiveBetween(10, 1000);

        // Children
        this.RuleForEach(static m => m.OutputFormats)
            .SetValidator(new OutputFormatValidator());
        this.RuleForEach(static m => m.Layers)
            .SetValidator(new LayerValidator());
    }
}