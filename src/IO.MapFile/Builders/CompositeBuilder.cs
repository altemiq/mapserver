// -----------------------------------------------------------------------
// <copyright file="CompositeBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Composite"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Composite), includeInternals: true)]
public sealed partial class CompositeBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="CompositeBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static CompositeBuilder New() => new();
}