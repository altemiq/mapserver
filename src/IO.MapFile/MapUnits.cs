// -----------------------------------------------------------------------
// <copyright file="MapUnits.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Map distance or tolerance units.
/// </summary>
public enum MapUnits
{
    /// <summary>
    /// Pixel units.
    /// </summary>
    Pixels,

    /// <summary>
    /// Feet units.
    /// </summary>
    Feet,

    /// <summary>
    /// Inches units.
    /// </summary>
    Inches,

    /// <summary>
    /// Kilometer units.
    /// </summary>
    Kilometers,

    /// <summary>
    /// Meter units.
    /// </summary>
    Meters,

    /// <summary>
    /// Mile units.
    /// </summary>
    Miles,

    /// <summary>
    /// Nautical mile units.
    /// </summary>
    NauticalMiles,

    /// <summary>Decimal degrees.</summary>
    DD,
}