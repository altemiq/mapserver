// -----------------------------------------------------------------------
// <copyright file="ScaleBar.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// SCALEBAR block — scalebar rendering, layout, and placement.
/// </summary>
public sealed class ScaleBar
{
    /// <summary>
    /// Gets or sets whether the scalebar is rendered (<c>STATUS</c>).
    /// </summary>
    [DefaultValue(MapStatus.Off)]
    public MapStatus Status { get; set; } = MapStatus.Off;

    /// <summary>
    /// Gets or sets the background image color for the scalebar (<c>IMAGECOLOR</c>).
    /// </summary>
    public Color? ImageColor { get; set; }

    /// <summary>
    /// Gets or sets the background color (<c>BACKGROUNDCOLOR</c>).
    /// </summary>
    public Color? BackColor { get; set; }

    /// <summary>
    /// Gets or sets the bar color (<c>COLOR</c>).
    /// </summary>
    public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets the outline color for the bar segments (<c>OUTLINECOLOR</c>).
    /// </summary>
    public Color? OutlineColor { get; set; }

    /// <summary>
    /// Gets or sets the scalebar size (width × height, pixels) (<c>SIZE</c>).
    /// </summary>
    public Size? Size { get; set; }

    /// <summary>
    /// Gets or sets the embedded position on the map (<c>POSITION</c>) when <see cref="Status"/> is <c>EMBED</c>.
    /// </summary>
    public CornerPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the horizontal alignment within the image (<c>ALIGN</c>).
    /// </summary>
    public HorizontalAlign? Align { get; set; }

    /// <summary>
    /// Gets or sets the number of intervals/segments to display (<c>INTERVALS</c>).
    /// </summary>
    [DefaultValue(4)]
    public int? Intervals { get; set; } = 4;

    /// <summary>
    /// Gets or sets the pixel offset from the corner when embedded (<c>OFFSET</c> X,Y).
    /// </summary>
    public Point? Offset { get; set; }

    /// <summary>
    /// Gets or sets the distance units for labeling (<c>UNITS</c>), e.g., <c>meters</c>, <c>miles</c>.
    /// </summary>
    public MapUnits? Units { get; set; }

    /// <summary>
    /// Gets or sets an optional nested LABEL block for scalebar text.
    /// </summary>
    public Label? Label { get; set; }

    /// <summary>
    /// Gets or sets whether the scalebar background is transparent (<c>TRANSPARENT</c>).
    /// If not set, the OUTPUTFORMAT transparency settings apply.
    /// </summary>
    public bool? Transparent { get; set; }
}