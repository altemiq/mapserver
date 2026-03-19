// -----------------------------------------------------------------------
// <copyright file="Identify.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// IDENTIFY (added in MapServer 8.6) — controls feature identification sensitivity and rules.
/// </summary>
/// <remarks>
/// This type is present to ease forward‑compatibility, but is not available in MapServer 8.0.
/// Use LAYER‑level <c>TOLERANCE</c>/<c>TOLERANCEUNITS</c> in 8.0.
/// </remarks>
public sealed class Identify
{
    /// <summary>
    /// Gets or sets the distance/radius used for point‑based queries (<c>TOLERANCE</c>).
    /// </summary>
    public double? Tolerance { get; set; }

    /// <summary>
    /// Gets or sets the units for <see cref="Tolerance"/> (<c>TOLERANCEUNITS</c>).
    /// </summary>
    [DefaultValue(MapUnits.Pixels)]
    public MapUnits ToleranceUnits { get; set; } = MapUnits.Pixels;

    /// <summary>
    /// Gets or sets a value indicating whether to use symbol shapes from the active styles
    /// for pixel‑precise identification on POINT layers with SYMBOL (<c>CLASSAUTO</c>).
    /// </summary>
    public bool? ClassAuto { get; set; }

    /// <summary>
    /// Gets or sets a class group whose symbols should govern identification
    /// (mutually exclusive with <see cref="ClassAuto"/>; <c>CLASSGROUP</c>).
    /// </summary>
    public string? ClassGroup { get; set; }
}