// -----------------------------------------------------------------------
// <copyright file="MapBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System;
using System.Drawing;

/// <summary>
/// Fluent builder for <see cref="Map"/>.
/// </summary>
/// <remarks>
/// The MAP object is the root of a Mapfile. Use this builder to configure global defaults,
/// declare output formats, and aggregate LAYERs and other child objects.
/// </remarks>
public sealed class MapBuilder
{
    private readonly Map map = new() { Status = MapStatus.On };

    /// <summary>Create a new map builder.</summary>
    /// <returns>The builder for chaining.</returns>
    public static MapBuilder New() => new();

    /// <summary>Start from an existing <see cref="Map"/> instance.</summary>
    /// <param name="map">The map to create from.</param>
    /// <returns>The builder for chaining.</returns>
    public static MapBuilder From(Map map)
    {
        var b = New()
            .Name(map.Name ?? string.Empty)
            .Status(map.Status)
            .Size(map.Size.Width, map.Size.Height)
            .MaxSize(map.MaxSize)
            .Units(map.Units)
            .ImageType(map.ImageType)
            .ImageColor(map.ImageColor)
            .SymbolSet(map.SymbolSet)
            .FontSet(map.FontSet)
            .ShapePath(map.ShapePath)
            .DebugLevel(map.DebugLevel);

        foreach (var kv in map.Config)
        {
            b.Config(kv.Key, kv.Value);
        }

        foreach (var kv in map.Metadata)
        {
            b.Metadata(kv.Key, kv.Value);
        }

        if (map.Projection is not null)
        {
            b.AddProjection(_ => ProjectionBuilder.From(map.Projection).Build());
        }

        if (map.Web is not null)
        {
            b.UseWeb(_ => WebBuilder.From(map.Web).Build());
        }

        foreach (var of in map.OutputFormats)
        {
            b.AddOutputFormat(_ => OutputFormatBuilder.From(of).Build());
        }

        if (map.Reference is not null)
        {
            b.UseReference(_ => ReferenceBuilder.From(map.Reference).Build());
        }

        if (map.Legend is not null)
        {
            b.UseLegend(_ => LegendBuilder.From(map.Legend).Build());
        }

        if (map.ScaleBar is not null)
        {
            b.UseScaleBar(_ => ScaleBarBuilder.From(map.ScaleBar).Build());
        }

        if (map.QueryMap is not null)
        {
            b.UseQueryMap(qb => QueryMapBuilder.From(map.QueryMap).Build());
        }

        foreach (var l in map.Layers)
        {
            b.AddLayer(lb => LayerBuilder.From(l).Build());
        }

        if (map.Extent is { } extent)
        {
            b.Extent(bb => BoundingBoxBuilder.From(extent).Build());
        }

        return b;
    }

    /// <summary>
    /// Sets the <see cref="Map.Name"/> value.
    /// </summary>
    /// <param name="v">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder Name(string v)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(v);
        this.map.Name = v;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.Status"/> value.
    /// </summary>
    /// <param name="s">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder Status(MapStatus s)
    {
        this.map.Status = s;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.Size"/> value.
    /// </summary>
    /// <param name="width">The width to set.</param>
    /// <param name="height">The height to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder Size(int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(height, 0);
        this.map.Size = new Size(width, height);
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.MaxSize"/> value.
    /// </summary>
    /// <param name="px">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder MaxSize(int? px)
    {
        this.map.MaxSize = px;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.Extent"/> value.
    /// </summary>
    /// <param name="minX">The minimum x-value.</param>
    /// <param name="minY">The minimum y-value.</param>
    /// <param name="maxX">The maximum x-value.</param>
    /// <param name="maxY">The maximum y-value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder Extent(double minX, double minY, double maxX, double maxY)
    {
        this.map.Extent = BoundingBoxBuilder.New()
            .MinX(minX)
            .MinY(minY)
            .MaxX(maxX)
            .MaxY(maxY)
            .Build();
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.Extent"/> value.
    /// </summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder Extent(Func<BoundingBoxBuilder, BoundingBox> configure)
    {
        this.map.Extent = configure(BoundingBoxBuilder.New());
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.Units"/> value.
    /// </summary>
    /// <param name="u">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder Units(MapUnits? u)
    {
        this.map.Units = u;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.Resolution"/> value.
    /// </summary>
    /// <param name="v">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder Resolution(int v)
    {
        this.map.Resolution = v;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.DefResolution"/> value.
    /// </summary>
    /// <param name="v">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder DefResolution(int v)
    {
        this.map.DefResolution = v;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.ImageType"/> value.
    /// </summary>
    /// <param name="name">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder ImageType(string? name)
    {
        this.map.ImageType = name;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.ImageColor"/> value.
    /// </summary>
    /// <param name="c">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder ImageColor(Color? c)
    {
        if (c.HasValue)
        {
            this.map.ImageColor = c.Value;
        }

        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.SymbolSet"/> value.
    /// </summary>
    /// <param name="path">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder SymbolSet(string? path)
    {
        this.map.SymbolSet = path;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.FontSet"/> value.
    /// </summary>
    /// <param name="path">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder FontSet(string? path)
    {
        this.map.FontSet = path;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.ShapePath"/> value.
    /// </summary>
    /// <param name="path">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder ShapePath(string? path)
    {
        this.map.ShapePath = path;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Map.DebugLevel"/> value.
    /// </summary>
    /// <param name="level">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder DebugLevel(int? level)
    {
        this.map.DebugLevel = level;
        return this;
    }

    /// <summary>Add or replace a global CONFIG key/value.</summary>
    /// <param name="key">Tke key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder Config(string key, string value)
    {
        this.map.Config[key] = value;
        return this;
    }

    /// <summary>Add or replace a MAP METADATA key/value.</summary>
    /// <param name="key">Tke key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder Metadata(string key, string value)
    {
        this.map.Metadata[key] = value;
        return this;
    }

    /// <summary>Attach a PROJECTION to the MAP (output) level.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder AddProjection(Action<ProjectionBuilder> configure) =>
        this.AddProjection(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Attach a PROJECTION to the MAP (output) level.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder AddProjection(Func<ProjectionBuilder, Projection> configure)
    {
        this.map.Projection = configure(ProjectionBuilder.New());
        return this;
    }

    /// <summary>Attach a WEB block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder UseWeb(Action<WebBuilder> configure) =>
        this.UseWeb(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Attach a WEB block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder UseWeb(Func<WebBuilder, Web> configure)
    {
        this.map.Web = configure(WebBuilder.New());
        return this;
    }

    /// <summary>Add one OUTPUTFORMAT declaration.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder AddOutputFormat(Action<OutputFormatBuilder> configure) =>
        this.AddOutputFormat(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Add one OUTPUTFORMAT declaration.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder AddOutputFormat(Func<OutputFormatBuilder, OutputFormat> configure)
    {
        this.map.OutputFormats.Add(configure(OutputFormatBuilder.New()));
        return this;
    }

    /// <summary>Attach a REFERENCE block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder UseReference(Action<ReferenceBuilder> configure) =>
        this.UseReference(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Attach a REFERENCE block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder UseReference(Func<ReferenceBuilder, Reference> configure)
    {
        this.map.Reference = configure(ReferenceBuilder.New());
        return this;
    }

    /// <summary>Attach a LEGEND block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder UseLegend(Action<LegendBuilder> configure) =>
        this.UseLegend(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Attach a LEGEND block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder UseLegend(Func<LegendBuilder, Legend> configure)
    {
        this.map.Legend = configure(LegendBuilder.New());
        return this;
    }

    /// <summary>Attach a SCALEBAR block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder UseScaleBar(Action<ScaleBarBuilder> configure) =>
        this.UseScaleBar(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Attach a SCALEBAR block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder UseScaleBar(Func<ScaleBarBuilder, ScaleBar> configure)
    {
        this.map.ScaleBar = configure(ScaleBarBuilder.New());
        return this;
    }

    /// <summary>Attach a QUERYMAP block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder UseQueryMap(Action<QueryMapBuilder> configure) =>
        this.UseQueryMap(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Attach a QUERYMAP block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder UseQueryMap(Func<QueryMapBuilder, QueryMap> configure)
    {
        this.map.QueryMap = configure(QueryMapBuilder.New());
        return this;
    }

    /// <summary>Add one LAYER.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder AddLayer(Action<LayerBuilder> configure) =>
        this.AddLayer(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Add one LAYER.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public MapBuilder AddLayer(Func<LayerBuilder, Layer> configure)
    {
        this.map.Layers.Add(configure(LayerBuilder.New()));
        return this;
    }

    /// <summary>Build the <see cref="Map"/>.</summary>
    /// <returns>The builder for chaining.</returns>
    public Map Build() => this.map;
}