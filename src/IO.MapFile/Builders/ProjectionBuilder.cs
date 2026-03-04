// -----------------------------------------------------------------------
// <copyright file="ProjectionBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System;

/// <summary>
/// Fluent builder for <see cref="Projection"/>.
/// </summary>
/// <remarks>
/// A PROJECTION in MapServer typically uses PROJ parameters or EPSG code lines.
/// Prefer <see cref="Epsg(int)"/> for portability; you may mix raw parameters if needed.
/// </remarks>
public sealed class ProjectionBuilder
{
    private readonly Projection projection = new();

    private ProjectionBuilder()
    {
    }

    /// <summary>Create a new projection builder.</summary>
    /// <returns>The builder for chaining.</returns>
    public static ProjectionBuilder New() => new();

    /// <summary>Start from an existing projection.</summary>
    /// <param name="p"></param>
    /// <returns>The builder for chaining.</returns>
    public static ProjectionBuilder From(Projection p)
    {
        var b = New();
        if (p.Auto == true)
        {
            b.Auto();
        }

        foreach (var line in p.Parameters)
        {
            b.Line(line);
        }

        return b;
    }

    /// <summary>Set PROJECTION AUTO (rare; MapServer decides based on data).</summary>
    /// <returns>The builder for chaining.</returns>
    public ProjectionBuilder Auto()
    {
        this.projection.Auto = true;
        this.projection.Parameters.Clear();
        return this;
    }

    /// <summary>
    /// Add a raw PROJ/parameter line (e.g., <c>"proj=utm"</c> or <c>"init=epsg:4326"</c>).
    /// </summary>
    /// <param name="paramLine"></param>
    /// <returns>The builder for chaining.</returns>
    public ProjectionBuilder Line(string paramLine)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(paramLine);
        this.projection.Parameters.Add(paramLine);
        this.projection.Auto = null;
        return this;
    }

    /// <summary>
    /// Convenience for EPSG usage: emits <c>"init=epsg:{code}"</c>.
    /// </summary>
    /// <param name="code"></param>
    /// <returns>The builder for chaining.</returns>
    public ProjectionBuilder Epsg(int code) => this.Line($"init=epsg:{code}");

    /// <summary>Build the <see cref="Projection"/>.</summary>
    /// <returns>The builder for chaining.</returns>
    public Projection Build() => this.projection;
}
