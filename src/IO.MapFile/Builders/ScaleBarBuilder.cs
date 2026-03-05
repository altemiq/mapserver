// -----------------------------------------------------------------------
// <copyright file="ScaleBarBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="ScaleBar"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(ScaleBar), includeInternals: true)]
public sealed partial class ScaleBarBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="ScaleBarBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static ScaleBarBuilder New() => new();

    /// <inheritdoc cref="WithSize(System.Drawing.Size?)" />
    public ScaleBarBuilder WithSize(int width, int height) => this.WithSize(new System.Drawing.Size(width, height));
}