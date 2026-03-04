// -----------------------------------------------------------------------
// <copyright file="GridBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>Fluent builder for <see cref="Grid"/>.</summary>
public sealed class GridBuilder
{
    private readonly Grid _g = new();

    private GridBuilder()
    {
    }

    public static GridBuilder New() => new();

    public static GridBuilder From(Grid g)
    {
        return New().LabelFormat(g.LabelFormat, g.LabelFormatCustom).MinArcs(g.MinArcs).MaxArcs(g.MaxArcs)
            .MinInterval(g.MinInterval).MaxInterval(g.MaxInterval).MinSubdivide(g.MinSubdivide).MaxSubdivide(g.MaxSubdivide);
    }

    public GridBuilder LabelFormat(GridLabelFormat fmt, string? custom = null)
    {
        this._g.LabelFormat = fmt;
        this._g.LabelFormatCustom = fmt == GridLabelFormat.Custom ? custom : null;
        return this;
    }

    public GridBuilder MinArcs(double? v)
    {
        this._g.MinArcs = v;
        return this;
    }

    public GridBuilder MaxArcs(double? v)
    {
        this._g.MaxArcs = v;
        return this;
    }

    public GridBuilder MinInterval(double? v)
    {
        this._g.MinInterval = v;
        return this;
    }

    public GridBuilder MaxInterval(double? v)
    {
        this._g.MaxInterval = v;
        return this;
    }

    public GridBuilder MinSubdivide(double? v)
    {
        this._g.MinSubdivide = v;
        return this;
    }

    public GridBuilder MaxSubdivide(double? v)
    {
        this._g.MaxSubdivide = v;
        return this;
    }

    public Grid Build() => this._g;
}
