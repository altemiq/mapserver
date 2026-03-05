// -----------------------------------------------------------------------
// <copyright file="Map.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

using System.Drawing;

/// <summary>
/// The root of a MapServer mapfile.
/// </summary>
/// <remarks>
/// See Mapfile index &amp; MAP object overview.
/// Contains global state, default output type, child objects (layers, outputs, web, projection, legend/scalebar, etc.).
/// </remarks>
public sealed class Map
{
    /// <summary>Gets or sets unique map name (often used in CGI requests).</summary>
    public string? Name { get; set; }

    /// <summary>Gets or sets on/Off; rarely needed at MAP level, but included for completeness.</summary>
    public MapStatus Status { get; set; }

    /// <summary>Gets or sets map image size in pixels (WIDTH, HEIGHT).</summary>
    public Size Size { get; set; }

    /// <summary>Gets or sets maximum allowed WIDTH/HEIGHT for requests (MAXSIZE in pixels).</summary>
    public int? MaxSize { get; set; }

    /// <summary>Gets or sets initial display extent (minx, miny, maxx, maxy).</summary>
    public BoundingBox? Extent { get; set; }

    /// <summary>Gets or sets map coordinate units (UNITS).</summary>
    public MapUnits? Units { get; set; }

    /// <summary>
    /// Gets or sets the output DPI used for scale computations. Default 72; valid range [10,1000].
    /// </summary>
    public int Resolution { get; set; } = 72;

    /// <summary>
    /// Gets or sets the reference DPI used for symbology scaling (the map “looks” the same when
    /// <see cref="Resolution"/> changes; scale factor = <c>Resolution/DefResolution</c>).
    /// Default 72; valid range [10,1000].
    /// </summary>
    public int DefResolution { get; set; } = 72;

    /// <summary>
    /// Gets or sets default image type (IMAGETYPE) – references an OUTPUTFORMAT NAME declared here.
    /// </summary>
    public string? ImageType { get; set; }

    /// <summary>Gets or sets background/fill color for the map image (IMAGECOLOR).</summary>
    public Color? ImageColor { get; set; }

    /// <summary>Gets or sets optional global background (can be used by some renderers).</summary>
    public Color? BackgroundColor { get; set; }

    /// <summary>Gets or sets symbol set file path (if using external symbol sets) or inline SYMBOL blocks.</summary>
    public string? SymbolSet { get; set; }

    /// <summary>Gets or sets fontset file path for TrueType fonts.</summary>
    public string? FontSet { get; set; }

    /// <summary>Gets or sets path used to resolve relative DATA references within layers (SHAPEPATH).</summary>
    public string? ShapePath { get; set; }

    /// <summary>Gets or sets global debugging level; 0..5 in modern MapServer (DEBUG).</summary>
    public int? DebugLevel { get; set; }

    /// <summary>Gets arbitrary environment and system configuration (CONFIG entries).</summary>
    public IDictionary<string, string> Config { get; internal init; } = new Dictionary<string, string>(StringComparer.Ordinal);

    /// <summary>Gets free-form metadata (METADATA) available to templates and OGC services.</summary>
    public IDictionary<string, string> Metadata { get; internal init; } = new Dictionary<string, string>(StringComparer.Ordinal);

    /// <summary>Gets or sets global PROJECTION for output.</summary>
    public Projection? Projection { get; set; }

    /// <summary>Gets or sets wEB block (CGI paths, formats, templates, etc.).</summary>
    public Web? Web { get; set; }

    /// <summary>Gets declared OUTPUTFORMATs; referenced by <see cref="ImageType"/>.</summary>
    public IList<OutputFormat> OutputFormats { get; internal init; } = [];

    /// <summary>Gets or sets legend block.</summary>
    public Legend? Legend { get; set; }

    /// <summary>Gets or sets scalebar block.</summary>
    public ScaleBar? ScaleBar { get; set; }

    /// <summary>Gets or sets reference map configuration.</summary>
    public Reference? Reference { get; set; }

    /// <summary>Gets or sets queryMap configuration.</summary>
    public QueryMap? QueryMap { get; set; }

    /// <summary>Gets child layers, evaluated top-to-bottom.</summary>
    public IList<Layer> Layers { get; internal init; } = [];
}