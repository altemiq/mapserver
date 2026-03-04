// -----------------------------------------------------------------------
// <copyright file="Style.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

using System.Drawing;

/// <summary>
/// STYLE – symbolization and styling parameters for geometry.
/// </summary>
public sealed class Style
{
    /// <summary>Gets or sets primary fill/line color; polygons use this as fill, lines as stroke.</summary>
    public ColorOrAttribute Color { get; set; }

    /// <summary>Gets or sets outline/stroke color for polygon fills or symbol outlines, if applicable.</summary>
    public ColorOrAttribute OutlineColor { get; set; }

    /// <summary>Gets or sets symbol name (from SYMBOLSET or inline SYMBOL).</summary>
    public string? Symbol { get; set; }

    /// <summary>Gets or sets pixel or unit-scaled width (stroke thickness).</summary>
    public double? Width { get; set; }

    /// <summary>Gets or sets pixel or unit-scaled size (point symbol height, hatch spacing, etc.).</summary>
    public double? Size { get; set; }

    /// <summary>
    /// Gets or sets angle may be numeric degrees, "AUTO", "AUTO2", "FOLLOW", or an attribute binding "[FIELD]".
    /// Provide as string to preserve full expressiveness.
    /// </summary>
    public Angle Angle { get; set; }

    /// <summary>Gets pattern for dashed lines, e.g., [10, 5, 4, 5]. Units follow SIZEUNITS/renderer.</summary>
    public IList<double> Pattern { get; } = [];

    /// <summary>Gets or sets gap for decorations along lines; semantics depend on symbolization.</summary>
    public double? Gap { get; set; }

    /// <summary>Gets or sets pixel offset (x,y) for symbolization.</summary>
    public Point? Offset { get; set; }

    /// <summary>Gets or sets 0..100 opacity (some renderers accept 0..100 or 0..255; use 0..100 here).</summary>
    public int? Opacity { get; set; }

    /// <summary>Gets or sets sTYLE-level GEOMTRANSFORM expression (pixel space at this level).</summary>
    public string? GeomTransform { get; set; }

    public double? MinScaleDenom { get; set; }

    public double? MaxScaleDenom { get; set; }
}