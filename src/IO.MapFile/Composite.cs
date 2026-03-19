// -----------------------------------------------------------------------
// <copyright file="Composite.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// COMPOSITE settings for a LAYER: render into a temporary image and
/// merge onto the final map image using an operator, optional filters,
/// and optional opacity.
/// </summary>
public sealed class Composite
{
    /// <summary>
    /// Gets or sets the compositing operator (e.g., <c>src-over</c>, <c>multiply</c>, <c>screen</c>).
    /// </summary>
    public string? CompOp { get; set; }

    /// <summary>
    /// Gets the list of compositing filters to apply (e.g., <c>blur(4)</c>, <c>translate(2,2)</c>, <c>grayscale()</c>).
    /// </summary>
    public IList<string> CompFilters { get; } = [];

    /// <summary>
    /// Gets or sets the opacity (0..100) applied during compositing, if applicable.
    /// </summary>
    public int? Opacity { get; set; }
}