// -----------------------------------------------------------------------
// <copyright file="Label.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

using System.Drawing;

/// <summary>
/// LABEL – labeling options for features (truetype/bitmap, placement, buffers, etc.).
/// </summary>
public sealed class Label
{
    /// <summary>
    /// Gets or sets text color.
    /// </summary>
    public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets halo/outline color (via labelpoly STYLE in modern MapServer).
    /// </summary>
    public Color? OutlineColor { get; set; }

    /// <summary>
    /// Gets or sets fONT alias from FONTSET.
    /// </summary>
    public string? Font { get; set; }

    /// <summary>
    /// Gets or sets "truetype" or "bitmap" (modern use focuses on truetype).
    /// </summary>
    public string? Type { get; set; }

    /// <summary>Gets or sets size in pixels for TrueType fonts (or keyword for bitmap fonts); use numeric here or standard names such as MEDIUM or LARGE.</summary>
    public string? Size { get; set; }

    /// <summary>Gets or sets aNGLE: numeric degrees, "AUTO", "AUTO2", "FOLLOW", or "[FIELD]".</summary>
    public string? Angle { get; set; }

    /// <summary>Gets or sets alignment for multi-line labels when WRAP is used.</summary>
    public HorizontalAlign? Align { get; set; }

    /// <summary>Gets or sets text placement; "Auto" lets MapServer choose best candidate.</summary>
    public LabelPosition Position { get; set; } = LabelPosition.Auto;

    /// <summary>
    /// Gets or sets allow partial labels.
    /// </summary>
    public bool? Partials { get; set; }

    /// <summary>
    /// Gets or sets force label even if overlaps.
    /// </summary>
    public bool? Force { get; set; }

    /// <summary>
    /// Gets or sets padding around labels (px).
    /// </summary>
    public int? Buffer { get; set; }

    /// <summary>
    /// Gets or sets min feature size to label (px).
    /// </summary>
    public int? MinFeatureSize { get; set; }

    /// <summary>
    /// Gets or sets min distance between labels (px).
    /// </summary>
    public int? MinDistance { get; set; }

    /// <summary>
    /// Gets or sets wrap character for multi-line labels.
    /// </summary>
    public string? Wrap { get; set; }

    /// <summary>
    /// Gets or sets the offset.
    /// </summary>
    public System.Drawing.Point? Offset { get; set; }

    /// <summary>
    /// Gets or sets the shadow size.
    /// </summary>
    public System.Drawing.Size? ShadowSize { get; set; }

    /// <summary>
    /// Gets optional additional STYLEs for label-aware geometry (e.g., GEOMTRANSFORM labelpoly).
    /// </summary>
    public IList<Style> Styles { get; } = [];
}