// -----------------------------------------------------------------------
// <copyright file="ConnectionType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// LAYER CONNECTIONTYPE values (data provider / pipeline).
/// </summary>
public enum ConnectionType
{
    /// <summary>
    /// Local connection type.
    /// </summary>
    Local,

    /// <summary>
    /// OGC connection type.
    /// </summary>
    Ogr,

    /// <summary>
    /// Oracle spatial connection type.
    /// </summary>
    OracleSpatial,

    /// <summary>
    /// Plugin connection type.
    /// </summary>
    Plugin,

    /// <summary>
    /// PostGIS connection type.
    /// </summary>
    PostGIS,

    /// <summary>
    /// SDE connection type.
    /// </summary>
    Sde,

    /// <summary>
    /// Union connection type.
    /// </summary>
    Union,

    /// <summary>
    /// Raster connection type.
    /// </summary>
    Raster,

    /// <summary>
    /// Raster label connection type.
    /// </summary>
    RasterLabel,

    /// <summary>
    /// UV raster connection type.
    /// </summary>
    UvRaster,

    /// <summary>
    /// WFS connection type.
    /// </summary>
    Wfs,

    /// <summary>
    /// WMS connection type.
    /// </summary>
    Wms,

    /// <summary>
    /// IDW connection type.
    /// </summary>
    Idw,

    /// <summary>
    /// Contour connection type.
    /// </summary>
    Contour,

    /// <summary>
    /// Kernel density connection type.
    /// </summary>
    KernelDensity,
}