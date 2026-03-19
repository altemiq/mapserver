// -----------------------------------------------------------------------
// <copyright file="ConnectionType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// LAYER <c>CONNECTIONTYPE</c> values (data provider / pipeline).
/// </summary>
public enum ConnectionType
{
    /// <summary>
    /// Local datasource (<c>local</c>).
    /// </summary>
    Local,

    /// <summary>
    /// OGR driver (<c>ogr</c>).
    /// </summary>
    Ogr,

    /// <summary>
    /// Oracle Spatial (<c>oraclespatial</c>).
    /// </summary>
    OracleSpatial,

    /// <summary>
    /// Plugin (<c>plugin</c>).
    /// </summary>
    Plugin,

    /// <summary>
    /// PostGIS (<c>postgis</c>).
    /// </summary>
    PostGIS,

    /// <summary>
    /// ESRI SDE (<c>sde</c>). Native SDE driver was removed in MapServer 7.0; SDE access is typically via OGR.
    /// </summary>
    Sde,

    /// <summary>
    /// Union layer (<c>union</c>).
    /// </summary>
    Union,

    /// <summary>
    /// Raster via GDAL (<c>raster</c>).
    /// </summary>
    Raster,

    /// <summary>
    /// Raster label (<c>rasterlabel</c>).
    /// </summary>
    RasterLabel,

    /// <summary>
    /// UV raster (<c>uvraster</c>).
    /// </summary>
    UvRaster,

    /// <summary>
    /// OGC WFS client (<c>wfs</c>).
    /// </summary>
    Wfs,

    /// <summary>
    /// OGC WMS client (<c>wms</c>).
    /// </summary>
    Wms,

    /// <summary>
    /// Inverse Distance Weighted interpolator (<c>idw</c>).
    /// </summary>
    Idw,

    /// <summary>
    /// Contour generator (<c>contour</c>).
    /// </summary>
    Contour,

    /// <summary>
    /// Kernel density (dynamic heatmap) (<c>kerneldensity</c>).
    /// </summary>
    KernelDensity,
}