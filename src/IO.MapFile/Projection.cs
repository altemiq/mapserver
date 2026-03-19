// -----------------------------------------------------------------------
// <copyright file="Projection.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// PROJECTION — declares the coordinate system using PROJ parameters or EPSG codes.
/// </summary>
/// <remarks>
/// Prefer EPSG codes (e.g., <c>init=epsg:4326</c>) when available; inline PROJ strings are also supported.
/// </remarks>
public sealed class Projection
{
    /// <summary>
    /// Gets or sets a value indicating whether to use PROJECTION AUTO (rare).
    /// Leave <see langword="null"/> for explicit EPSG/PROJ parameter definitions.
    /// </summary>
    public bool? Auto { get; set; }

    /// <summary>
    /// Gets the PROJ parameter lines as declared (e.g., <c>"init=epsg:4326"</c>, <c>"proj=utm"</c> …).
    /// </summary>
    public IList<string> Parameters { get; } = [];
}