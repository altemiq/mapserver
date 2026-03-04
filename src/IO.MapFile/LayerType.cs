// -----------------------------------------------------------------------
// <copyright file="LayerType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Enumeration of layer geometry types.
/// </summary>
public enum LayerType
{
    /// <summary>
    /// Point layer type.
    /// </summary>
    Point,

    /// <summary>
    /// Line layer type.
    /// </summary>
    Line,

    /// <summary>
    /// Polygon layer type.
    /// </summary>
    Polygon,

    /// <summary>
    /// Raster layer type.
    /// </summary>
    Raster,

    /// <summary>
    /// Annotation layer type.
    /// </summary>
    Annotation,

    /// <summary>
    /// Circle layer type.
    /// </summary>
    Circle,
}