// -----------------------------------------------------------------------
// <copyright file="BoundingBoxBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// The <see cref="BoundingBox"/> builder.
/// </summary>
public sealed class BoundingBoxBuilder
{
#pragma warning disable CA1051, S1104, SA1401
    /// <summary>Gets or sets the min-X.</summary>
    public System.Lazy<double> MinX = new(() => default);

    /// <summary>Gets or sets the min-Y.</summary>
    public System.Lazy<double> MinY = new(() => default);

    /// <summary>Gets or sets the max-X.</summary>
    public System.Lazy<double> MaxX = new(() => default);

    /// <summary>Gets or sets the max-Y.</summary>
    public System.Lazy<double> MaxY = new(() => default);
#pragma warning restore CA1051, S1104, SA1401

    /// <summary>Gets or sets the object returned by this builder.</summary>
    /// <value>The constructed object.</value>
    public System.Lazy<Altemiq.IO.MapFile.BoundingBox>? BoundingBox { get; set; }

    /// <summary>Gets or sets the action to be performed when an object is built.</summary>
    /// <remarks>
    ///     This is only performed when an object is created from scratch for the first time.
    ///     When the object value has been injected from outside, this action will not be called.
    /// </remarks>
    public System.Action<Altemiq.IO.MapFile.BoundingBox>? PostBuildAction { get; set; }

    /// <summary>
    /// Builds the instance of <see cref="BoundingBox"/>.
    /// </summary>
    /// <returns>The instance of <see cref="BoundingBox"/>.</returns>
    public BoundingBox Build()
    {
        if (this.BoundingBox?.IsValueCreated is not true)
        {
            this.BoundingBox = new System.Lazy<Altemiq.IO.MapFile.BoundingBox>(() =>
            {
                var result = new Altemiq.IO.MapFile.BoundingBox
                {
                    MinX = this.MinX.Value,
                    MinY = this.MinY.Value,
                    MaxX = this.MaxX.Value,
                    MaxY = this.MaxY.Value,
                };

                return result;
            });

            this.PostBuildAction?.Invoke(this.BoundingBox.Value);
        }

        return this.BoundingBox.Value;
    }

    /// <summary>Sets the object to be returned by this instance.</summary>
    /// <param name="value">The object to be returned.</param>
    /// <returns>A reference to this builder instance.</returns>
    public BoundingBoxBuilder WithBoundingBox(Altemiq.IO.MapFile.BoundingBox value)
    {
        this.BoundingBox = new System.Lazy<Altemiq.IO.MapFile.BoundingBox>(() => value);
        this.WithValuesFrom(value);

        return this;
    }

    /// <summary>Populates this instance with values from the provided example.</summary>
    /// <param name="example">The example.</param>
    /// <remarks>This is a shallow clone, and does not traverse the example object creating builders for its properties.</remarks>
    /// <returns>A reference to this builder instance.</returns>
    public BoundingBoxBuilder WithValuesFrom(Altemiq.IO.MapFile.BoundingBox example) =>
        this
            .WithMinX(example.MinX)
            .WithMinY(example.MinY)
            .WithMaxX(example.MaxX)
            .WithMaxY(example.MaxY);

    /// <summary>Gets or sets the min-X.</summary>
    /// <param name="value">The value to set.</param>
    /// <returns>A reference to this builder instance.</returns>
    public BoundingBoxBuilder WithMinX(double value)
    {
        this.MinX = new(() => value);
        return this;
    }

    /// <summary>Removes the min-X.</summary>
    /// <returns>A reference to this builder instance.</returns>
    public BoundingBoxBuilder WithoutMinX()
    {
        this.MinX = new(() => default);
        return this;
    }

    /// <summary>Gets or sets the min-y.</summary>
    /// <param name="value">The value to set.</param>
    /// <returns>A reference to this builder instance.</returns>
    public BoundingBoxBuilder WithMinY(double value)
    {
        this.MinY = new(() => value);
        return this;
    }

    /// <summary>Removes the min-Y.</summary>
    /// <returns>A reference to this builder instance.</returns>
    public BoundingBoxBuilder WithoutMinY()
    {
        this.MinY = new(() => default);
        return this;
    }

    /// <summary>Gets or sets the max-X.</summary>
    /// <param name="value">The value to set.</param>
    /// <returns>A reference to this builder instance.</returns>
    public BoundingBoxBuilder WithMaxX(double value)
    {
        this.MaxX = new(() => value);
        return this;
    }

    /// <summary>Removes the max-X.</summary>
    /// <returns>A reference to this builder instance.</returns>
    public BoundingBoxBuilder WithoutMaxX()
    {
        this.MaxX = new(() => default);
        return this;
    }

    /// <summary>Gets or sets the max-Y.</summary>
    /// <param name="value">The value to set.</param>
    /// <returns>A reference to this builder instance.</returns>
    public BoundingBoxBuilder WithMaxY(double value)
    {
        this.MaxY = new(() => value);
        return this;
    }

    /// <summary>Removes the max-Y.</summary>
    /// <returns>A reference to this builder instance.</returns>
    public BoundingBoxBuilder WithoutMaxY()
    {
        this.MaxY = new(() => default);
        return this;
    }
}