// -----------------------------------------------------------------------
// <copyright file="ScaleBarBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System;
using System.Drawing;

/// <summary>Fluent builder for <see cref="ScaleBar"/>.</summary>
public sealed class ScaleBarBuilder
{
    private readonly ScaleBar _s = new();

    private ScaleBarBuilder()
    {
    }

    public static ScaleBarBuilder New() => new();

    public static ScaleBarBuilder From(ScaleBar s)
    {
        var b = New()
            .Status(s.Status)
            .ImageColor(s.ImageColor)
            .BackColor(s.BackColor)
            .Color(s.Color)
            .OutlineColor(s.OutlineColor)
            .Size(s.Size?.Width ?? 0, s.Size?.Height ?? 0, s.Size.HasValue)
            .Position(s.Position)
            .Align(s.Align)
            .Intervals(s.Intervals)
            .Offset(s.Offset?.X ?? 0, s.Offset?.Y ?? 0, s.Offset.HasValue)
            .Units(s.Units)
            .Transparent(s.Transparent);
        if (s.Label is not null)
        {
            b.Label(lb => LabelBuilder.From(s.Label!).Build());
        }

        return b;
    }

    public ScaleBarBuilder Status(MapStatus v)
    {
        this._s.Status = v;
        return this;
    }

    public ScaleBarBuilder ImageColor(Color? v)
    {
        if (v.HasValue)
        {
            this._s.ImageColor = v.Value;
        }

        return this;
    }

    public ScaleBarBuilder BackColor(Color? v)
    {
        if (v.HasValue)
        {
            this._s.BackColor = v.Value;
        }

        return this;
    }

    public ScaleBarBuilder Color(Color? v)
    {
        if (v.HasValue)
        {
            this._s.Color = v.Value;
        }

        return this;
    }

    public ScaleBarBuilder OutlineColor(Color? v)
    {
        if (v.HasValue)
        {
            this._s.OutlineColor = v.Value;
        }

        return this;
    }

    public ScaleBarBuilder Size(int w, int h, bool apply = true)
    {
        if (apply)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(w, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(h, 0);
            this._s.Size = new Size(w, h);
        }

        return this;
    }

    public ScaleBarBuilder Position(CornerPosition? v)
    {
        this._s.Position = v;
        return this;
    }

    public ScaleBarBuilder Align(HorizontalAlign? v)
    {
        this._s.Align = v;
        return this;
    }

    public ScaleBarBuilder Intervals(int? v)
    {
        this._s.Intervals = v;
        return this;
    }

    public ScaleBarBuilder Offset(int x, int y, bool apply = true)
    {
        if (apply)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(x, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(y, 0);
            this._s.Offset = new Point(x, y);
        }

        return this;
    }

    public ScaleBarBuilder Units(MapUnits? v)
    {
        this._s.Units = v;
        return this;
    }

    public ScaleBarBuilder Transparent(bool? v)
    {
        this._s.Transparent = v;
        return this;
    }

    public ScaleBarBuilder Label(Action<LabelBuilder> configure) =>
        this.Label(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    public ScaleBarBuilder Label(Func<LabelBuilder, Label> configure)
    {
        this._s.Label = configure(LabelBuilder.New());
        return this;
    }

    public ScaleBar Build() => this._s;
}
