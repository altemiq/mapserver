// -----------------------------------------------------------------------
// <copyright file="QueryMapBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System.Drawing;

/// <summary>Fluent builder for <see cref="QueryMap"/>.</summary>
public sealed class QueryMapBuilder
{
    private readonly QueryMap _q = new();

    private QueryMapBuilder()
    {
    }

    public static QueryMapBuilder New() => new();

    public static QueryMapBuilder From(QueryMap q)
    {
        return New().Status(q.Status).Color(q.Color).Size(q.Size?.Width ?? 0, q.Size?.Height ?? 0, q.Size.HasValue).Style(q.Style);
    }

    public QueryMapBuilder Status(MapStatus s)
    {
        this._q.Status = s;
        return this;
    }

    public QueryMapBuilder Color(Color? c)
    {
        if (c.HasValue)
        {
            this._q.Color = c.Value;
        }

        return this;
    }

    public QueryMapBuilder Size(int w, int h, bool apply = true)
    {
        if (apply)
        {
            this._q.Size = new Size(w, h);
        }

        return this;
    }

    public QueryMapBuilder Style(QueryMapStyle s)
    {
        this._q.Style = s;
        return this;
    }

    public QueryMap Build() => this._q;
}
