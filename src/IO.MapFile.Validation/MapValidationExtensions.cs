// -----------------------------------------------------------------------
// <copyright file="MapValidationExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Validation;

/// <summary>
/// Extensions for validating map objects.
/// </summary>
public static class MapValidationExtensions
{
    /// <summary>
    /// Validates the map object.
    /// </summary>
    /// <param name="map">The map to validate.</param>
    /// <returns>The validation result.</returns>
    public static ValidationResult Validate(this Map map) => new MapValidator().Validate(map);

    /// <summary>
    /// Throws a <see cref="ValidationException"/> if the map is invalid.
    /// </summary>
    /// <param name="map">The map to validate.</param>
    /// <exception cref="ValidationException"><paramref name="map"/> is not valid.</exception>
    public static void ThrowIfInvalid(this Map map)
    {
        if (map.Validate() is { IsValid: false, Errors: var errors })
        {
            throw new ValidationException(errors.Where(static e => e.Severity is Severity.Error));
        }
    }
}