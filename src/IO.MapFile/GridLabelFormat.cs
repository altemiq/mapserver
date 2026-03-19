// -----------------------------------------------------------------------
// <copyright file="GridLabelFormat.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Supported label formats for the <see cref="Grid"/> graticule labels.
/// </summary>
public enum GridLabelFormat
{
    /// <summary>
    /// Degrees (DD).
    /// </summary>
    DD,

    /// <summary>
    /// Degrees and minutes (DDMM).
    /// </summary>
    DDMM,

    /// <summary>
    /// Degrees, minutes, seconds (DDMMSS).
    /// </summary>
    DDMMSS,

    /// <summary>
    /// Custom format using a C‑style printf pattern (see <see cref="Grid.LabelFormatCustom"/>).
    /// </summary>
    Custom,
}