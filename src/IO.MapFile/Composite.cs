// -----------------------------------------------------------------------
// <copyright file="Composite.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// COMPOSITE (layer-level temporary rendering &amp; blending into final image).
/// </summary>
public sealed class Composite
{
    /// <summary>Gets or sets name of the compositing operator (e.g., "src-over", "multiply", "screen", etc.).</summary>
    public string? CompOp { get; set; }

    /// <summary>Gets one or more compositing filters (e.g., "blur(4)", "translate(2,2)", "grayscale()").</summary>
    public IList<string> CompFilters { get; } = [];

    /// <summary>Gets or sets opacity (0..100), if applicable to the operator.</summary>
    public int? Opacity { get; set; }
}