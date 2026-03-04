// -----------------------------------------------------------------------
// <copyright file="CornerPosition.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// LEGEND/SCALEBAR position around the map canvas (ul, uc, ur, ll, lc, lr).
/// </summary>
public enum CornerPosition
{
    /// <summary>
    /// Upper-left.
    /// </summary>
    UL,

    /// <summary>
    /// Upper-center.
    /// </summary>
    UC,

    /// <summary>
    /// Upper-right.
    /// </summary>
    UR,

    /// <summary>
    /// lower-left.
    /// </summary>
    LL,

    /// <summary>
    /// lower-center.
    /// </summary>
    LC,

    /// <summary>
    /// lower-right.
    /// </summary>
    LR,
}