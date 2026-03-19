// -----------------------------------------------------------------------
// <copyright file="QueryMap.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// QUERYMAP block — controls how the special map image highlighting query
/// results is rendered (status, color, size, and style).
/// </summary>
public sealed class QueryMap
{
    /// <summary>
    /// Gets or sets whether the query map is drawn (<c>STATUS</c>).
    /// </summary>
    [DefaultValue(MapStatus.Off)]
    public MapStatus Status { get; set; } = MapStatus.Off;

    /// <summary>
    /// Gets or sets the highlight color for the query result overlay (<c>COLOR</c> / <c>HILITE</c>).
    /// </summary>
    public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets an optional size override (pixels) for the query map image (<c>SIZE</c>).
    /// When not set, the main MAP <c>SIZE</c> is used.
    /// </summary>
    public Size? Size { get; set; }

    /// <summary>
    /// Gets or sets the query map drawing style (<c>STYLE</c>).
    /// </summary>
    [DefaultValue(QueryMapStyle.Hilite)]
    public QueryMapStyle Style { get; set; } = QueryMapStyle.Hilite;
}