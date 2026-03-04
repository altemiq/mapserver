// -----------------------------------------------------------------------
// <copyright file="HorizontalAlign.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Legend alignment inside its own image.
/// </summary>
[Flags]
public enum HorizontalAlign
{
    /// <summary>
    /// None. Invalid.
    /// </summary>
    None = 0,

    /// <summary>
    /// Left alignment.
    /// </summary>
    Left = 1 << 16,

    /// <summary>
    /// Horizontal alignment.
    /// </summary>
    Center = 1 << 17,

    /// <summary>
    /// Right alignment.
    /// </summary>
    Right = 1 << 18,
}