// -----------------------------------------------------------------------
// <copyright file="WebBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Web"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Web), includeInternals: true)]
public sealed partial class WebBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="WebBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static WebBuilder New() => new();

    public WebBuilder WithMetadata(System.Action<System.Collections.Generic.IDictionary<string, string>> func) => this.WithMetadata(BuilderExtensions.BuildAndConfigureDictionary(func));

    public WebBuilder WithValidation(System.Action<System.Collections.Generic.IDictionary<string, string>> func) => this.WithValidation(BuilderExtensions.BuildAndConfigureDictionary(func));
}