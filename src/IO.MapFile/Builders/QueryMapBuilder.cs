// -----------------------------------------------------------------------
// <copyright file="QueryMapBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="QueryMap"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(QueryMap), includeInternals: true)]
public sealed partial class QueryMapBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="QueryMapBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static QueryMapBuilder New() => new();

    /// <inheritdoc cref="WithSize(System.Drawing.Size?)" />
    public QueryMapBuilder WithSize(int width, int height) => this.WithSize(new System.Drawing.Size(width, height));
}