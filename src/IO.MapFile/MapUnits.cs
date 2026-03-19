// -----------------------------------------------------------------------
// <copyright file="MapUnits.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Units used for distances, sizes, tolerances, scale computations, and other
/// measurement‑based parameters in MapServer.
/// </summary>
public enum MapUnits
{
    /// <summary>
    /// Pixel units (<c>PIXELS</c>).
    /// </summary>
    Pixels,

    /// <summary>
    /// Feet (<c>FEET</c>).
    /// </summary>
    Feet,

    /// <summary>
    /// Inches (<c>INCHES</c>).
    /// </summary>
    Inches,

    /// <summary>
    /// Kilometers (<c>KILOMETERS</c>).
    /// </summary>
    Kilometers,

    /// <summary>
    /// Meters (<c>METERS</c>).
    /// </summary>
    Meters,

    /// <summary>
    /// Miles (<c>MILES</c>).
    /// </summary>
    Miles,

    /// <summary>
    /// Nautical miles (<c>NAUTICALMILES</c>).
    /// </summary>
    NauticalMiles,

    /// <summary>
    /// Decimal degrees (<c>DD</c>).
    /// </summary>
    DD,
}