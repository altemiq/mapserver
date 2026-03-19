// -----------------------------------------------------------------------
// <copyright file="Reference.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// REFERENCE block — miniature locator map and highlight box settings.
/// </summary>
public sealed class Reference
{
    /// <summary>
    /// Gets or sets the fill color for the current map extent box drawn on the reference image (<c>COLOR</c>).
    /// </summary>
    public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets the outline color for the current map extent box (<c>OUTLINECOLOR</c>).
    /// </summary>
    public Color? OutlineColor { get; set; }

    /// <summary>
    /// Gets or sets the extent of the base reference image (<c>EXTENT</c>).
    /// </summary>
    public BoundingBox? Extent { get; set; }

    /// <summary>
    /// Gets or sets the size (width × height in pixels) of the base reference image (<c>SIZE</c>).
    /// </summary>
    public Size? Size { get; set; }

    /// <summary>
    /// Gets or sets the file path/URL of the background image used for the reference map (<c>IMAGE</c>).
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the symbol name (or "0" for default) used to mark the map center (<c>MARKER</c>).
    /// </summary>
    public string? Marker { get; set; }

    /// <summary>
    /// Gets or sets the marker size in pixels (<c>MARKERSIZE</c>).
    /// </summary>
    public int? MarkerSize { get; set; }

    /// <summary>
    /// Gets or sets the minimum pixel size of the drawn extent box (<c>MINBOXSIZE</c>).
    /// </summary>
    public int? MinBoxSize { get; set; }

    /// <summary>
    /// Gets or sets the maximum pixel size of the drawn extent box (<c>MAXBOXSIZE</c>).
    /// </summary>
    public int? MaxBoxSize { get; set; }

    /// <summary>
    /// Gets or sets whether the reference map is rendered (<c>STATUS</c>).
    /// </summary>
    [DefaultValue(MapStatus.Off)]
    public MapStatus Status { get; set; } = MapStatus.Off;
}