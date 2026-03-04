// -----------------------------------------------------------------------
// <copyright file="BoundingBoxBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System.Runtime.CompilerServices;

/// <summary>
/// Fluent builder for <see cref="BoundingBox"/>.
/// </summary>
/// <remarks>
/// A convenience wrapper to avoid directly instantiating <see cref="BoundingBox"/> for extents.
/// </remarks>
public sealed class BoundingBoxBuilder
{
    private double minX;
    private double minY;
    private double maxX;
    private double maxY;

    private BoundingBoxBuilder()
    {
    }

    /// <summary>Create a new <see cref="BoundingBoxBuilder"/>.</summary>
    /// <returns>The builder for chaining.</returns>
    public static BoundingBoxBuilder New() => new();

    /// <summary>Start from an existing <see cref="BoundingBox"/>.</summary>
    /// <param name="bbox">The bounding box.</param>
    /// <returns>The builder for chaining.</returns>
    public static BoundingBoxBuilder From(BoundingBox bbox) =>
        New().MinX(bbox.MinX).MinY(bbox.MinY).MaxX(bbox.MaxX).MaxY(bbox.MaxY);

    /// <summary>Set minimum X.</summary>
    /// <param name="v">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public BoundingBoxBuilder MinX(double v)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(v, double.NaN);
        ArgumentOutOfRangeException.ThrowIfEqual(v, double.PositiveInfinity);
        this.minX = v;
        return this;
    }

    /// <summary>Set minimum Y.</summary>
    /// <param name="v">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public BoundingBoxBuilder MinY(double v)
    {
        this.minY = Finite(v);
        return this;
    }

    /// <summary>Set maximum X.</summary>
    /// <param name="v">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public BoundingBoxBuilder MaxX(double v)
    {
        this.maxX = Finite(v);
        return this;
    }

    /// <summary>Set maximum Y.</summary>
    /// <param name="v">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public BoundingBoxBuilder MaxY(double v)
    {
        this.maxY = Finite(v);
        return this;
    }

    /// <summary>Set all four values in one call.</summary>
    /// <param name="minX">The minimum x-value.</param>
    /// <param name="minY">The minimum y-value.</param>
    /// <param name="maxX">The maximum x-value.</param>
    /// <param name="maxY">The maximum y-value.</param>
    /// <returns>The builder for chaining.</returns>
    public BoundingBoxBuilder Values(double minX, double minY, double maxX, double maxY) => this.MinX(minX).MinY(minY).MaxX(maxX).MaxY(maxY);

    /// <summary>Build the <see cref="BoundingBox"/> instance.</summary>
    /// <returns>The builder for chaining.</returns>
    public BoundingBox Build() => new(this.minX, this.minY, this.maxX, this.maxY);

    private static double Finite(double value, [CallerArgumentExpression(nameof(value))] string? param = default)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentOutOfRangeException(param, $"{param} must be finite.");
        }

        return value;
    }
}