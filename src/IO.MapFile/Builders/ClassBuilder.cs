// -----------------------------------------------------------------------
// <copyright file="ClassBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System;

/// <summary>
/// Fluent builder for <see cref="Class"/>.
/// </summary>
/// <remarks>
/// A CLASS selects features (via EXPRESSION) and attaches one or more STYLEs and LABELs to them.
/// </remarks>
public sealed class ClassBuilder
{
    private readonly Class _c = new();

    private ClassBuilder()
    {
    }

    public static ClassBuilder New() => new();

    public static ClassBuilder From(Class c)
    {
        var b = New()
            .Name(c.Name)
            .Group(c.Group)
            .Expression(c.Expression)
            .MinScaleDenom(c.MinScaleDenom)
            .MaxScaleDenom(c.MaxScaleDenom)
            .MinGeoWidth(c.MinGeoWidth)
            .MaxGeoWidth(c.MaxGeoWidth)
            .Fallback(c.Fallback)
            .DebugLevel(c.DebugLevel);

        foreach (var s in c.Styles)
        {
            b.AddStyle(sb => StyleBuilder.From(s).Build());
        }

        foreach (var l in c.Labels)
        {
            b.AddLabel(lb => LabelBuilder.From(l).Build());
        }

        foreach (var kv in c.Validation)
        {
            b.Validation(kv.Key, kv.Value);
        }

        return b;
    }

    public ClassBuilder Name(string? v)
    {
        this._c.Name = v;
        return this;
    }

    public ClassBuilder Group(string? v)
    {
        this._c.Group = v;
        return this;
    }

    public ClassBuilder Expression(string? expr)
    {
        this._c.Expression = expr;
        return this;
    }

    public ClassBuilder MinScaleDenom(double? v)
    {
        this._c.MinScaleDenom = v;
        return this;
    }

    public ClassBuilder MaxScaleDenom(double? v)
    {
        this._c.MaxScaleDenom = v;
        return this;
    }

    public ClassBuilder MinGeoWidth(double? v)
    {
        this._c.MinGeoWidth = v;
        return this;
    }

    public ClassBuilder MaxGeoWidth(double? v)
    {
        this._c.MaxGeoWidth = v;
        return this;
    }

    public ClassBuilder Fallback(bool? v = true)
    {
        this._c.Fallback = v;
        return this;
    }

    public ClassBuilder DebugLevel(int? level)
    {
        this._c.DebugLevel = level;
        return this;
    }

    /// <summary>Add a STYLE via nested builder.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public ClassBuilder AddStyle(Action<StyleBuilder> configure) =>
        this.AddStyle(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Add a STYLE via nested builder.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public ClassBuilder AddStyle(Func<StyleBuilder, Style> configure)
    {
        this._c.Styles.Add(configure(StyleBuilder.New()));
        return this;
    }

    /// <summary>Add a LABEL via nested builder.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public ClassBuilder AddLabel(Action<LabelBuilder> configure) =>
        this.AddLabel(builder =>
        {
            configure(builder);
            return builder.Build();
        });

    /// <summary>Add a LABEL via nested builder.</summary>
    /// <param name="configure">The function to create the value.</param>
    /// <returns>The builder for chaining.</returns>
    public ClassBuilder AddLabel(Func<LabelBuilder, Label> configure)
    {
        this._c.Labels.Add(configure(LabelBuilder.New()));
        return this;
    }

    /// <summary>Add or replace a CLASS VALIDATION key/value.</summary>
    /// <param name="key">Tke key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The builder for chaining.</returns>
    public ClassBuilder Validation(string key, string value)
    {
        this._c.Validation[key] = value;
        return this;
    }

    public Class Build() => this._c;
}