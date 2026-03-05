// -----------------------------------------------------------------------
// <copyright file="MapBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="Map"/> <see cref="BuilderGenerator.Builder{T}"/>.
/// </summary>
[BuilderGenerator.BuilderFor(typeof(Map), includeInternals: true)]
public sealed partial class MapBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="MapBuilder"/>.
    /// </summary>
    /// <returns>The builder.</returns>
    public static MapBuilder New() => new();

    /// <summary>Gets or sets map image size in pixels (WIDTH, HEIGHT).</summary>
    public MapBuilder WithSize(int width, int height) => this.WithSize(new System.Drawing.Size(width, height));

    /// <summary>Gets or sets initial display extent (minx, miny, maxx, maxy).</summary>
    public MapBuilder WithExtent(double minX, double minY, double maxX, double maxY) => this.WithExtent(new BoundingBox(minX, minY, maxX, maxY));

    /// <summary>Gets child layers, evaluated top-to-bottom.</summary>
    public MapBuilder WithLayers(System.Collections.Generic.IEnumerable<LayerBuilder> layers) => this.WithLayers(() => layers);

    /// <summary>Gets child layers, evaluated top-to-bottom.</summary>
    public MapBuilder WithLayers(Func<System.Collections.Generic.IEnumerable<LayerBuilder>> func) => this.WithLayers(() => func().BuildToList<LayerBuilder, Layer>());

    /// <summary>Gets or sets wEB block (CGI paths, formats, templates, etc.).</summary>
    public MapBuilder WithWeb(WebBuilder builder) => WithWeb(() => builder);

    /// <summary>Gets or sets wEB block (CGI paths, formats, templates, etc.).</summary>
    public MapBuilder WithWeb(System.Func<WebBuilder> func) => this.WithWeb(() => func().Build());

    public MapBuilder WithConfig(System.Action<System.Collections.Generic.IDictionary<string, string>> func) => this.WithConfig(BuilderExtensions.BuildAndConfigureDictionary(func));
}