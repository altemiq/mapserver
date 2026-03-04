// -----------------------------------------------------------------------
// <copyright file="Grid.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// GRID block – graticule / grid lines with labeling.
/// </summary>
public sealed class Grid
{
    /// <summary>
    /// Gets or sets the label format.
    /// </summary>
    public GridLabelFormat LabelFormat { get; set; } = GridLabelFormat.DD;

    /// <summary>
    /// Gets or sets the format when <see cref="LabelFormat"/> is <see cref="GridLabelFormat.Custom"/>, a C-format string such as "%g°".
    /// </summary>
    public string? LabelFormatCustom { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of arcs to draw. Increase this parameter to get more lines. Optional. Must be greater than 0.
    /// </summary>
    public double? MinArcs { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of arcs to draw. Decrease this parameter to get fewer lines. Optional. Must be greater than 0.
    /// </summary>
    public double? MaxArcs { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of intervals to try to use.The distance between the grid lines, in the units of the grid’s coordinate system.Optional.Must be greater than 0.
    /// </summary>
    public double? MinInterval { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of intervals to try to use. The distance between the grid lines, in the units of the grid’s coordinate system. Optional. Must be greater than 0.
    /// </summary>
    public double? MaxInterval { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of segments to use when rendering an arc. If the lines should be very curved, use this to smooth the lines by adding more segments. Optional. Must be greater than 0.
    /// </summary>
    public double? MinSubdivide { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of segments to use when rendering an arc. If the graticule should be very straight, use this to minimize the number of points for faster rendering. Optional, default 256. Must be greater than 0.
    /// </summary>
    public double? MaxSubdivide { get; set; }
}