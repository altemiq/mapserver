// -----------------------------------------------------------------------
// <copyright file="GridLabelFormat.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// GRID label formats.
/// </summary>
public enum GridLabelFormat
{
    /// <summary>
    /// Degrees.
    /// </summary>
    DD,

    /// <summary>
    /// Degrees and minutes.
    /// </summary>
    DDMM,

    /// <summary>
    /// Degrees, minutes, seconds.
    /// </summary>
    DDMMSS,

    /// <summary>
    /// Use a C-style format string (see <see cref="Grid.LabelFormatCustom"/>).
    /// </summary>
    Custom,
}