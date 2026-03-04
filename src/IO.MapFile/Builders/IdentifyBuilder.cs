// -----------------------------------------------------------------------
// <copyright file="IdentifyBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>Fluent builder for <see cref="Identify"/> (8.6+).</summary>
public sealed class IdentifyBuilder
{
    private readonly Identify _i = new();

    private IdentifyBuilder()
    {
    }

    public static IdentifyBuilder New() => new();

    public static IdentifyBuilder From(Identify i)
        => New().Tolerance(i.Tolerance).ToleranceUnits(i.ToleranceUnits).ClassAuto(i.ClassAuto).ClassGroup(i.ClassGroup);

    public IdentifyBuilder Tolerance(double? v)
    {
        this._i.Tolerance = v;
        return this;
    }

    public IdentifyBuilder ToleranceUnits(MapUnits v)
    {
        this._i.ToleranceUnits = v;
        return this;
    }

    public IdentifyBuilder ClassAuto(bool? v = true)
    {
        this._i.ClassAuto = v;
        return this;
    }

    public IdentifyBuilder ClassGroup(string? v)
    {
        this._i.ClassGroup = v;
        return this;
    }

    public Identify Build() => this._i;
}
