// -----------------------------------------------------------------------
// <copyright file="Label.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Text labeling options for features (<c>LABEL</c>), including font, color, placement,
/// multi‑line alignment and spacing.
/// </summary>
/// <remarks>
/// <para>
/// This documentation is locked to <strong>MapServer 8.0</strong>. Modern usage supports:
/// attribute‑bound values (e.g., for <c>ANGLE</c>), multi‑line alignment via <c>ALIGN</c>
/// when using <c>WRAP</c>, and curved labeling with <c>ANGLE FOLLOW</c> for linear features.
/// </para>
/// <para>
/// Several legacy billboard/shadow options were removed earlier (e.g., background shadow fields in 6.0)
/// and GD‑specific antialias options are obsolete due to GD’s removal in 7.0; use <c>LABEL STYLE</c>
/// with <c>GEOMTRANSFORM labelpoly</c> for background effects.
/// </para>
/// </remarks>
public sealed class Label
{
    /// <summary>Gets or sets the text color (<c>COLOR</c>).</summary>
    public Color? Color { get; set; }

    /// <summary>Gets or sets the outline/halo color (<c>OUTLINECOLOR</c>), often achieved via a LABEL <c>STYLE</c>.</summary>
    public Color? OutlineColor { get; set; }

    /// <summary>Gets or sets the font alias from <c>FONTSET</c> (<c>FONT</c>).</summary>
    public string? Font { get; set; }

    /// <summary>Gets or sets the label rendering type (<c>TYPE</c>, e.g., <c>truetype</c> or <c>bitmap</c>).</summary>
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the font size (pixels for TrueType; or a bitmap size keyword).
    /// </summary>
    public string? Size { get; set; }

    /// <summary>
    /// Gets or sets the label angle: degrees, <c>AUTO</c>/<c>AUTO2</c>, <c>FOLLOW</c>, or attribute‑bound (<c>[FIELD]</c>).
    /// </summary>
    /// <remarks>Default angle is 0°.</remarks>
    public string? Angle { get; set; }

    /// <summary>
    /// Gets or sets the multi‑line alignment for labels when <see cref="Wrap"/> is set: <c>left</c>, <c>center</c>, <c>right</c>.
    /// </summary>
    public HorizontalAlign? Align { get; set; }

    /// <summary>
    /// Gets or sets the preferred placement (<c>POSITION</c>). <c>AUTO</c> allows MapServer to choose a candidate.
    /// </summary>
    public LabelPosition Position { get; set; } = LabelPosition.Auto;

    /// <summary>
    /// Gets or sets the allow partial labels at the map edge (<c>PARTIALS</c>).
    /// </summary>
    public bool? Partials { get; set; }

    /// <summary>
    /// Gets or sets the force label placement even when overlaps occur (<c>FORCE</c>).
    /// </summary>
    public bool? Force { get; set; }

    /// <summary>
    /// Gets or sets the padding around labels in pixels (<c>BUFFER</c>).
    /// </summary>
    public int? Buffer { get; set; }

    /// <summary>Gets or sets the minimum feature size (pixels) to label (<c>MINFEATURESIZE</c>).</summary>
    public int? MinFeatureSize { get; set; }

    /// <summary>Gets or sets the mMinimum distance between placed labels, in pixels (<c>MINDISTANCE</c>).</summary>
    public int? MinDistance { get; set; }

    /// <summary>Gets or sets the wrap character to create multi‑line labels (<c>WRAP</c>).</summary>
    public string? Wrap { get; set; }

    /// <summary>Gets or sets the pixel offset for point placement (<c>OFFSET</c> X,Y).</summary>
    public Point? Offset { get; set; }

    /// <summary>
    /// Gets or sets the shadow size in pixels (<c>SHADOWSIZE</c>).
    /// </summary>
    /// <remarks>
    /// For background billboards and shadows, prefer a LABEL <c>STYLE</c> with
    /// <c>GEOMTRANSFORM labelpoly</c> and <c>OFFSET</c>. Background‑shadow fields present in much older
    /// versions were removed by 6.0.
    /// </remarks>
    public System.Drawing.Size? ShadowSize { get; set; }

    /// <summary>
    /// Gets the optional LABEL‑scoped <c>STYLE</c> entries (e.g., <c>GEOMTRANSFORM labelpoly</c> to emulate backgrounds).
    /// </summary>
    public IList<Style> Styles { get; } = [];
}