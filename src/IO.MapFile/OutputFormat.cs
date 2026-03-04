// -----------------------------------------------------------------------
// <copyright file="OutputFormat.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;
/// <summary>
/// OUTPUTFORMAT object – declares image/vector output encodings (e.g., PNG, JPEG, PDF, SVG, GeoTIFF).
/// </summary>
/// <remarks>See OUTPUTFORMAT reference (driver, mimetype, imagemode, extension, formatoptions).</remarks>
public sealed class OutputFormat
{
    /// <summary>Gets or sets logical name of the format (referenced by MAP.IMAGETYPE).</summary>
    public string Name { get; set; } = "png";

    /// <summary>Gets or sets driver name (e.g., "AGG/PNG", "AGG/JPEG", "CAIRO/PDF", "GDAL/GTiff").</summary>
    public string Driver { get; set; } = "AGG/PNG";

    /// <summary>Gets or sets mime type (e.g., "image/png").</summary>
    public string? MimeType { get; set; }

    /// <summary>Gets or sets image mode hint – often implied by driver (e.g., RGB, RGBA).</summary>
    public string? ImageMode { get; set; }

    /// <summary>Gets or sets default file extension (e.g., "png", "jpg", "tif").</summary>
    public string? Extension { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether transparency should be enabled for this format.
    /// </summary>
    /// <remarks>
    /// Note that transparency does not work for IMAGEMODE RGB output. Not all formats support transparency (optional).
    /// When transparency is enabled for the typical case of 8-bit pseudocolored map generation, the IMAGECOLOR color will be marked as transparent in the output file palette.
    /// Any other map components drawn in this color will also be transparent, so for map generation with transparency it is best to use an otherwise unused color as the background color.
    /// </remarks>
    public bool? Transparent { get; set; }

    /// <summary>Gets driver/format specific options (FORMATOPTION key/value pairs).</summary>
    public IDictionary<string, string> FormatOptions { get; } = new Dictionary<string, string>(StringComparer.Ordinal);
}