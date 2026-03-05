// -----------------------------------------------------------------------
// <copyright file="JoinBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Join"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Join), includeInternals: true)]
public sealed partial class JoinBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="JoinBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static JoinBuilder New() => new();
}