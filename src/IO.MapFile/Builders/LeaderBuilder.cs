// -----------------------------------------------------------------------
// <copyright file="LeaderBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Leader"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Leader), includeInternals: true)]
public sealed partial class LeaderBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="LeaderBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static LeaderBuilder New() => new();
}