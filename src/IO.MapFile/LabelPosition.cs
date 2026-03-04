// -----------------------------------------------------------------------
// <copyright file="LabelPosition.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Label positioning relative to the feature, including AUTO.
/// (Consolidates common positions used in docs &amp; tutorial.)
/// </summary>
public enum LabelPosition
{
    /// <summary>
    /// Auto.
    /// </summary>
    Auto,

    /// <summary>
    /// center-center.
    /// </summary>
    CC,

    /// <summary>
    /// upper-center.
    /// </summary>
    UC,

    /// <summary>
    /// lower-center.
    /// </summary>
    LC,

    /// <summary>
    /// center-left.
    /// </summary>
    CL,

    /// <summary>
    /// center-right.
    /// </summary>
    CR,

    /// <summary>
    /// upper-left.
    /// </summary>
    UL,

    /// <summary>
    /// upper-right.
    /// </summary>
    UR,

    /// <summary>
    /// lower-left.
    /// </summary>
    LL,

    /// <summary>
    /// lower-right.
    /// </summary>
    LR,
}