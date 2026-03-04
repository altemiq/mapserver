// -----------------------------------------------------------------------
// <copyright file="VersionedMapValidator.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Validation;

/// <summary>
/// The versioned <see cref="Map"/> <see cref="FluentValidation.IValidator"/>.
/// </summary>
public class VersionedMapValidator : AbstractValidator<Map>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="VersionedMapValidator"/> class.
    /// </summary>
    /// <param name="options">The validation options.</param>
    public VersionedMapValidator(VersionedValidationOptions options)
    {
        // Reuse base validator first (structure/semantics not tied to version)
        this.Include(new MapValidator());

        // Validate each layer with version-aware rules
        this.RuleForEach(m => m.Layers)
            .SetValidator(new VersionedLayerValidator(options));
    }
}