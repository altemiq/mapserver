// -----------------------------------------------------------------------
// <copyright file="Class.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// CLASS block: a thematic rule within a LAYER (filter + styles + labeling).
/// A feature is rendered by the first CLASS whose scale constraints and
/// <see cref="Expression"/> match (respecting LAYER order and any CLASSGROUP).
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1716:Identifiers should not match keywords",
    Justification = "MapServer nomenclature uses CLASS.")]
public sealed class Class
{
    /// <summary>
    /// Gets or sets the CLASS name used in legends.
    /// If not set, the class will not appear in the legend.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the CLASS group identifier (<c>GROUP</c>).
    /// Only classes whose <c>GROUP</c> matches the parent LAYER’s <c>CLASSGROUP</c>
    /// are considered at render time.
    /// </summary>
    public string? Group { get; set; }

    /// <summary>
    /// Gets or sets the CLASS <c>EXPRESSION</c> used to select features for this class.
    /// Supports string comparisons, regular expressions (<c>/regex/</c>), logical
    /// expressions, and functions. If empty, all features belong to the class.
    /// </summary>
    public string? Expression { get; set; }

    /// <summary>
    /// Gets or sets the minimum scale denominator at which this CLASS is drawn
    /// (<c>MAXSCALEDENOM</c>). Requests at a smaller denominator (more zoomed‑out)
    /// are clamped to this minimum for the class. Must be ≥ 0.
    /// </summary>
    public double? MaxScaleDenom { get; set; }

    /// <summary>
    /// Gets or sets the maximum scale denominator at which this CLASS is drawn
    /// (<c>MINSCALEDENOM</c>). Requests at a larger denominator (more zoomed‑in)
    /// are clamped to this maximum for the class. Must be ≥ 0.
    /// </summary>
    public double? MinScaleDenom { get; set; }

    /// <summary>
    /// Gets or sets the minimum map width (in map units) at which this CLASS is drawn
    /// (<c>MINGEOWIDTH</c>).
    /// </summary>
    public double? MinGeoWidth { get; set; }

    /// <summary>
    /// Gets or sets the maximum map width (in map units) at which this CLASS is drawn
    /// (<c>MAXGEOWIDTH</c>).
    /// </summary>
    public double? MaxGeoWidth { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this class is a fallback (like SLD’s ElseFilter);
    /// it applies only if no previous classes matched.
    /// </summary>
    public bool? Fallback { get; set; }

    /// <summary>
    /// Gets or sets the class‑level debug level (<c>DEBUG</c>) to emit diagnostics
    /// for this class (commonly 0–5, or ON/OFF depending on build). Output goes to STDERR
    /// or the MapServer error file.
    /// </summary>
    public int? DebugLevel { get; set; }

    /// <summary>
    /// Gets the STYLE objects used to draw features of this class. Multiple STYLEs
    /// are overlaid/stacked to form complex symbols.
    /// </summary>
    public IList<Style> Styles { get; } = [];

    /// <summary>
    /// Gets the LABEL objects used for text rendering associated with this class.
    /// </summary>
    public IList<Label> Labels { get; } = [];

    /// <summary>
    /// Gets the VALIDATION entries used to constrain runtime substitutions at the CLASS level.
    /// </summary>
    public IDictionary<string, string> Validation { get; } =
        new Dictionary<string, string>(StringComparer.Ordinal);
}