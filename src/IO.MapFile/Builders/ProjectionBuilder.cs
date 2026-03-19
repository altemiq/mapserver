// -----------------------------------------------------------------------
// <copyright file="ProjectionBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Projection"/> builder.
/// </summary>
[Altemiq.Patterns.Builder.GenerateBuilderFor<Projection>]
public sealed partial class ProjectionBuilder
{
    /// <summary>
    /// Sets the PROJ string for the EPSG code.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <returns>The builder.</returns>
    public ProjectionBuilder WithEpsg(int code) => this.AddParameter(string.Create(System.Globalization.CultureInfo.InvariantCulture, $"init=epsg:{code}"));
}