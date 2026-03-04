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
    Pixels,
    Feet,
    Inches,
    Kilometers,
    Meters,
    Miles,
    NauticalMiles,
    /// <summary>Decimal degrees.</summary>
    DD,
}
