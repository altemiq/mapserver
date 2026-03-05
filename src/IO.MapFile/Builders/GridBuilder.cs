// -----------------------------------------------------------------------
// <copyright file="GridBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Grid"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Grid), includeInternals: true)]
public sealed partial class GridBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="GridBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static GridBuilder New() => new();
}