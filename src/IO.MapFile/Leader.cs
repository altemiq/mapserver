// -----------------------------------------------------------------------
// <copyright file="Leader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// LEADER block — draws a leader line from the label to the feature when
/// the preferred label position is unavailable due to collisions.
/// </summary>
/// <remarks>
/// Leader lines are typically used with dense point or polygon‑centroid
/// labeling to maintain label readability.
/// </remarks>
public sealed class Leader
{
    /// <summary>
    /// Gets or sets the pixel step between candidate leader positions (<c>GRIDSTEP</c>).
    /// Larger values reduce search granularity; smaller values increase precision.
    /// </summary>
    public int? GridStep { get; set; }

    /// <summary>
    /// Gets or sets the maximum leader line length in pixels (<c>MAXDISTANCE</c>).
    /// A leader is only drawn if the label can be placed within this distance.
    /// </summary>
    public int? MaxDistance { get; set; }

    /// <summary>
    /// Gets or sets the STYLE block used to draw the leader line (color, width, pattern).
    /// </summary>
    public Style? Style { get; set; }
}