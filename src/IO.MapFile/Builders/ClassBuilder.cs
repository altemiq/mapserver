// -----------------------------------------------------------------------
// <copyright file="ClassBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Class"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Class), includeInternals: true)]
public sealed partial class ClassBuilder
{
    private Dictionary<string, string>? validations;

    /// <summary>
    /// Creates a new instance of <see cref="ClassBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static ClassBuilder New() => new();

    public ClassBuilder AddValidation(string key, string value)
    {
        this.validations ??= new Dictionary<string, string>(StringComparer.Ordinal);
        this.validations.Add(key, value);
        return this.WithValidation(() => this.validations);
    }

    /// <summary>
    /// Gets style(s) used to draw features of this class.
    /// </summary>
    public ClassBuilder WithStyles(System.Collections.Generic.IEnumerable<StyleBuilder> builders) => this.WithStyles(() => builders);

    /// <summary>
    /// Gets style(s) used to draw features of this class.
    /// </summary>
    public ClassBuilder WithStyles(Func<System.Collections.Generic.IEnumerable<StyleBuilder>> func) => this.WithStyles(() => func().BuildToList<StyleBuilder, Style>());
}