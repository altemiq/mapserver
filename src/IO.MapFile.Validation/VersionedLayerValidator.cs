// -----------------------------------------------------------------------
// <copyright file="VersionedLayerValidator.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Validation;

/// <summary>
/// The versioned <see cref="Layer"/> <see cref="FluentValidation.IValidator"/>.
/// </summary>
public sealed class VersionedLayerValidator : AbstractValidator<Layer>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="VersionedLayerValidator"/> class.
    /// </summary>
    /// <param name="options">The validation options.</param>
    public VersionedLayerValidator(VersionedValidationOptions options)
    {
        // Start with non-versioned rules
        this.Include(new LayerValidator());

        // 1) CONNECTIONOPTIONS: allow only when MapServer >= 7.6
        this.RuleFor(l => l.ConnectionOptions)
            .Empty()
            .When(_ => !options.Target.IsAtLeast(7, 6))
            .WithMessage($"CONNECTIONOPTIONS not supported in MapServer {options.Target}. This keyword was introduced in late 7.x and is fully documented for 8.x.");

        // 2) Native SDE driver removed at 7.0 (still available via OGR)
        this.RuleFor(l => l.ConnectionType)
            .NotEqual(ConnectionType.Sde)
            .When(_ => options.Target.IsAtLeast(7, 0))
            .WithMessage("Native SDE driver was removed in MapServer 7.0. Use OGR instead (CONNECTIONTYPE OGR).");

        this.RuleFor(static l => l.Tolerance)
            .NotNull()
            .When(_ => options.Target.IsAtLeast(8, 6))
            .WithMessage("Tolerance was removed in MapServer 8.6. Use IDENTITY TOLERANCE instead.");
    }
}