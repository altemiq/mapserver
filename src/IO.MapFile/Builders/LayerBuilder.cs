// -----------------------------------------------------------------------
// <copyright file="LayerBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System;

/// <summary>
/// Fluent builder for <see cref="Layer"/>.
/// </summary>
/// <remarks>
/// A LAYER combines a data source (DATA/CONNECTION/CONNECTIONTYPE) with styling via CLASS blocks.
/// The first matching CLASS per feature is used unless controlled by layer processing options.
/// </remarks>
public sealed class LayerBuilder
{
    private readonly Layer layer = new() { Status = MapStatus.Off }; // default aligns with common Mapfile convention

    public static LayerBuilder New() => new();

    public static LayerBuilder From(Layer l)
    {
        var b = New()
            .Name(l.Name)
            .Type(l.Type)
            .Status(l.Status)
            .Group(l.Group)
            .ClassGroup(l.ClassGroup)
            .Data(l.Data)
            .Connection(l.Connection)
            .ConnectionType(l.ConnectionType)
            .ClassItem(l.ClassItem)
            .Filter(l.Filter)
            .FilterItem(l.FilterItem)
            .LabelItem(l.LabelItem)
            .LabelRequires(l.LabelRequires)
            .LabelMinScaleDenom(l.LabelMinScaleDenom)
            .LabelMaxScaleDenom(l.LabelMaxScaleDenom)
            .MinScaleDenom(l.MinScaleDenom)
            .MaxScaleDenom(l.MaxScaleDenom)
            .MinGeoWidth(l.MinGeoWidth)
            .MaxGeoWidth(l.MaxGeoWidth)
            .MinFeatureSize(l.MinFeatureSize)
            .MaxFeatures(l.MaxFeatures)
            .Header(l.Header)
            .Footer(l.Footer)
            .Template(l.Template)
            .Requires(l.Requires)
            .Mask(l.Mask)
            .Units(l.Units)
            .GeomTransform(l.GeomTransform);

        if (l.Extent is { } extent)
        {
            b.Extent(lb => BoundingBoxBuilder.From(extent).Build());
        }

        foreach (var p in l.Processing)
        {
            b.Processing(p);
        }

        foreach (var kv in l.Metadata)
        {
            b.Metadata(kv.Key, kv.Value);
        }

        foreach (var kv in l.ConnectionOptions)
        {
            b.ConnectionOption(kv.Key, kv.Value);
        }

        foreach (var kv in l.BindVals)
        {
            b.BindVal(kv.Key, kv.Value);
        }

        foreach (var kv in l.Validation)
        {
            b.Validation(kv.Key, kv.Value);
        }

        if (l.Projection is not null)
        {
            b.AddProjection(pb => ProjectionBuilder.From(l.Projection!).Build());
        }

        if (l.Grid is not null)
        {
            b.Grid(gb => GridBuilder.From(l.Grid!).Build());
        }

        if (l.Identify is not null)
        {
            b.Identify(ib => IdentifyBuilder.From(l.Identify!).Build());
        }

        if (l.Leader is not null)
        {
            b.Leader(leb => LeaderBuilder.From(l.Leader!).Build());
        }

        foreach (var j in l.Joins)
        {
            b.AddJoin(jb => JoinBuilder.From(j).Build());
        }

        if (l.Composite is not null)
        {
            b.Composite(cb => CompositeBuilder.From(l.Composite!).Build());
        }

        foreach (var c in l.Classes)
        {
            b.AddClass(cb => ClassBuilder.From(c).Build());
        }

        return b;
    }

    public LayerBuilder Name(string v)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(v);
        this.layer.Name = v;
        return this;
    }

    public LayerBuilder Type(LayerType t)
    {
        this.layer.Type = t;
        return this;
    }

    public LayerBuilder Status(MapStatus s)
    {
        this.layer.Status = s;
        return this;
    }

    public LayerBuilder Group(string? v)
    {
        this.layer.Group = v;
        return this;
    }

    public LayerBuilder ClassGroup(string? v)
    {
        this.layer.ClassGroup = v;
        return this;
    }

    public LayerBuilder Data(string? v)
    {
        this.layer.Data = v;
        return this;
    }

    public LayerBuilder Connection(string? v)
    {
        this.layer.Connection = v;
        return this;
    }

    public LayerBuilder ConnectionType(ConnectionType t)
    {
        this.layer.ConnectionType = t;
        return this;
    }

    public LayerBuilder ConnectionOption(string key, string value)
    {
        this.layer.ConnectionOptions[key] = value;
        return this;
    }

    public LayerBuilder BindVal(string key, string value)
    {
        this.layer.BindVals[key] = value;
        return this;
    }

    public LayerBuilder ClassItem(string? v)
    {
        this.layer.ClassItem = v;
        return this;
    }

    public LayerBuilder Filter(string? expr)
    {
        this.layer.Filter = expr;
        return this;
    }

    public LayerBuilder FilterItem(string? v)
    {
        this.layer.FilterItem = v;
        return this;
    }

    public LayerBuilder LabelItem(string? v)
    {
        this.layer.LabelItem = v;
        return this;
    }

    public LayerBuilder LabelRequires(string? expr)
    {
        this.layer.LabelRequires = expr;
        return this;
    }

    public LayerBuilder LabelMinScaleDenom(double? v)
    {
        this.layer.LabelMinScaleDenom = v;
        return this;
    }

    public LayerBuilder LabelMaxScaleDenom(double? v)
    {
        this.layer.LabelMaxScaleDenom = v;
        return this;
    }

    public LayerBuilder MinScaleDenom(double? v)
    {
        this.layer.MinScaleDenom = v;
        return this;
    }

    public LayerBuilder MaxScaleDenom(double? v)
    {
        this.layer.MaxScaleDenom = v;
        return this;
    }

    public LayerBuilder MinGeoWidth(double? v)
    {
        this.layer.MinGeoWidth = v;
        return this;
    }

    public LayerBuilder MaxGeoWidth(double? v)
    {
        this.layer.MaxGeoWidth = v;
        return this;
    }

    public LayerBuilder MinFeatureSize(int? px)
    {
        this.layer.MinFeatureSize = px;
        return this;
    }

    public LayerBuilder MaxFeatures(int? n)
    {
        this.layer.MaxFeatures = n;
        return this;
    }

    public LayerBuilder Header(string? v)
    {
        this.layer.Header = v;
        return this;
    }

    public LayerBuilder Footer(string? v)
    {
        this.layer.Footer = v;
        return this;
    }

    public LayerBuilder Template(string? v)
    {
        this.layer.Template = v;
        return this;
    }

    public LayerBuilder Requires(string? expr)
    {
        this.layer.Requires = expr;
        return this;
    }

    public LayerBuilder Mask(string? name)
    {
        this.layer.Mask = name;
        return this;
    }

    public LayerBuilder Units(MapUnits? u)
    {
        this.layer.Units = u;
        return this;
    }

    public LayerBuilder SizeUnits(MapUnits? u)
    {
        this.layer.SizeUnits = u;
        return this;
    }

    public LayerBuilder GeomTransform(string? expr)
    {
        this.layer.GeomTransform = expr;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Layer.Tolerance"/> value.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    [Obsolete("Moved in MapServer 8.6; use LAYER IDENTITY for tolerance.")]
    public LayerBuilder Tolerance(double value)
    {
        this.layer.Tolerance = value;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Layer.ToleranceUnits"/> value.
    /// </summary>
    /// <param name="unit">The value to set.</param>
    /// <returns>The builder for chaining.</returns>

    [Obsolete("Moved in MapServer 8.6; use LAYER IDENTITY for tolerance.")]
    public LayerBuilder ToleranceUnits(MapUnits unit)
    {
        this.layer.ToleranceUnits = unit;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Layer.Extent"/> value.
    /// </summary>
    /// <param name="minX">The minimum x-value.</param>
    /// <param name="minY">The minimum y-value.</param>
    /// <param name="maxX">The maximum x-value.</param>
    /// <param name="maxY">The maximum y-value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Extent(double minX, double minY, double maxX, double maxY)
    {
        this.layer.Extent = BoundingBoxBuilder.New()
            .MinX(minX)
            .MinY(minY)
            .MaxX(maxX)
            .MaxX(maxY)
            .Build();
        return this;
    }

    /// <summary>
    /// Sets the <see cref="Layer.Extent"/> value.
    /// </summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Extent(Func<BoundingBoxBuilder, BoundingBox> configure)
    {
        this.layer.Extent = configure(BoundingBoxBuilder.New());
        return this;
    }

    /// <summary>Add one PROCESSING directive (free-form <c>KEY=VALUE</c>).</summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Processing(string key, string value)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(key);
        ArgumentNullException.ThrowIfNullOrEmpty(value);
        this.layer.Processing.Add($"{key}={value}");
        return this;
    }

    /// <summary>Add one PROCESSING directive (free-form <c>KEY=VALUE</c>).</summary>
    /// <param name="keyValuePair">The key/value pair.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Processing(string keyValuePair)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(keyValuePair);
        this.layer.Processing.Add(keyValuePair);
        return this;
    }

    /// <summary>Add or replace a LAYER METADATA key/value.</summary>
    /// <param name="key">Tke key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Metadata(string key, string value)
    {
        this.layer.Metadata[key] = value;
        return this;
    }

    /// <summary>Add or replace a LAYER VALIDATION key/value.</summary>
    /// <param name="key">Tke key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Validation(string key, string value)
    {
        this.layer.Validation[key] = value;
        return this;
    }

    /// <summary>Attach a PROJECTION block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder AddProjection(Action<ProjectionBuilder> configure) =>
        this.AddProjection(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Attach a PROJECTION block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder AddProjection(Func<ProjectionBuilder, Projection> configure)
    {
        this.layer.Projection = configure(ProjectionBuilder.New());
        return this;
    }

    /// <summary>Attach a GRID block (TYPE LINE layers when drawing graticules).</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Grid(Action<GridBuilder> configure) =>
        this.Grid(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Attach a GRID block (TYPE LINE layers when drawing graticules).</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Grid(Func<GridBuilder, Grid> configure)
    {
        this.layer.Grid = configure(GridBuilder.New());
        return this;
    }

    /// <summary>Attach an IDENTIFY block (8.6+).</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Identify(Action<IdentifyBuilder> configure) =>
        this.Identify(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Attach an IDENTIFY block (8.6+).</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Identify(Func<IdentifyBuilder, Identify> configure)
    {
        this.layer.Identify = configure(IdentifyBuilder.New());
        return this;
    }

    /// <summary>Attach a LEADER block to control leader lines for labels.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Leader(Action<LeaderBuilder> configure) =>
        this.Leader(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Attach a LEADER block to control leader lines for labels.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Leader(Func<LeaderBuilder, Leader> configure)
    {
        this.layer.Leader = configure(LeaderBuilder.New());
        return this;
    }

    /// <summary>Add one JOIN block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder AddJoin(Action<JoinBuilder> configure) =>
        this.AddJoin(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Add one JOIN block.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder AddJoin(Func<JoinBuilder, Join> configure)
    {
        this.layer.Joins.Add(configure(JoinBuilder.New()));
        return this;
    }

    /// <summary>Attach a COMPOSITE block (advanced blending/filters).</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Composite(Action<CompositeBuilder> configure) =>
        this.Composite(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Attach a COMPOSITE block (advanced blending/filters).</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder Composite(Func<CompositeBuilder, Composite> configure)
    {
        this.layer.Composite = configure(CompositeBuilder.New());
        return this;
    }

    /// <summary>Add one CLASS via nested builder.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder AddClass(Action<ClassBuilder> configure) =>
        this.AddClass(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Add one CLASS via nested builder.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LayerBuilder AddClass(Func<ClassBuilder, Class> configure)
    {
        this.layer.Classes.Add(configure(ClassBuilder.New()));
        return this;
    }

    public Layer Build() => this.layer;
}