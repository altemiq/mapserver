// -----------------------------------------------------------------------
// <copyright file="IdentifyBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Identify"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Identify), includeInternals: true)]
public sealed partial class IdentifyBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="IdentifyBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static IdentifyBuilder New() => new();
}