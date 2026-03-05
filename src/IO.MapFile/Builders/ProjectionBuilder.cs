// -----------------------------------------------------------------------
// <copyright file="ProjectionBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Projection"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Projection), includeInternals: true)]
public sealed partial class ProjectionBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="ProjectionBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static ProjectionBuilder New() => new();

    /// <summary>
    /// Sets the PROJ string for the EPSG code.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <returns>The builder.</returns>
    public ProjectionBuilder WithEpsg(int code) => this.WithParameters(() => [string.Create(System.Globalization.CultureInfo.InvariantCulture, $"init=epsg:{code}")]);
}