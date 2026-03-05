// -----------------------------------------------------------------------
// <copyright file="ScaleBar.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

using System.Drawing;

/// <summary>
/// SCALEBAR block – scalebar rendering and placement.
/// </summary>
public sealed class ScaleBar
{
    public MapStatus Status { get; set; } = MapStatus.Off;           // on|off|embed

    public Color? ImageColor { get; set; } // IMAGECOLOR (background)

    public Color? BackColor { get; set; } // BACKGROUNDCOLOR

    public Color? Color { get; set; } // COLOR (bar color)

    public Color? OutlineColor { get; set; } // OUTLINECOLOR (if supported)

    public Size? Size { get; set; } // SIZE (w x h, px)

    public CornerPosition? Position { get; set; } // POSITION (when embedded)

    public HorizontalAlign? Align { get; set; } // ALIGN within image

    public int? Intervals { get; set; } // INTERVALS (default 4)

    public Point? Offset { get; set; } // OFFSET (x,y) from corner

    public MapUnits? Units { get; set; } // UNITS (feet, meters, miles, etc.)

    public Label? Label { get; set; } // LABEL block

    public bool? Transparent { get; set; } // override transparency
}