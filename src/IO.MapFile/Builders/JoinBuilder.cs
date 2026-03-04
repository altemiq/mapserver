// -----------------------------------------------------------------------
// <copyright file="JoinBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System;

/// <summary>Fluent builder for <see cref="Join"/>.</summary>
public sealed class JoinBuilder
{
    private readonly Join _j = new();

    private JoinBuilder()
    {
        this._j.Name = "join";
    }

    public static JoinBuilder New() => new();

    public static JoinBuilder From(Join j)
        => New().Name(j.Name).Table(j.Table).FromField(j.From).ToField(j.To).Template(j.Template).Header(j.Header).Footer(j.Footer)
                .ConnectionType(j.ConnectionType).Connection(j.Connection).OneToMany(j.OneToMany);

    public JoinBuilder Name(string v)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(v);
        this._j.Name = v;
        return this;
    }

    public JoinBuilder Table(string? v)
    {
        this._j.Table = v;
        return this;
    }

    public JoinBuilder FromField(string? v)
    {
        this._j.From = v;
        return this;
    }

    public JoinBuilder ToField(string? v)
    {
        this._j.To = v;
        return this;
    }

    public JoinBuilder Template(string? v)
    {
        this._j.Template = v;
        return this;
    }

    public JoinBuilder Header(string? v)
    {
        this._j.Header = v;
        return this;
    }

    public JoinBuilder Footer(string? v)
    {
        this._j.Footer = v;
        return this;
    }

    public JoinBuilder ConnectionType(string? v)
    {
        this._j.ConnectionType = v;
        return this;
    }

    public JoinBuilder Connection(string? v)
    {
        this._j.Connection = v;
        return this;
    }

    public JoinBuilder OneToMany(bool v = true)
    {
        this._j.OneToMany = v;
        return this;
    }

    public Join Build() => this._j;
}
