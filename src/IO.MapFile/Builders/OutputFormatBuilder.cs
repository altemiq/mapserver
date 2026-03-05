// -----------------------------------------------------------------------
// <copyright file="OutputFormatBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="OutputFormat"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(OutputFormat), includeInternals: true)]
public sealed partial class OutputFormatBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="OutputFormatBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static OutputFormatBuilder New() => new();
}