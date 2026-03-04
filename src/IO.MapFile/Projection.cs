// -----------------------------------------------------------------------
// <copyright file="Projection.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;
/// <summary>
/// PROJECTION – declares the coordinate system using PROJ parameters (prefer EPSG when available).
/// </summary>
/// <remarks>
/// You can use EPSG via "init=epsg:XXXX" or inline PROJ strings. Starting with MapServer 8 and PROJ≥6,
/// EPSG is recommended for higher accuracy reprojection.
/// </remarks>
public sealed class Projection
{
    /// <summary>
    /// Gets or sets set true to use PROJECTION AUTO (rare). Leave null for explicit params/EPSG.
    /// </summary>
    public bool? Auto { get; set; }

    /// <summary>
    /// Gets pROJ parameter lines as declared (e.g., "init=epsg:4326" or "proj=utm", ...).
    /// </summary>
    public List<string> Parameters { get; } = [];
}