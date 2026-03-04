// -----------------------------------------------------------------------
// <copyright file="StyleBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// Fluent builder for <see cref="Style"/>.
/// </summary>
/// <remarks>
/// STYLE defines symbolization (colors, stroke width, hatch spacing, angle, etc.).
/// </remarks>
public sealed class StyleBuilder
{
    private readonly Style style = new();

    private StyleBuilder()
    {
    }

    public static StyleBuilder New() => new();

    public static StyleBuilder From(Style s)
    {
        var b = New()
            .Color(s.Color)
            .OutlineColor(s.OutlineColor)
            .Symbol(s.Symbol)
            .Width(s.Width)
            .Size(s.Size)
            .Angle(s.Angle)
            .Gap(s.Gap)
            .Offset(s.Offset)
            .Opacity(s.Opacity)
            .GeomTransform(s.GeomTransform)
            .MinScaleDenom(s.MinScaleDenom)
            .MaxScaleDenom(s.MaxScaleDenom);

        if (s.Pattern.Count > 0)
        {
            b.Pattern(s.Pattern);
        }

        return b;
    }

    public StyleBuilder Color(byte r, byte g, byte b) => this.Color(System.Drawing.Color.FromArgb(r, g, b));

    public StyleBuilder Color(ColorOrAttribute c)
    {
        this.style.Color = c;
        return this;
    }

    public StyleBuilder OutlineColor(byte r, byte g, byte b) => this.OutlineColor(System.Drawing.Color.FromArgb(r, g, b));

    public StyleBuilder OutlineColor(ColorOrAttribute c)
    {
        this.style.OutlineColor = c;
        return this;
    }

    public StyleBuilder Symbol(string? name)
    {
        this.style.Symbol = name;
        return this;
    }

    public StyleBuilder Width(double? px)
    {
        this.style.Width = px;
        return this;
    }

    public StyleBuilder Size(double? value)
    {
        this.style.Size = value;
        return this;
    }

    public StyleBuilder Angle(Angle value)
    {
        this.style.Angle = value;
        return this;
    }

    /// <summary>Set a dash PATTERN. Values are rendered in layer SIZEUNITS.</summary>
    /// <param name="dashes">The dash pattern.</param>
    /// <returns>The builder for chaining.</returns>
    public StyleBuilder Pattern(IEnumerable<double> dashes)
    {
        this.style.Pattern.Clear();
        foreach (var d in dashes)
        {
            this.style.Pattern.Add(d);
        }

        return this;
    }

    public StyleBuilder Gap(double? value)
    {
        this.style.Gap = value;
        return this;
    }

    public StyleBuilder Offset(Point? px)
    {
        this.style.Offset = px;
        return this;
    }

    public StyleBuilder Opacity(int? pct)
    {
        this.style.Opacity = pct;
        return this;
    }

    public StyleBuilder GeomTransform(string? expr)
    {
        this.style.GeomTransform = expr;
        return this;
    }

    public StyleBuilder MinScaleDenom(double? v)
    {
        this.style.MinScaleDenom = v;
        return this;
    }

    public StyleBuilder MaxScaleDenom(double? v)
    {
        this.style.MaxScaleDenom = v;
        return this;
    }

    public Style Build() => this.style;
}