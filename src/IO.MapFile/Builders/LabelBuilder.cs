// -----------------------------------------------------------------------
// <copyright file="LabelBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System;
using System.Drawing;

/// <summary>
/// Fluent builder for <see cref="Label"/>.
/// </summary>
/// <remarks>
/// LABEL configures text rendering (font, size, placement, outline/halo via label STYLEs, etc.).
/// </remarks>
public sealed class LabelBuilder
{
    private readonly Label _l = new();

    private LabelBuilder()
    {
    }

    public static LabelBuilder New() => new();

    public static LabelBuilder From(Label l)
    {
        var b = New()
            .Color(l.Color)
            .OutlineColor(l.OutlineColor)
            .Font(l.Font)
            .Type(l.Type)
            .Size(l.Size)
            .Angle(l.Angle)
            .Align(l.Align)
            .Position(l.Position)
            .Partials(l.Partials)
            .Force(l.Force)
            .Buffer(l.Buffer)
            .MinFeatureSize(l.MinFeatureSize)
            .MinDistance(l.MinDistance)
            .Wrap(l.Wrap);

        foreach (var s in l.Styles)
        {
            b.AddStyle(sb => StyleBuilder.From(s).Build());
        }

        return b;
    }

    public LabelBuilder Color(Color? c)
    {
        if (c.HasValue)
        {
            this._l.Color = c.Value;
        }

        return this;
    }

    public LabelBuilder OutlineColor(Color? c)
    {
        if (c.HasValue)
        {
            this._l.OutlineColor = c.Value;
        }

        return this;
    }

    public LabelBuilder Font(string? name)
    {
        this._l.Font = name;
        return this;
    }

    public LabelBuilder Type(string? type)
    {
        this._l.Type = type;
        return this;
    }

    public LabelBuilder Size(string? size)
    {
        this._l.Size = size;
        return this;
    }

    public LabelBuilder Angle(string? v)
    {
        this._l.Angle = v;
        return this;
    }

    public LabelBuilder Align(HorizontalAlign? a)
    {
        this._l.Align = a;
        return this;
    }

    public LabelBuilder Position(LabelPosition p)
    {
        this._l.Position = p;
        return this;
    }

    public LabelBuilder Partials(bool? v)
    {
        this._l.Partials = v;
        return this;
    }

    public LabelBuilder Force(bool? v)
    {
        this._l.Force = v;
        return this;
    }

    public LabelBuilder Buffer(int? px)
    {
        this._l.Buffer = px;
        return this;
    }

    public LabelBuilder MinFeatureSize(int? px)
    {
        this._l.MinFeatureSize = px;
        return this;
    }

    public LabelBuilder MinDistance(int? px)
    {
        this._l.MinDistance = px;
        return this;
    }

    public LabelBuilder Wrap(string? ch)
    {
        this._l.Wrap = ch;
        return this;
    }

    public LabelBuilder Offset(int x, int y)
    {
        this._l.Offset = new(x, y);
        return this;
    }

    public LabelBuilder ShadowSize(int width, int height)
    {
        this._l.ShadowSize = new(width, height);
        return this;
    }

    /// <summary>Add a LABEL-STYLE (commonly used for halos with <c>GEOMTRANSFORM labelpoly</c>).</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public LabelBuilder AddStyle(Func<StyleBuilder, Style> configure)
    {
        this._l.Styles.Add(configure(StyleBuilder.New()));
        return this;
    }

    public Label Build() => this._l;
}