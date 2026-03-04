// -----------------------------------------------------------------------
// <copyright file="VersionedValidationOptions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Validation;

/// <summary>
/// The versioned validation options.
/// </summary>
public sealed class VersionedValidationOptions(MapServerVersion target)
{
    /// <summary>
    /// Gets the target mapserver version for validation.
    /// </summary>
    public MapServerVersion Target { get; } = target;
}