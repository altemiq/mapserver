// -----------------------------------------------------------------------
// <copyright file="Leader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;
/// <summary>
/// LEADER block – draws leader lines from labels to features when there is not enough room.
/// </summary>
public sealed class Leader
{
    public int? GridStep { get; set; } // pixels between candidate positions

    public int? MaxDistance { get; set; } // max distance in pixels

    public Style? Style { get; set; } // line style (color/width/etc.)
}