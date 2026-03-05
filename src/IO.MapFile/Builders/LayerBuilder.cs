// -----------------------------------------------------------------------
// <copyright file="LayerBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Layer"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Layer), includeInternals: true, includeObsolete: true)]
public sealed partial class LayerBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="LayerBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static LayerBuilder New() => new();

    /// <summary>Gets child classes (feature selection and style sets).</summary>
    public LayerBuilder WithClasses(System.Collections.Generic.IEnumerable<ClassBuilder> builders) => this.WithClasses(() => builders);

    /// <summary>Gets child classes (feature selection and style sets).</summary>
    public LayerBuilder WithClasses(Func<System.Collections.Generic.IEnumerable<ClassBuilder>> func) => this.WithClasses(() => func().BuildToList<ClassBuilder, Class>());

    public LayerBuilder WithMetadata(System.Action<System.Collections.Generic.IDictionary<string, string>> func) =>
        this.WithMetadata(BuilderExtensions.BuildAndConfigureDictionary(func));

    public LayerBuilder WithValidation(System.Action<System.Collections.Generic.IDictionary<string, string>> func) =>
        this.WithValidation(BuilderExtensions.BuildAndConfigureDictionary(func));

    /// <summary>Gets list of PROCESSING directives (free-form key=value strings).</summary>
    public LayerBuilder WithProcessing(System.Action<System.Collections.Generic.IDictionary<string, string>> func) =>
        this.WithProcessing(() => [.. BuilderExtensions.BuildAndConfigureDictionary(func).Select(kvp => $"{kvp.Key}={kvp.Value}")]);
}