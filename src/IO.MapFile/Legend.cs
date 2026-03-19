// -----------------------------------------------------------------------
// <copyright file="Legend.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// LEGEND block — automatic or embedded legend image and label formatting.
/// </summary>
public sealed partial class Legend
{
    /// <summary>
    /// Gets or sets whether the legend is rendered (<c>STATUS</c>).
    /// </summary>
    [DefaultValue(MapStatus.Off)]
    public MapStatus Status { get; set; } = MapStatus.Off;

    /// <summary>
    /// Gets or sets the legend background image color (<c>IMAGECOLOR</c>).
    /// </summary>
    public Color? ImageColor { get; set; }

    /// <summary>
    /// Gets or sets the legend outline color (<c>OUTLINECOLOR</c>).
    /// </summary>
    public Color? OutlineColor { get; set; }

    /// <summary>
    /// Gets or sets the key box size in pixels (width × height) (<c>KEYSIZE</c>).
    /// </summary>
    public Size? KeySize { get; set; }

    /// <summary>
    /// Gets or sets the spacing between key boxes (Y) and labels (X), in pixels (<c>KEYSPACING</c>).
    /// </summary>
    public Size? KeySpacing { get; set; }

    /// <summary>
    /// Gets or sets the embedded position on the map (<c>POSITION</c>) when <see cref="Status"/> is <c>EMBED</c>.
    /// </summary>
    public CornerPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets whether to draw after the label cache (<c>POSTLABELCACHE</c>).
    /// Useful for neatline/overlay effects.
    /// </summary>
    public bool? PostLabelCache { get; set; }

    /// <summary>
    /// Gets or sets whether the legend background is transparent (<c>TRANSPARENT</c>).
    /// If not set, the OUTPUTFORMAT transparency settings apply.
    /// </summary>
    public bool? Transparent { get; set; }

    /// <summary>
    /// Gets or sets the nested LABEL block for legend text (<c>LABEL</c>).
    /// </summary>
    public Label? Label { get; set; }

    /// <summary>
    /// Gets or sets the HTML legend template path (<c>TEMPLATE</c>).
    /// </summary>
    public string? Template { get; set; }
}