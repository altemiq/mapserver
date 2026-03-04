// -----------------------------------------------------------------------
// <copyright file="LeaderBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System;

/// <summary>Fluent builder for <see cref="Leader"/> (label leader lines).</summary>
public sealed class LeaderBuilder
{
    private readonly Leader _l = new();

    private LeaderBuilder()
    {
    }

    public static LeaderBuilder New() => new();

    public static LeaderBuilder From(Leader l)
    {
        var b = New().GridStep(l.GridStep).MaxDistance(l.MaxDistance);
        if (l.Style is not null)
        {
            b.Style(sb => StyleBuilder.From(l.Style!).Build());
        }

        return b;
    }

    public LeaderBuilder GridStep(int? px)
    {
        this._l.GridStep = px;
        return this;
    }

    public LeaderBuilder MaxDistance(int? px)
    {
        this._l.MaxDistance = px;
        return this;
    }

    public LeaderBuilder Style(Func<StyleBuilder, Style> configure)
    {
        this._l.Style = configure(StyleBuilder.New());
        return this;
    }

    public Leader Build() => this._l;
}
