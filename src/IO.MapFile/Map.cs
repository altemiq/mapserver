// -----------------------------------------------------------------------
// <copyright file="Map.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>Root <c>MAP</c> object: global state (extent, size, DPI), defaults (e.g., <c>IMAGETYPE</c>),
/// child objects (LAYERs, OUTPUTFORMATs, PROJECTION, WEB, LEGEND/SCALEBAR, etc.).
/// </summary>
public sealed class Map
{
    /// <summary>Gets or sets the unique map name (<c>NAME</c>).</summary>
    public string? Name { get; set; }

    /// <summary>Gets or sets the map drawing status (<c>STATUS</c>).</summary>
    public MapStatus Status { get; set; }

    /// <summary>Gets or sets the map image size in pixels (<c>SIZE</c>).</summary>
    public Size Size { get; set; }

    /// <summary>Gets or sets the maximum allowed WIDTH/HEIGHT in pixels for requests (<c>MAXSIZE</c>).</summary>
    public int? MaxSize { get; set; }

    /// <summary>Gets or sets the initial display/map extent (<c>EXTENT</c>).</summary>
    public BoundingBox? Extent { get; set; }

    /// <summary>Gets or sets the coordinate units for the map (<c>UNITS</c>).</summary>
    public MapUnits? Units { get; set; }

    /// <summary>
    /// Gets or sets the output DPI used for scale computations (<c>RESOLUTION</c>).
    /// </summary>
    [DefaultValue(72)]
    public int Resolution { get; set; } = 72;

    /// <summary>
    /// Gets or sets the reference DPI used for symbology scaling (<c>DEFRESOLUTION</c>).
    /// Scale factor is <c>Resolution / DefResolution</c>.
    /// </summary>
    [DefaultValue(72)]
    public int DefResolution { get; set; } = 72;

    /// <summary>
    /// Gets or sets the default image type (<c>IMAGETYPE</c>)—must match an <see cref="OutputFormat.Name"/>.
    /// </summary>
    public string? ImageType { get; set; }

    /// <summary>Gets or sets the background color (<c>IMAGECOLOR</c>).</summary>
    public Color? ImageColor { get; set; }

    /// <summary>Gets or sets the optional global background color.</summary>
    public Color? BackgroundColor { get; set; }

    /// <summary>Gets or sets the symbol set file path (<c>SYMBOLSET</c>).</summary>
    public string? SymbolSet { get; set; }

    /// <summary>Gets or sets the fontset file path for TrueType fonts (<c>FONTSET</c>).</summary>
    public string? FontSet { get; set; }

    /// <summary>Gets or sets the root path used to resolve relative layer <c>DATA</c> paths (<c>SHAPEPATH</c>).</summary>
    public string? ShapePath { get; set; }

    /// <summary>Gets or sets the global debug level 0..5 (<c>DEBUG</c>).</summary>
    public int? DebugLevel { get; set; }

    /// <summary>Gets the environment and system configuration (<c>CONFIG</c> entries).</summary>
    public IDictionary<string, string> Config { get; } =
        new Dictionary<string, string>(System.StringComparer.Ordinal);

    /// <summary>Gets the free-form metadata (<c>METADATA</c>) available to templates and OGC services.</summary>
    public IDictionary<string, string> Metadata { get; } =
        new Dictionary<string, string>(System.StringComparer.Ordinal);

    /// <summary>Gets or sets the map-level projection (<c>PROJECTION</c>).</summary>
    public Projection? Projection { get; set; }

    /// <summary>Gets or sets the WEB block (CGI paths, formats, templates, etc.).</summary>
    public Web? Web { get; set; }

    /// <summary>Gets the declared output formats (<c>OUTPUTFORMAT</c> blocks).</summary>
    public IList<OutputFormat> OutputFormats { get; } = [];

    /// <summary>Gets or sets the legend block (<c>LEGEND</c>).</summary>
    public Legend? Legend { get; set; }

    /// <summary>Gets or sets the scalebar block (<c>SCALEBAR</c>).</summary>
    public ScaleBar? ScaleBar { get; set; }

    /// <summary>Gets or sets the reference (overview) map configuration (<c>REFERENCE</c>).</summary>
    public Reference? Reference { get; set; }

    /// <summary>Gets or sets the query map configuration (<c>QUERYMAP</c>).</summary>
    public QueryMap? QueryMap { get; set; }

    /// <summary>Gets the child layers rendered top-to-bottom (<c>LAYER</c>).</summary>
    public IList<Layer> Layers { get; } = [];
}