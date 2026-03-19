// -----------------------------------------------------------------------
// <copyright file="Style.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Symbolization for CLASS/LABEL (<c>STYLE</c>): colors, widths, size, rotation,
/// patterns, offsets, and (pixel-space) <c>GEOMTRANSFORM</c>.
/// </summary>
public sealed class Style
{
    /// <summary>Gets or sets the primary draw color (<c>COLOR</c>).</summary>
    public ColorOrAttribute Color { get; set; }

    /// <summary>Gets or sets the outline color (<c>OUTLINECOLOR</c>).</summary>
    public ColorOrAttribute OutlineColor { get; set; }

    /// <summary>Gets or sets the symbol name from <c>SYMBOLSET</c> or an inline <c>SYMBOL</c>.</summary>
    public string? Symbol { get; set; }

    /// <summary>Gets or sets the stroke thickness (<c>WIDTH</c>).</summary>
    public double? Width { get; set; }

    /// <summary>Gets or sets the symbol size (<c>SIZE</c>).</summary>
    public double? Size { get; set; }

    /// <summary>Gets or sets the rotation (<c>ANGLE</c>).</summary>
    public Angle Angle { get; set; }

    /// <summary>Gets the dash pattern for lines.</summary>
    public IList<double> Pattern { get; } = [];

    /// <summary>Gets or sets the decoration spacing along lines (<c>GAP</c>).</summary>
    public double? Gap { get; set; }

    /// <summary>Gets or sets the pixel offset (<c>OFFSET</c>).</summary>
    public Point? Offset { get; set; }

    /// <summary>Gets or sets the opacity (0–100).</summary>
    public int? Opacity { get; set; }

    /// <summary>Gets or sets the STYLE-level geometry transform (<c>GEOMTRANSFORM</c>).</summary>
    public string? GeomTransform { get; set; }

    /// <summary>Gets or sets the minimum scale denominator at which this STYLE applies.</summary>
    public double? MinScaleDenom { get; set; }

    /// <summary>Gets or sets the maximum scale denominator at which this STYLE applies.</summary>
    public double? MaxScaleDenom { get; set; }

    // ------------------------------
    // Legacy shims to guide migration (present to surface compile-time guidance)
    // ------------------------------

    /// <summary>
    /// Gets or sets the legacy attribute binding for angles (<c>ANGLEITEM</c>). (Deprecated)
    /// Use <c>ANGLE [attribute]</c> instead.
    /// </summary>
    [Obsolete("Deprecated since MapServer 5.0. Use ANGLE [attribute] instead.")]
    public string? AngleItem { get; set; }

    /// <summary>
    /// Gets or sets the legacy GD-only text antialias flag (<c>ANTIALIAS</c>). (Obsolete)
    /// GD renderer was removed in MapServer 7.0.
    /// </summary>
    [Obsolete("Obsolete since MapServer 7.0 (GD renderer removed).")]
    public bool? Antialias { get; set; }

    /// <summary>
    /// Gets or sets the legacy background color (<c>BACKGROUNDCOLOR</c>). (Deprecated)
    /// Prefer multiple <c>STYLE</c> blocks to achieve the same effect.
    /// </summary>
    [Obsolete("Deprecated since MapServer 6.2. Use multiple STYLE blocks instead.")]
    public Color? BackgroundColor { get; set; }
}