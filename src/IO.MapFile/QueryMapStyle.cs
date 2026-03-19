// -----------------------------------------------------------------------
// <copyright file="QueryMapStyle.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// QueryMap style: how to render selected vs non-selected features.
/// </summary>
public enum QueryMapStyle
{
    Normal,
    Hilite,
    Selected,
}