// -----------------------------------------------------------------------
// <copyright file="MapStatus.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Discrete status switches used across objects (MAP/LAYER defaulting, or EMBED in legends/scalebars).
/// </summary>
public enum MapStatus
{
    On = 0,
    Off = 1,
    /// <summary>Use default behavior (e.g., draw in all cases). Valid for LAYER.</summary>
    Default = 2,
    /// <summary>Embed object into main map image (valid where supported, e.g., LEGEND/SCALEBAR).</summary>
    Embed = 3,
}