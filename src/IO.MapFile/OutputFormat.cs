// -----------------------------------------------------------------------
// <copyright file="OutputFormat.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Declares an available output format (<c>OUTPUTFORMAT</c>): driver, MIME type,
/// image mode, extension, and driver-specific <c>FORMATOPTION</c>s.
/// </summary>
public sealed class OutputFormat
{
    /// <summary>
    /// Gets or sets the logical name of the format, referenced by MAP <c>IMAGETYPE</c>.
    /// </summary>
    public string Name { get; set; } = "png";

    /// <summary>
    /// Gets or sets the driver name (e.g., <c>AGG/PNG</c>, <c>AGG/JPEG</c>, <c>CAIRO/PDF</c>, <c>GDAL/GTiff</c>).
    /// </summary>
    public string Driver { get; set; } = "AGG/PNG";

    /// <summary>Gets or sets the MIME type (e.g., <c>image/png</c>).</summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// Gets or sets the image mode hint — often implied by driver (<c>RGB</c>, <c>RGBA</c>, etc.).
    /// </summary>
    public string? ImageMode { get; set; }

    /// <summary>Gets or sets the default file extension (<c>png</c>, <c>jpg</c>, <c>tif</c>, etc.).</summary>
    public string? Extension { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether transparency is enabled where supported.
    /// </summary>
    /// <remarks>
    /// For classic RGB output transparency is not effective; paletted/alpha formats may
    /// mark the background color as transparent (be careful to use a background color
    /// not otherwise used by map content).
    /// </remarks>
    public bool? Transparent { get; set; }

    /// <summary>
    /// Gets the driver/format-specific options (<c>FORMATOPTION</c> entries).
    /// </summary>
    public IDictionary<string, string> FormatOptions { get; } = new Dictionary<string, string>(StringComparer.Ordinal);
}