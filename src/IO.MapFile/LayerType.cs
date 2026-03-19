// -----------------------------------------------------------------------
// <copyright file="LayerType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Supported geometry types for a LAYER (<c>TYPE</c>).
/// </summary>
/// <remarks>
/// These values map directly to MapServer’s <c>TYPE</c> keyword:
/// <c>POINT</c>, <c>LINE</c>, <c>POLYGON</c>, <c>RASTER</c>, <c>ANNOTATION</c>, <c>CIRCLE</c>.
/// </remarks>
public enum LayerType
{
    /// <summary>
    /// Point or multipoint geometry (<c>POINT</c>).
    /// </summary>
    Point,

    /// <summary>
    /// Line or multiline geometry (<c>LINE</c>).
    /// </summary>
    Line,

    /// <summary>
    /// Polygon or multipolygon geometry (<c>POLYGON</c>).
    /// </summary>
    Polygon,

    /// <summary>
    /// Raster imagery (<c>RASTER</c>).
    /// </summary>
    Raster,

    /// <summary>
    /// Annotation / label‑only layers (<c>ANNOTATION</c>).
    /// </summary>
    Annotation,

    /// <summary>
    /// Circle geometry (<c>CIRCLE</c>).
    /// </summary>
    Circle,
}