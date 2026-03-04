// -----------------------------------------------------------------------
// <copyright file="CompositeBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System;

/// <summary>Fluent builder for <see cref="Composite"/> (layer compositing/blending).</summary>
public sealed class CompositeBuilder
{
    private readonly Composite _c = new();

    private CompositeBuilder()
    {
    }

    public static CompositeBuilder New() => new();

    public static CompositeBuilder From(Composite c)
    {
        var b = New().CompOp(c.CompOp).Opacity(c.Opacity);
        foreach (var f in c.CompFilters)
        {
            b.CompFilter(f);
        }

        return b;
    }

    public CompositeBuilder CompOp(string? name)
    {
        this._c.CompOp = name;
        return this;
    }

    public CompositeBuilder Opacity(int? pct)
    {
        this._c.Opacity = pct;
        return this;
    }

    public CompositeBuilder CompFilter(string filterCall)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(filterCall);
        this._c.CompFilters.Add(filterCall);
        return this;
    }

    public Composite Build() => this._c;
}