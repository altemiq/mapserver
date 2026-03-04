// -----------------------------------------------------------------------
// <copyright file="VerticalAlignment.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Legend alignment inside its own image.
/// </summary>
[Flags]
public enum VerticalAlignment
{
    /// <summary>
    /// None. Invalid.
    /// </summary>
    None = 0,

    /// <summary>
    /// Lower alignment.
    /// </summary>
    Lower = 1 << 0,

    /// <summary>
    /// Center alignment.
    /// </summary>
    Center = 1 << 1,

    /// <summary>
    /// Upper alignment.
    /// </summary>
    Upper = 1 << 2,
}