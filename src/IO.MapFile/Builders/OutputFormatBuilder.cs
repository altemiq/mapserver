// -----------------------------------------------------------------------
// <copyright file="OutputFormatBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

using System;

/// <summary>
/// Fluent builder for <see cref="OutputFormat"/>.
/// </summary>
/// <remarks>
/// OUTPUTFORMAT declares a named renderer/driver (e.g., "AGG/PNG", "CAIRO/PDF").
/// Reference the NAME via <see cref="Map.ImageType"/> in the MAP object.
/// </remarks>
public sealed class OutputFormatBuilder
{
    private readonly OutputFormat outputFormat = new();

    private OutputFormatBuilder()
    {
    }

    public static OutputFormatBuilder New() => new();

    public static OutputFormatBuilder From(OutputFormat of)
    {
        var b = New()
            .Name(of.Name)
            .Driver(of.Driver)
            .MimeType(of.MimeType)
            .ImageMode(of.ImageMode)
            .Extension(of.Extension);
        foreach (var kv in of.FormatOptions)
        {
            b.FormatOption(kv.Key, kv.Value);
        }

        return b;
    }

    /// <summary>Logical format name (e.g., "png", "pdf", "GTiff").</summary>
    /// <param name="name"></param>
    /// <returns>The builder for chaining.</returns>
    public OutputFormatBuilder Name(string name)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(name);
        this.outputFormat.Name = name;
        return this;
    }

    /// <summary>Driver string (e.g., "AGG/PNG", "CAIRO/SVG", "GDAL/GTiff").</summary>
    /// <param name="driver"></param>
    /// <returns>The builder for chaining.</returns>
    public OutputFormatBuilder Driver(string driver)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(driver);
        this.outputFormat.Driver = driver;
        return this;
    }

    /// <summary>Mime-type (e.g., "image/png").</summary>
    /// <param name="mimetype"></param>
    /// <returns>The builder for chaining.</returns>
    public OutputFormatBuilder MimeType(string? mimetype)
    {
        this.outputFormat.MimeType = mimetype;
        return this;
    }

    /// <summary>Image mode hint (renderer dependent; often "RGB" or "RGBA").</summary>
    /// <param name="mode"></param>
    /// <returns>The builder for chaining.</returns>
    public OutputFormatBuilder ImageMode(string? mode)
    {
        this.outputFormat.ImageMode = mode;
        return this;
    }

    /// <summary>Default file extension (e.g., "png", "pdf", "tif").</summary>
    /// <param name="ext"></param>
    /// <returns>The builder for chaining.</returns>
    public OutputFormatBuilder Extension(string? ext)
    {
        this.outputFormat.Extension = ext;
        return this;
    }

    /// <summary>Add one FORMATOPTION pair (e.g., <c>GAMMA=0.75</c>).</summary>
    /// <param name="key">Tke key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The builder for chaining.</returns>
    public OutputFormatBuilder FormatOption(string key, string? value)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(key);
        this.outputFormat.FormatOptions[key] = value ?? string.Empty;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="OutputFormat.Transparent"/> value.
    /// </summary>
    /// <param name="v">The value to set.</param>
    /// <returns>The builder for chaining.</returns>
    public OutputFormatBuilder Transparent(bool v)
    {
        this.outputFormat.Transparent = v;
        return this;
    }

    public OutputFormat Build() => this.outputFormat;
}