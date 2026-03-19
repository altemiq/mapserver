// -----------------------------------------------------------------------
// <copyright file="VerticalAlignment.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Vertical alignment options for objects rendered inside their own image
/// (e.g., LEGEND, SCALEBAR), used when the container supports alignment.
/// </summary>
[System.Flags]
public enum VerticalAlignment
{
    /// <summary>
    /// No alignment specified (invalid for actual placement).
    /// </summary>
    None = 0,

    /// <summary>
    /// Align content to the bottom edge.
    /// </summary>
    Lower = 1 << 0,

    /// <summary>
    /// Align content to the vertical center.
    /// </summary>
    Center = 1 << 1,

    /// <summary>
    /// Align content to the top edge.
    /// </summary>
    Upper = 1 << 2,
}