// -----------------------------------------------------------------------
// <copyright file="ReferenceBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Reference"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Reference), includeInternals: true)]
public sealed partial class ReferenceBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="ReferenceBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static ReferenceBuilder New() => new();
}