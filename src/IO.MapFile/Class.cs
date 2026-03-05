// -----------------------------------------------------------------------
// <copyright file="Class.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// CLASS block: defines a thematic rule within a LAYER (filter + style + labeling).
/// A feature is rendered by the first CLASS whose scale constraints and <see cref="Expression"/> match (honoring LAYER ordering and any CLASSGROUP).
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "This would make this confusing as this is called 'CLASS'")]
public sealed class Class
{
    /// <summary>
    /// Gets or sets the CLASS name used in legends. If not set, the class
    /// will not appear in the legend.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the CLASS group identifier (<c>GROUP</c>), which is only
    /// considered when the parent LAYER has <c>CLASSGROUP</c> set. Only classes
    /// with the same group value as <c>CLASSGROUP</c> are considered at render time.
    /// </summary>
    public string? Group { get; set; }

    /// <summary>
    /// Gets or sets the CLASS <c>EXPRESSION</c> used to select features for this class.
    /// Supports string comparisons, regular expressions (/regex/), logical expressions
    /// (e.g., <c>([POP] &gt; 50000 and [TYPE] = 'CITY')</c>), and string functions
    /// such as <c>length()</c>. If empty, all features belong to the class.
    /// </summary>
    public string? Expression { get; set; }

    /// <summary>
    /// Gets or sets the minimum scale denominator at which this CLASS is drawn
    /// (<c>MAXSCALEDENOM</c>). Requests at a *smaller* denominator (zoomed out more)
    /// are clamped to this min‑scale for the class. Must be ≥ 0. Replaces legacy MAXSCALE.
    /// </summary>
    public double? MaxScaleDenom { get; set; }

    /// <summary>
    /// Gets or sets the maximum scale denominator at which this CLASS is drawn
    /// (<c>MINSCALEDENOM</c>). Requests at a *larger* denominator (zoomed in more)
    /// are clamped to this max‑scale for the class. Must be ≥ 0. Replaces legacy MINSCALE.
    /// </summary>
    public double? MinScaleDenom { get; set; }

    /// <summary>
    /// Gets or sets the minimum geo width at which this CLASS is drawn.
    /// </summary>
    public double? MinGeoWidth { get; set; }

    /// <summary>
    /// Gets or sets the maximum geo width at which this CLASS is drawn.
    /// </summary>
    public double? MaxGeoWidth { get; set; }

    /// <summary>
    /// Gets or sets if true, acts like SLD ElseFilter—applies only if no previous classes matched.
    /// </summary>
    public bool? Fallback { get; set; }

    /// <summary>
    /// Gets or sets the class-level debug flag (<c>DEBUG</c>) to emit diagnostics
    /// for this class (0–5, or ON/OFF depending on docs). Output goes to STDERR or
    /// the MapServer error file. Useful while tuning rules.
    /// </summary>
    public int? DebugLevel { get; set; }

    /// <summary>
    /// Gets style(s) used to draw features of this class.
    /// </summary>
    public IList<Style> Styles { get; internal init; } = [];

    /// <summary>
    /// Gets the style list.
    /// A CLASS can contain multiple STYLE objects that are overlaid/stacked to form complex symbols.
    /// Use STYLEs (not legacy color/size keys).
    /// </summary>
    public IList<Label> Labels { get; internal init; } = [];

    public IDictionary<string, string> Validation { get; internal init; } = new Dictionary<string, string>(StringComparer.Ordinal);
}