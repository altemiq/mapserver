// -----------------------------------------------------------------------
// <copyright file="Legend.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

using System.Drawing;

/// <summary>
/// LEGEND block – auto or embedded legend image &amp; label formatting.
/// </summary>
public sealed class Legend
{
    /// <summary>
    /// Gets or sets the map status.
    /// </summary>
    public MapStatus Status { get; set; } = MapStatus.Off;

    /// <summary>
    /// Gets or sets the image color.
    /// </summary>
    public Color? ImageColor { get; set; }

    /// <summary>
    /// Gets or sets the outline color.
    /// </summary>
    public Color? OutlineColor { get; set; }

    /// <summary>Gets or sets key box size (default 20x10 px).</summary>
    public Size? KeySize { get; set; }

    /// <summary>Gets or sets spacing between key boxes (y) and labels (x), in pixels.</summary>
    public Size? KeySpacing { get; set; }

    /// <summary>Gets or sets where to embed the legend in the map (when Status=Embed).</summary>
    public CornerPosition? Position { get; set; }

    /// <summary>Gets or sets render after label cache (neatlines use-case).</summary>
    public bool? PostLabelCache { get; set; }

    /// <summary>Gets or sets override transparency; otherwise inherits OUTPUTFORMAT transparency.</summary>
    public bool? Transparent { get; set; }

    /// <summary>
    /// Gets or sets the nested LABEL for legend text.
    /// </summary>
    public Label? Label { get; set; }

    /// <summary>
    /// Gets or sets the HTML legend template.
    /// </summary>
    public string? Template { get; set; }
}