// -----------------------------------------------------------------------
// <copyright file="CornerPosition.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Positions for placing legend/scalebar around the map canvas.
/// </summary>
public enum CornerPosition
{
    /// <summary>
    /// Upper left.
    /// </summary>
    UL,

    /// <summary>
    /// Upper center.
    /// </summary>
    UC,

    /// <summary>
    /// Upper right.
    /// </summary>
    UR,

    /// <summary>
    /// Lower left.
    /// </summary>
    LL,

    /// <summary>
    /// Lower center.
    /// </summary>
    LC,

    /// <summary>
    /// Lower right.
    /// </summary>
    LR,
}