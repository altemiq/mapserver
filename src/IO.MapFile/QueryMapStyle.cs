// -----------------------------------------------------------------------
// <copyright file="QueryMapStyle.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Styles used by QUERYMAP to render selected or highlighted query results.
/// </summary>
public enum QueryMapStyle
{
    /// <summary>
    /// Render the query map normally (<c>NORMAL</c>).
    /// </summary>
    Normal,

    /// <summary>
    /// Highlight the selected features using the configured highlight color (<c>HILITE</c>).
    /// </summary>
    Hilite,

    /// <summary>
    /// Render only the selected features (<c>SELECTED</c>).
    /// </summary>
    Selected,
}