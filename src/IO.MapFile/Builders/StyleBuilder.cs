// -----------------------------------------------------------------------
// <copyright file="StyleBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Style"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Style), includeInternals: true)]
public sealed partial class StyleBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="StyleBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static StyleBuilder New() => new();

    /// <summary>Gets or sets primary fill/line color; polygons use this as fill, lines as stroke.</summary>
    public StyleBuilder WithColor(byte r, byte g, byte b) => this.WithColor(System.Drawing.Color.FromArgb(r, g, b));

    /// <summary>Gets or sets outline/stroke color for polygon fills or symbol outlines, if applicable.</summary>
    public StyleBuilder WithOutlineColor(byte r, byte g, byte b) => this.WithOutlineColor(System.Drawing.Color.FromArgb(r, g, b));
}