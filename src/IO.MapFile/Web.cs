// -----------------------------------------------------------------------
// <copyright file="Web.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// WEB block (CGI integration paths, formats, templates).
/// </summary>
/// <remarks>Commonly used in CGI workflows; includes IMAGEPATH/IMAGEURL and HTML template hooks.</remarks>
public sealed class Web
{
    public string? BrowseFormat { get; set; } // BROWSEFORMAT (e.g., "text/html", "image/svg+xml")

    public string? LegendFormat { get; set; } // LEGENDFORMAT

    public string? EmptyUrl { get; set; } // EMPTY

    public string? ErrorUrl { get; set; } // ERROR

    public string? HeaderTemplate { get; set; } // HEADER

    public string? FooterTemplate { get; set; } // FOOTER

    public string? ImagePath { get; set; } // IMAGEPATH (filesystem)

    public string? ImageUrl { get; set; } // IMAGEURL (public URL base)

    public double? MaxScaleDenom { get; set; } // MAXSCALEDENOM

    public string? MaxTemplate { get; set; } // MAXTEMPLATE

    public Dictionary<string, string> Metadata { get; } = [];

    public IDictionary<string, string> Validation { get; } = new Dictionary<string, string>(StringComparer.Ordinal);
}