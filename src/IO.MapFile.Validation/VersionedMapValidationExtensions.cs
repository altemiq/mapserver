// -----------------------------------------------------------------------
// <copyright file="VersionedMapValidationExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Validation;

/// <summary>
/// Extensions for validating versioned map objects.
/// </summary>
public static class VersionedMapValidationExtensions
{
    /// <summary>
    /// Validates the map object.
    /// </summary>
    /// <param name="map">The map to validate.</param>
    /// <param name="major">The major version.</param>
    /// <param name="minor">The minor version.</param>
    /// <returns>The validation result.</returns>
    public static ValidationResult Validate(this Map map, int major, int minor) => map.Validate(new(new(major, minor)));

    /// <summary>
    /// Validates the map object.
    /// </summary>
    /// <param name="map">The map to validate.</param>
    /// <param name="options">The validation options.</param>
    /// <returns>The validation result.</returns>
    public static ValidationResult Validate(this Map map, VersionedValidationOptions options) => new VersionedMapValidator(options).Validate(map);

    /// <summary>
    /// Throws a <see cref="ValidationException"/> if the map is invalid.
    /// </summary>
    /// <param name="map">The map to validate.</param>
    /// <param name="major">The major version.</param>
    /// <param name="minor">The minor version.</param>
    /// <exception cref="ValidationException"><paramref name="map"/> is not valid.</exception>
    public static void ThrowIfInvalid(this Map map, int major, int minor) => map.ThrowIfInvalid(new(new(major, minor)));

    /// <summary>
    /// Throws a <see cref="ValidationException"/> if the map is invalid.
    /// </summary>
    /// <param name="map">The map to validate.</param>
    /// <param name="options">The validation options.</param>
    /// <exception cref="ValidationException"><paramref name="map"/> is not valid.</exception>
    public static void ThrowIfInvalid(this Map map, VersionedValidationOptions options)
    {
        if (map.Validate(options) is { IsValid: false, Errors: var errors })
        {
            throw new ValidationException(errors.Where(static e => e.Severity is Severity.Error));
        }
    }
}