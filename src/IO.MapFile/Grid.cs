// -----------------------------------------------------------------------
// <copyright file="Grid.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// GRID block (graticule/grid lines with labeling).
/// </summary>
public sealed class Grid
{
    /// <summary>
    /// Gets or sets the label format for graticule annotations.
    /// </summary>
    [DefaultValue(GridLabelFormat.DD)]
    public GridLabelFormat LabelFormat { get; set; } = GridLabelFormat.DD;

    /// <summary>
    /// Gets or sets the format string used when <see cref="LabelFormat"/> is
    /// <see cref="GridLabelFormat.Custom"/> (e.g., <c>"%g°"</c>).
    /// </summary>
    public string? LabelFormatCustom { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of arcs to draw. Must be greater than 0.
    /// Increasing this typically produces more grid lines.
    /// </summary>
    public double? MinArcs { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of arcs to draw. Must be greater than 0.
    /// Decreasing this typically produces fewer grid lines.
    /// </summary>
    public double? MaxArcs { get; set; }

    /// <summary>
    /// Gets or sets the minimum interval (in grid coordinate units) between grid lines.
    /// Must be greater than 0.
    /// </summary>
    public double? MinInterval { get; set; }

    /// <summary>
    /// Gets or sets the maximum interval (in grid coordinate units) between grid lines.
    /// Must be greater than 0.
    /// </summary>
    public double? MaxInterval { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of segments to use when rendering an arc.
    /// Higher values yield smoother curves. Must be greater than 0.
    /// </summary>
    public double? MinSubdivide { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of segments to use when rendering an arc.
    /// Lower values yield straighter lines and may render faster. Must be greater than 0.
    /// </summary>
    public double? MaxSubdivide { get; set; }
}