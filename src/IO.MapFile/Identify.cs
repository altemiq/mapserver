// -----------------------------------------------------------------------
// <copyright file="Identify.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// IDENTIFY block (MapServer 8.6+) – controls feature identification sensitivity/rules.
/// </summary>
public sealed class Identify
{
    /// <summary>Gets or sets distance/radius for point-based queries.</summary>
    public double? Tolerance { get; set; }

    /// <summary>Gets or sets units of <see cref="Tolerance"/>.</summary>
    public MapUnits ToleranceUnits { get; set; } = MapUnits.Pixels;

    /// <summary>Gets or sets use symbols from actual styles for pixel-precise identification (POINT layers with SYMBOL).</summary>
    public bool? ClassAuto { get; set; }

    /// <summary>Gets or sets group name whose class symbols govern identification (mutually exclusive with <see cref="ClassAuto"/>).</summary>
    public string? ClassGroup { get; set; }
}
