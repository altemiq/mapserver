// -----------------------------------------------------------------------
// <copyright file="ReferenceBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System.Drawing;

/// <summary>Fluent builder for <see cref="Reference"/>.</summary>
public sealed class ReferenceBuilder
{
    private readonly Reference _r = new();

    private ReferenceBuilder()
    {
    }

    public static ReferenceBuilder New() => new();

    public static ReferenceBuilder From(Reference r)
    {
        return New()
            .Color(r.Color)
            .OutlineColor(r.OutlineColor)
            .Extent(r.Extent)
            .Size(r.Size?.Width ?? 0, r.Size?.Height ?? 0, r.Size.HasValue)
            .Image(r.Image)
            .Marker(r.Marker)
            .MarkerSize(r.MarkerSize)
            .MinBoxSize(r.MinBoxSize)
            .MaxBoxSize(r.MaxBoxSize)
            .Status(r.Status);
    }

    public ReferenceBuilder Color(Color? c)
    {
        if (c.HasValue)
        {
            this._r.Color = c.Value;
        }

        return this;
    }

    public ReferenceBuilder OutlineColor(Color? c)
    {
        if (c.HasValue)
        {
            this._r.OutlineColor = c.Value;
        }

        return this;
    }

    public ReferenceBuilder Extent(BoundingBox? e)
    {
        this._r.Extent = e;
        return this;
    }

    /// <summary>Set SIZE. Provide <paramref name="apply"/> <c>false</c> to skip when you only pass zeros.</summary>
    /// <param name="w"></param>
    /// <param name="h"></param>
    /// <param name="apply"></param>
    /// <returns>The builder for chaining.</returns>
    public ReferenceBuilder Size(int w, int h, bool apply = true)
    {
        if (apply)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(w, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(h, 0);
            this._r.Size = new Size(w, h);
        }

        return this;
    }

    public ReferenceBuilder Image(string? file)
    {
        this._r.Image = file;
        return this;
    }

    public ReferenceBuilder Marker(string? symbol)
    {
        this._r.Marker = symbol;
        return this;
    }

    public ReferenceBuilder MarkerSize(int? px)
    {
        this._r.MarkerSize = px;
        return this;
    }

    public ReferenceBuilder MinBoxSize(int? px)
    {
        this._r.MinBoxSize = px;
        return this;
    }

    public ReferenceBuilder MaxBoxSize(int? px)
    {
        this._r.MaxBoxSize = px;
        return this;
    }

    public ReferenceBuilder Status(MapStatus s)
    {
        this._r.Status = s;
        return this;
    }

    public Reference Build() => this._r;
}