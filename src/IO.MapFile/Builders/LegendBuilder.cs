// -----------------------------------------------------------------------
// <copyright file="LegendBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Legend"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Legend), includeInternals: true)]
public sealed partial class LegendBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="LegendBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static LegendBuilder New() => new();

    /// <inheritdoc cref="WithKeySize(System.Drawing.Size?)" />
    public LegendBuilder WithKeySize(int width, int height) => this.WithKeySize(new System.Drawing.Size(width, height));

    /// <inheritdoc cref="WithKeySpacing(System.Drawing.Size?)" />
    public LegendBuilder WithKeySpacing(int width, int height) => this.WithKeySpacing(new System.Drawing.Size(width, height));
}