// -----------------------------------------------------------------------
// <copyright file="MapStatus.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Discrete status flags used across MAP, LAYER, LEGEND, SCALEBAR, and QUERYMAP.
/// </summary>
/// <remarks>
/// MapServer 8.0 supports <c>ON</c>, <c>OFF</c>, and <c>DEFAULT</c> for most objects.
/// <c>EMBED</c> is only valid where explicitly supported (e.g., LEGEND, SCALEBAR).
/// </remarks>
public enum MapStatus
{
    /// <summary>
    /// Enabled / drawn / active (<c>ON</c>).
    /// </summary>
    On = 0,

    /// <summary>
    /// Disabled / not drawn (<c>OFF</c>).
    /// </summary>
    Off = 1,

    /// <summary>
    /// Use MapServer’s default behavior (<c>DEFAULT</c>).
    /// Applicable mainly for LAYER status.
    /// </summary>
    Default = 2,

    /// <summary>
    /// Embed the object (LEGEND/SCALEBAR) into the main map image (<c>EMBED</c>).
    /// </summary>
    Embed = 3,
}