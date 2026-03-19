// -----------------------------------------------------------------------
// <copyright file="HorizontalAlign.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Horizontal alignment options for objects rendered inside their own image
/// (e.g., LEGEND, SCALEBAR), used when the container supports alignment.
/// </summary>
[System.Flags]
public enum HorizontalAlign
{
    /// <summary>
    /// No alignment specified (invalid for actual placement).
    /// </summary>
    None = 0,

    /// <summary>
    /// Align content to the left edge.
    /// </summary>
    Left = 1 << 16,

    /// <summary>
    /// Align content to the horizontal center.
    /// </summary>
    Center = 1 << 17,

    /// <summary>
    /// Align content to the right edge.
    /// </summary>
    Right = 1 << 18,
}