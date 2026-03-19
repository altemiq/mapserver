// -----------------------------------------------------------------------
// <copyright file="Reference.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

using System.Drawing;

/// <summary>
/// REFERENCE block – miniature locator map and highlight box settings.
/// </summary>
public sealed class Reference
{
    public Color? Color { get; set; } // COLOR (fill)

    public Color? OutlineColor { get; set; } // OUTLINECOLOR

    public BoundingBox? Extent { get; set; } // EXTENT of base reference image

    public Size? Size { get; set; } // SIZE of base image (in px; both >=5)

    public string? Image { get; set; } // IMAGE (background GIF/PNG)

    public string? Marker { get; set; } // MARKER (symbol name or "0" default)

    public int? MarkerSize { get; set; } // MARKERSIZE

    public int? MinBoxSize { get; set; } // MINBOXSIZE

    public int? MaxBoxSize { get; set; } // MAXBOXSIZE

    public MapStatus Status { get; set; } = MapStatus.Off;
}