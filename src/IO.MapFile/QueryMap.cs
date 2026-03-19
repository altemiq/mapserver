// -----------------------------------------------------------------------
// <copyright file="QueryMap.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

using System.Drawing;

/// <summary>
/// QUERYMAP block – how to draw a special map highlighting query results.
/// </summary>
public sealed class QueryMap
{
    public MapStatus Status { get; set; } = MapStatus.Off;   // on|off

    public Color? Color { get; set; } // highlight color (HILITE)

    public Size? Size { get; set; } // override WIDTH/HEIGHT

    public QueryMapStyle Style { get; set; } = QueryMapStyle.Hilite;
}