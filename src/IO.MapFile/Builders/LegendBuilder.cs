// -----------------------------------------------------------------------
// <copyright file="LegendBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System;
using System.Drawing;

/// <summary>Fluent builder for <see cref="Legend"/>.</summary>
public sealed class LegendBuilder
{
    private readonly Legend _l = new();

    private LegendBuilder()
    {
    }

    public static LegendBuilder New() => new();

    public static LegendBuilder From(Legend l)
    {
        var b = New()
            .Status(l.Status)
            .ImageColor(l.ImageColor)
            .OutlineColor(l.OutlineColor)
            .KeySize(l.KeySize?.Width ?? 0, l.KeySize?.Height ?? 0, l.KeySize.HasValue)
            .KeySpacing(l.KeySpacing?.Width ?? 0, l.KeySpacing?.Height ?? 0, l.KeySpacing.HasValue)
            .Position(l.Position)
            .PostLabelCache(l.PostLabelCache)
            .Template(l.Template)
            .Transparent(l.Transparent);
        if (l.Label is not null)
        {
            b.Label(lb => LabelBuilder.From(l.Label!).Build());
        }

        return b;
    }

    public LegendBuilder Status(MapStatus s)
    {
        this._l.Status = s;
        return this;
    }

    public LegendBuilder ImageColor(Color? c)
    {
        if (c.HasValue)
        {
            this._l.ImageColor = c.Value;
        }

        return this;
    }

    public LegendBuilder OutlineColor(Color? c)
    {
        if (c.HasValue)
        {
            this._l.OutlineColor = c.Value;
        }

        return this;
    }

    public LegendBuilder KeySize(int w, int h, bool apply = true)
    {
        if (apply)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(w, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(h, 0);
            this._l.KeySize = new Size(w, h);
        }

        return this;
    }

    public LegendBuilder KeySpacing(int x, int y, bool apply = true)
    {
        if (apply)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(x, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(y, 0);
            this._l.KeySpacing = new Size(x, y);
        }

        return this;
    }

    public LegendBuilder Position(CornerPosition? p)
    {
        this._l.Position = p;
        return this;
    }

    public LegendBuilder PostLabelCache(bool? v)
    {
        this._l.PostLabelCache = v;
        return this;
    }

    public LegendBuilder Template(string? t)
    {
        this._l.Template = t;
        return this;
    }

    public LegendBuilder Transparent(bool? v)
    {
        this._l.Transparent = v;
        return this;
    }

    /// <summary>Set the legend LABEL block via nested builder.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LegendBuilder Label(Action<LabelBuilder> configure) =>
        this.Label(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Set the legend LABEL block via nested builder.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LegendBuilder Label(Func<LabelBuilder, Label> configure)
    {
        this._l.Label = configure(LabelBuilder.New());
        return this;
    }

    public Legend Build() => this._l;
}
