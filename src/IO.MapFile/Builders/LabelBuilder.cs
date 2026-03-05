// -----------------------------------------------------------------------
// <copyright file="LabelBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Label"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Label), includeInternals: true)]
public sealed partial class LabelBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="LabelBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static LabelBuilder New() => new();

    /// <inheritdoc cref="WithOffset(Func{System.Drawing.Point?})" />
    public LabelBuilder WithOffset(int x, int y) => this.WithOffset(new System.Drawing.Point(x, y));

    /// <inheritdoc cref="WithShadowSize(Func{System.Drawing.Size?})" />
    public LabelBuilder WithShadowSize(int width, int height) => this.WithShadowSize(new System.Drawing.Size(width, height));
}