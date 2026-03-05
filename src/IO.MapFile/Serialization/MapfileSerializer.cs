// -----------------------------------------------------------------------
// <copyright file="MapfileSerializer.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Serialization;

using System.Drawing;
using System.Globalization;
using System.Text;

/// <summary>
/// Serializes the model back to a MapServer .map file text.
/// </summary>
public static class MapfileSerializer
{
    /// <summary>
    /// Deserializes the specified text into a <see cref="Map"/> object.
    /// </summary>
    /// <param name="text">The text to deserialize.</param>
    /// <returns>The map.</returns>
    public static Map Deserialize(string text) => Deserialize(text.AsSpan());

    /// <summary>
    /// Deserializes the specified text into a <see cref="Map"/> object.
    /// </summary>
    /// <param name="text">The text to deserialize.</param>
    /// <returns>The map.</returns>
    public static Map Deserialize(ReadOnlySpan<char> text)
    {
        var parser = new MapfileParser(text);
        return parser.ParseMap();
    }

    /// <summary>
    /// Serializes the specified <see cref="Map"/> to a string.
    /// </summary>
    /// <param name="map">The map.</param>
    /// <param name="options">The options.</param>
    /// <returns>The serialized version of <paramref name="map"/>.</returns>
    public static string Serialize(Map map, MapfileSerializationOptions? options = null)
    {
        var o = options ?? new MapfileSerializationOptions();
        var w = new Writer(o);

        w.Line("MAP");
        w.WithIndent(() =>
        {
            w.PropQuoted("NAME", map.Name);
            w.Prop("EXTENT", map.Extent, Serialize);
            w.Prop("STATUS", map.Status, Serialize);
            w.Prop("SIZE", map.Size);
            w.Prop("UNITS", map.Units, Serialize);
            w.Prop("IMAGECOLOR", map.ImageColor);
            w.PropQuoted("FONTSET", map.FontSet);
            w.PropQuoted("SYMBOLSET", map.SymbolSet);
            w.PropQuoted("SHAPEPATH", map.ShapePath);
            w.PropQuoted("IMAGETYPE", map.ImageType);
            w.Prop("MAXSIZE", map.MaxSize);
            w.Prop("DEBUG", map.DebugLevel);

            if (map.Web is not null)
            {
                w.Web(map.Web);
            }

            if (map.Config is not null)
            {
                foreach (var cfg in map.Config)
                {
                    w.PropKeyValuePair("CONFIG", cfg.Key, cfg.Value);
                }
            }

            w.Metadata(map.Metadata);

            foreach (var outputFormat in map.OutputFormats)
            {
                w.OutputFormat(outputFormat);
            }

            if (map.Projection is not null)
            {
                w.Projection(map.Projection);
            }

            if (map.Reference is not null)
            {
                w.Reference(map.Reference);
            }

            if (map.Legend is not null)
            {
                w.Legend(map.Legend);
            }

            if (map.QueryMap is not null)
            {
                w.QueryMap(map.QueryMap);
            }

            if (map.ScaleBar is not null)
            {
                w.ScaleBar(map.ScaleBar);
            }

            foreach (var layer in map.Layers)
            {
                w.Layer(layer);
            }
        });
        w.Line("END # MAP");

        return w.ToString();
    }

    private static string Serialize(MapStatus s) =>
        s switch
        {
            MapStatus.On => "ON",
            MapStatus.Off => "OFF",
            MapStatus.Default => "DEFAULT",
            MapStatus.Embed => "EMBED",
            _ => "OFF",
        };

    private static string Serialize(MapUnits u) =>
        u switch
        {
            MapUnits.Pixels => "PIXELS",
            MapUnits.Feet => "FEET",
            MapUnits.Inches => "INCHES",
            MapUnits.Kilometers => "KILOMETERS",
            MapUnits.Meters => "METERS",
            MapUnits.Miles => "MILES",
            MapUnits.NauticalMiles => "NAUTICALMILES",
            MapUnits.DD => "DD",
            _ => "METERS",
        };

    private static string Serialize(BoundingBox b) => FormattableString.Invariant($"{b.MinX} {b.MinY} {b.MaxX} {b.MaxY}");

    private static string Serialize(Color c, MapfileSerializationOptions o)
    {
        if (o.PreferHexColors)
        {
            var a = c.A is byte.MaxValue ? string.Empty : c.A.ToString("X2", System.Globalization.CultureInfo.InvariantCulture);
            return $"\"#{c.R:X2}{c.G:X2}{c.B:X2}{a}\"";
        }

        // r g b [a]
        return c.A is byte.MaxValue
            ? $"{c.R} {c.G} {c.B}"
            : $"{c.R} {c.G} {c.B} {c.A}";
    }

    private sealed class Writer(MapfileSerializationOptions o)
    {
        private readonly StringBuilder builder = new();
        private readonly MapfileSerializationOptions options = o;
        private int indent;

        public void Line(System.IFormatProvider? provider, [System.Runtime.CompilerServices.InterpolatedStringHandlerArgument("", nameof(provider))] ref IndentInterpolatedStringHandler handler) => this.builder.Append(this.options.NewLine);

        public void Line(ReadOnlySpan<char> text = default)
        {
            for (var i = 0; i < this.indent; i++)
            {
                this.builder.Append(this.options.Indent);
            }

            this.builder.Append(text);
            this.builder.Append(this.options.NewLine);
        }

        public void WithIndent(Action body)
        {
            this.indent++;
            body();
            this.indent--;
        }

        public void Prop<T>(string name, T value, Func<T, string?> serialize, T defaultValue = default)
            where T : struct
        {
            if (!value.Equals(defaultValue) && serialize(value) is { } serialized)
            {
                this.Line(default, $"{name} {serialized}");
            }
        }

        public void Prop<T>(string name, T? value, Func<T, string?> serialize)
            where T : struct
        {
            if (value is { } v && serialize(v) is { } serialized)
            {
                this.Line(default, $"{name} {serialized}");
            }
        }

        public void Prop<T>(string name, T? value)
            where T : struct
        {
            if (value is { } v)
            {
                this.Line(System.Globalization.CultureInfo.InvariantCulture, $"{name} {v}");
            }
        }

        public void Prop(string name, Color? value) => this.Prop(name, value, color => MapfileSerializer.Serialize(color, this.options));

        public void Prop(string name, Point? value) => this.Prop(name, value, Serialize);

        public void Prop(string name, Size value) => this.Prop(name, value, Serialize);

        public void Prop(string name, Size? value) => this.Prop(name, value, Serialize);

        public void Prop(string name, bool? value) => this.Prop(name, value, "TRUE", "FALSE");

        public void Prop(string name, bool? value, string trueString, string falseString)
        {
            if (value is { } v)
            {
                this.Line(default, $"{name} {(v ? trueString : falseString)}");
            }
        }

        public void Prop(string name, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                this.Line(default, $"{name} {value}");
            }
        }

        public void Prop(string name, bool quote, System.IFormatProvider? provider, [System.Runtime.CompilerServices.InterpolatedStringHandlerArgument("", nameof(provider), nameof(name), nameof(quote))] ref IndentInterpolatedStringHandler handler)
        {
            if (quote)
            {
                this.builder.Append('\"');
            }

            this.builder.Append(this.options.NewLine);
        }

        public void PropQuoted(string name, bool quote, System.IFormatProvider? provider, [System.Runtime.CompilerServices.InterpolatedStringHandlerArgument("", nameof(provider), nameof(name), nameof(quote))] ref IndentInterpolatedStringHandler handler)
        {
            if (quote)
            {
                this.builder.Append('\"');
            }

            this.builder.Append(this.options.NewLine);
        }

        public void PropQuoted(string name, System.IFormatProvider? provider, [System.Runtime.CompilerServices.InterpolatedStringHandlerArgument("", nameof(provider), nameof(name))] ref IndentInterpolatedStringHandler handler) => this.builder.Append('\"').Append(this.options.NewLine);

        public void PropQuoted(string name, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                this.Line(default, $"{name} {DoubleQuote(value)}");
            }
        }

        public void PropKeyValuePair(string name, string key, string value) => this.Line(default, $"{name} {DoubleQuote(key)} {DoubleQuote(value)}");

        public void Dictionary(string name, IDictionary<string, string> dictionary)
        {
            if (dictionary is null || (this.options.OmitEmptyBlocks && dictionary.Count == 0))
            {
                return;
            }

            this.Block(
                name,
                () =>
                {
                    var quoted = dictionary.ToDictionary(
                        kvp => DoubleQuote(kvp.Key),
                        kvp => DoubleQuote(kvp.Value),
                        StringComparer.Ordinal);

                    var keyLength = quoted.Keys.Max(key => key.Length);

                    foreach (var kv in quoted)
                    {
                        this.Line(default, $"{kv.Key.PadRight(keyLength)} {kv.Value}");
                    }
                });
        }

        public void Metadata(IDictionary<string, string> md) => this.Dictionary("METADATA", md);

        public void Validation(IDictionary<string, string> v)
        {
            if (v is null || (this.options.OmitEmptyBlocks && v.Count == 0))
            {
                return;
            }

            this.Block(
                "VALIDATION",
                () =>
                {
                    var quoted = v.ToDictionary(
                        kvp => SingleQuote(kvp.Key),
                        kvp => SingleQuote(kvp.Value),
                        StringComparer.Ordinal);

                    var keyLength = quoted.Keys.Max(key => key.Length);

                    foreach (var kv in quoted)
                    {
                        this.Line(default, $"{kv.Key.PadRight(keyLength)} {kv.Value}");
                    }
                });
        }

        public void Projection(Projection projection) =>
            this.Block(
                "PROJECTION",
                () =>
                {
                    if (projection.Auto is true)
                    {
                        this.Line("AUTO");
                    }
                    else
                    {
                        foreach (var p in projection.Parameters)
                        {
                            this.Line(DoubleQuote(p));
                        }
                    }
                });

        public void Web(Web web) =>
            this.Block(
                "WEB",
                () =>
                {
                    this.PropQuoted("BROWSEFORMAT", web.BrowseFormat);
                    this.PropQuoted("LEGENDFORMAT", web.LegendFormat);
                    this.PropQuoted("EMPTY", web.EmptyUrl);
                    this.PropQuoted("ERROR", web.ErrorUrl);
                    this.PropQuoted("FOOTER", web.FooterTemplate);
                    this.PropQuoted("HEADER", web.HeaderTemplate);
                    this.PropQuoted("IMAGEPATH", web.ImagePath);
                    this.PropQuoted("IMAGEURL", web.ImageUrl);
                    this.Prop("MAXSCALEDENOM", web.MaxScaleDenom);
                    this.PropQuoted("MAXTEMPLATE", web.MaxTemplate);
                    this.Metadata(web.Metadata);
                    this.Validation(web.Validation);
                });

        public void OutputFormat(OutputFormat of) =>
            this.Block(
                "OUTPUTFORMAT",
                () =>
                {
                    this.PropQuoted("NAME", of.Name);
                    this.PropQuoted("MIMETYPE", of.MimeType);
                    this.Prop("DRIVER", of.Driver);
                    this.PropQuoted("EXTENSION", of.Extension);
                    this.Prop("IMAGEMODE", of.ImageMode);
                    this.Prop("TRANSPARENT", of.Transparent);
                    foreach (var f in of.FormatOptions)
                    {
                        this.Line(default, $"FORMATOPTION {DoubleQuote($"{f.Key}={f.Value}")}");
                    }
                });

        public void Reference(Reference r) =>
            this.Block(
                "REFERENCE",
                () =>
                {
                    this.Prop("COLOR", r.Color);

                    this.Prop("OUTLINECOLOR", r.OutlineColor);

                    if (r.Extent is { } extent)
                    {
                        this.Prop("EXTENT", extent, MapfileSerializer.Serialize);
                    }

                    this.Prop("SIZE", r.Size);

                    this.PropQuoted("IMAGE", r.Image);
                    this.PropQuoted("MARKER", r.Marker);

                    this.Prop("MARKERSIZE", r.MarkerSize);
                    this.Prop("MINBOXSIZE", r.MinBoxSize);
                    this.Prop("MAXBOXSIZE", r.MaxBoxSize);

                    this.Prop("STATUS", r.Status, MapfileSerializer.Serialize, MapStatus.On);
                });

        public void Legend(Legend l) =>
            this.Block(
                "LEGEND",
                () =>
                {
                    this.Prop("IMAGECOLOR", l.ImageColor);
                    this.Prop("OUTLINECOLOR", l.OutlineColor);
                    this.Prop("KEYSIZE", l.KeySize);
                    this.Prop("KEYSPACING", l.KeySpacing);
                    this.Prop("POSITION", l.Position, Serialize);
                    this.Prop("POSTLABELCACHE", l.PostLabelCache);
                    this.PropQuoted("TEMPLATE", l.Template);
                    this.Prop("TRANSPARENT", l.Transparent, "ON", "OFF");

                    if (l.Label is not null)
                    {
                        this.Label(l.Label);
                    }

                    this.Prop("STATUS", l.Status, MapfileSerializer.Serialize, MapStatus.On);
                });

        public void ScaleBar(ScaleBar s) =>
            this.Block(
                "SCALEBAR",
                () =>
                {
                    this.Prop("INTERVALS", s.Intervals);

                    if (s.Label is not null)
                    {
                        this.Label(s.Label);
                    }

                    this.Prop("SIZE", s.Size);
                    this.Prop("STATUS", s.Status, MapfileSerializer.Serialize);
                    this.Prop("IMAGECOLOR", s.ImageColor);
                    this.Prop("BACKGROUNDCOLOR", s.BackColor);
                    this.Prop("COLOR", s.Color);
                    this.Prop("OUTLINECOLOR", s.OutlineColor);
                    this.Prop("POSITION", s.Position, Serialize);
                    this.Prop("ALIGN", s.Align, Serialize);
                    this.Prop("OFFSET", s.Offset, Serialize);
                    this.Prop("UNITS", s.Units, MapfileSerializer.Serialize);
                    this.Prop("TRANSPARENT", s.Transparent, "ON", "OFF");
                });

        public void QueryMap(QueryMap q) =>
            this.Block(
                "QUERYMAP",
                () =>
                {
                    this.Prop("SIZE", q.Size);
                    this.Prop("COLOR", q.Color);
                    this.Prop("STATUS", q.Status, MapfileSerializer.Serialize);
                    this.Prop("STYLE", q.Style.ToString().ToUpperInvariant());
                });

        public void Layer(Layer layer) =>
            this.Block(
                "LAYER",
                () =>
                {
                    this.PropQuoted("GROUP", layer.Group);
                    this.PropQuoted("NAME", layer.Name);
                    this.PropQuoted("CLASSGROUP", layer.ClassGroup);
                    this.Prop("CONNECTIONTYPE", layer.ConnectionType, Serialize);
                    this.PropQuoted("CONNECTION", layer.Connection);
                    this.Dictionary("CONNECTIONOPTIONS", layer.ConnectionOptions);
                    foreach (var p in layer.Processing)
                    {
                        this.Prop("PROCESSING", quote: true, default, $"{p}");
                    }

                    this.Prop("STATUS", layer.Status, MapfileSerializer.Serialize, MapStatus.Off);
                    this.Metadata(layer.Metadata);
                    this.Prop("TYPE", layer.Type, Serialize);

                    this.Prop("TOLERANCE", layer.Tolerance);
                    this.Prop("TOLERANCEUNITS", layer.ToleranceUnits, MapfileSerializer.Serialize);
                    this.PropQuoted("HEADER", layer.Header);
                    this.PropQuoted("FOOTER", layer.Footer);
                    this.PropQuoted("TEMPLATE", layer.Template);
                    this.Prop("UNITS", layer.Units, MapfileSerializer.Serialize);

                    this.PropQuoted("DATA", layer.Data);
                    this.Dictionary("BINDVALS", layer.BindVals);
                    this.PropQuoted("CLASSITEM", layer.ClassItem);
                    if (!string.IsNullOrWhiteSpace(layer.Filter))
                    {
                        this.Prop("FILTER", quote: false, default, $"{this.SerializeExpression(layer.Filter)}");
                    }

                    this.PropQuoted("FILTERITEM", layer.FilterItem);
                    this.PropQuoted("LABELITEM", layer.LabelItem);

                    this.Validation(layer.Validation);

                    if (!string.IsNullOrWhiteSpace(layer.LabelRequires))
                    {
                        this.Prop("LABELREQUIRES", quote: false, default, $"{this.SerializeExpression(layer.LabelRequires)}");
                    }

                    this.Prop("LABELMINSCALEDENOM", layer.LabelMinScaleDenom);
                    this.Prop("LABELMAXSCALEDENOM", layer.LabelMaxScaleDenom);
                    this.Prop("MINSCALEDENOM", layer.MinScaleDenom);
                    this.Prop("MAXSCALEDENOM", layer.MaxScaleDenom);
                    this.Prop("MINGEOWIDTH", layer.MinGeoWidth);
                    this.Prop("MAXGEOWIDTH", layer.MaxGeoWidth);
                    this.Prop("MINFEATURESIZE", layer.MinFeatureSize);
                    this.Prop("MAXFEATURES", layer.MaxFeatures);
                    if (!string.IsNullOrWhiteSpace(layer.Requires))
                    {
                        this.Prop("REQUIRES", quote: false, default, $"{this.SerializeExpression(layer.Requires)}");
                    }

                    this.PropQuoted("MASK", layer.Mask);
                    this.Prop("SIZEUNITS", layer.SizeUnits, MapfileSerializer.Serialize);

                    if (layer.Projection is not null)
                    {
                        this.Projection(layer.Projection);
                    }

                    this.Prop("EXTENT", layer.Extent, MapfileSerializer.Serialize);

                    if (!string.IsNullOrWhiteSpace(layer.GeomTransform))
                    {
                        this.Prop("GEOMTRANSFORM", quote: false, default, $"{this.SerializeExpression(layer.GeomTransform)}");
                    }

                    if (layer.Grid is not null)
                    {
                        this.Grid(layer.Grid);
                    }

                    if (layer.Identify is not null)
                    {
                        this.Identify(layer.Identify);
                    }

                    if (layer.Leader is not null)
                    {
                        this.Leader(layer.Leader);
                    }

                    foreach (var j in layer.Joins)
                    {
                        this.Join(j);
                    }

                    if (layer.Composite is not null)
                    {
                        this.Composite(layer.Composite);
                    }

                    foreach (var c in layer.Classes)
                    {
                        this.Class(c);
                    }
                });

        public void Class(Class c) =>
            this.Block(
                "CLASS",
                () =>
                {
                    this.PropQuoted("NAME", c.Name ?? string.Empty);
                    this.PropQuoted("GROUP", c.Group);
                    if (!string.IsNullOrWhiteSpace(c.Expression))
                    {
                        this.Prop("EXPRESSION", quote: false, default, $"{this.SerializeExpression(c.Expression)}");
                    }

                    this.Prop("MINSCALEDENOM", c.MinScaleDenom);
                    this.Prop("MAXSCALEDENOM", c.MaxScaleDenom);
                    this.Prop("MINGEOWIDTH", c.MinGeoWidth);
                    this.Prop("MAXGEOWIDTH", c.MaxGeoWidth);
                    if (c.Fallback is true)
                    {
                        this.Prop("FALLBACK", "TRUE");
                    }

                    this.Prop("DEBUG", c.DebugLevel);

                    foreach (var s in c.Styles)
                    {
                        this.Style(s);
                    }

                    foreach (var l in c.Labels)
                    {
                        this.Label(l);
                    }
                });

        public void Label(Label label) =>
            this.Block(
                "LABEL",
                () =>
                {
                    this.Prop("SIZE", label.Size);
                    this.Prop("OFFSET", label.Offset);
                    this.Prop("SHADOWSIZE", label.ShadowSize);
                    this.Prop("TYPE", label.Type);
                    this.Prop("COLOR", label.Color);
                    this.Prop("OUTLINECOLOR", label.OutlineColor);
                    this.PropQuoted("FONT", label.Font);
                    this.PropQuoted("ANGLE", label.Angle);
                    this.Prop("ALIGN", label.Align, Serialize);
                    this.Prop("POSITION", label.Position, Serialize);
                    this.Prop("PARTIALS", label.Partials);
                    this.Prop("FORCE", label.Force);
                    this.Prop("BUFFER", label.Buffer);
                    this.Prop("MINFEATURESIZE", label.MinFeatureSize);
                    this.Prop("MINDISTANCE", label.MinDistance);
                    this.PropQuoted("WRAP", label.Wrap);

                    foreach (var s in label.Styles)
                    {
                        this.Style(s);
                    }
                });

        public override string ToString() => this.builder.ToString();

        private static string DoubleQuote(string s) => $"\"{s.Replace("\\", "\\\\", StringComparison.Ordinal).Replace("\"", "\\\"", StringComparison.Ordinal)}\"";

        private static string SingleQuote(string s) => $"\'{s.Replace("\'", "\\\'", StringComparison.Ordinal)}\'";

#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members
        private static string Serialize<T>(T v)
            where T : Enum => v.ToString().ToUpperInvariant();

        private static string Serialize(System.Drawing.Point pt) => FormattableString.Invariant($"{pt.X} {pt.Y}");

        private static string Serialize(System.Drawing.Size s) => FormattableString.Invariant($"{s.Width} {s.Height}");

        private static string? Serialize(Angle a) =>
            a.Match<string?>(
                n => n.ToString(System.Globalization.CultureInfo.InvariantCulture),
                a => a.ToString(),
                _ => "AUTO",
                _ => default);

        private string? Serialize(ColorOrAttribute c) =>
            c.Match<string?>(
                color => MapfileSerializer.Serialize(color, this.options),
                attribute => attribute.ToString(),
                _ => default);
#pragma warning restore S3218 // Inner class members should not shadow outer class "static" or type members

        private void Block(string name, Action body)
        {
            this.Line(name);
            this.WithIndent(body);
            this.Line(default, $"END # {name}");
        }

        private void Style(Style s) =>
            this.Block(
                "STYLE",
                () =>
                {
                    this.Prop("COLOR", s.Color, this.Serialize);
                    this.Prop("OUTLINECOLOR", s.OutlineColor, this.Serialize);
                    this.PropQuoted("SYMBOL", s.Symbol);
                    this.Prop("WIDTH", s.Width);
                    this.Prop("SIZE", s.Size);
                    this.Prop("ANGLE", s.Angle, Serialize);

                    if (s.Pattern.Count > 0)
                    {
                        this.Prop("PATTERN", quote: false, default, $"{string.Join(' ', s.Pattern.Select(p => p.ToString(CultureInfo.InvariantCulture)))}");
                    }

                    this.Prop("GAP", s.Gap);
                    this.Prop("OFFSET", s.Offset);
                    this.Prop("OPACITY", s.Opacity);

                    if (!string.IsNullOrWhiteSpace(s.GeomTransform))
                    {
                        this.Prop("GEOMTRANSFORM", quote: false, default, $"{this.SerializeExpression(s.GeomTransform)}");
                    }

                    this.Prop("MINSCALEDENOM", s.MinScaleDenom);

                    this.Prop("MAXSCALEDENOM", s.MaxScaleDenom);
                });

        private string SerializeExpression(string expr)
            => this.options.QuoteExpressions ? DoubleQuote(expr) : expr;

        private void Composite(Composite c) =>
            this.Block(
                "COMPOSITE",
                () =>
                {
                    this.PropQuoted("COMPOP", c.CompOp);

                    foreach (var f in c.CompFilters)
                    {
                        this.Prop("COMPFILTER", quote: false, default, $"{this.SerializeExpression(f)}");
                    }

                    this.Prop("OPACITY", c.Opacity);
                });

        private void Join(Join j) =>
            this.Block(
                "JOIN",
                () =>
                {
                    this.PropQuoted("NAME", j.Name);
                    this.PropQuoted("TABLE", j.Table);
                    this.PropQuoted("FROM", j.From);
                    this.PropQuoted("TO", j.To);
                    this.PropQuoted("TEMPLATE", j.Template);
                    this.PropQuoted("HEADER", j.Header);
                    this.PropQuoted("FOOTER", j.Footer);
                    this.PropQuoted("CONNECTIONTYPE", j.ConnectionType);
                    this.PropQuoted("CONNECTION", j.Connection);
                    this.PropQuoted("TYPE", j.OneToMany ? "ONE-TO-MANY" : "ONE-TO-ONE");
                });

        private void Identify(Identify id) =>
            this.Block(
                "IDENTIFY",
                () =>
                {
                    this.Prop("TOLERANCE", id.Tolerance);
                    this.Prop("TOLERANCEUNITS", id.ToleranceUnits, MapfileSerializer.Serialize);
                    if (id.ClassAuto is true)
                    {
                        this.Line("CLASSAUTO");
                    }

                    this.PropQuoted("CLASSGROUP", id.ClassGroup);
                });

        private void Leader(Leader l) =>
            this.Block(
                "LEADER",
                () =>
                {
                    if (l.GridStep is not null)
                    {
                        this.Prop("GRIDSTEP", l.GridStep);
                    }

                    if (l.MaxDistance is not null)
                    {
                        this.Prop("MAXDISTANCE", l.MaxDistance);
                    }

                    if (l.Style is not null)
                    {
                        this.Style(l.Style);
                    }
                });

        private void Grid(Grid g) =>
            this.Block(
                "GRID",
                () =>
                {
                    if (g.LabelFormat == GridLabelFormat.DD)
                    {
                        this.Prop("LABELFORMAT", "DD");
                    }
                    else if (g.LabelFormat == GridLabelFormat.DDMM)
                    {
                        this.Prop("LABELFORMAT", "DDMM");
                    }
                    else if (g.LabelFormat == GridLabelFormat.DDMMSS)
                    {
                        this.Prop("LABELFORMAT", "DDMMSS");
                    }
                    else if (!string.IsNullOrWhiteSpace(g.LabelFormatCustom))
                    {
                        this.PropQuoted("LABELFORMAT", g.LabelFormatCustom);
                    }

                    if (g.MinArcs is not null)
                    {
                        this.Prop("MINARCS", g.MinArcs);
                    }

                    if (g.MaxArcs is not null)
                    {
                        this.Prop("MAXARCS", g.MaxArcs);
                    }

                    if (g.MinInterval is not null)
                    {
                        this.Prop("MININTERVAL", g.MinInterval);
                    }

                    if (g.MaxInterval is not null)
                    {
                        this.Prop("MAXINTERVAL", g.MaxInterval);
                    }

                    if (g.MinSubdivide is not null)
                    {
                        this.Prop("MINSUBDIVIDE", g.MinSubdivide);
                    }

                    if (g.MaxSubdivide is not null)
                    {
                        this.Prop("MAXSUBDIVIDE", g.MaxSubdivide);
                    }
                });

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Runtime.CompilerServices.InterpolatedStringHandler]
        public struct IndentInterpolatedStringHandler
        {
            private readonly Writer writer;
            private StringBuilder.AppendInterpolatedStringHandler builder;

#pragma warning disable S1144, S3427
            public IndentInterpolatedStringHandler(int literalLength, int formattedCount, Writer writer, IFormatProvider? provider)
            {
                this.writer = writer;
                this.Indent();
                this.builder = new(literalLength, formattedCount, this.writer.builder, provider);
            }

            public IndentInterpolatedStringHandler(int literalLength, int formattedCount, Writer writer, IFormatProvider? provider, string? name)
            {
                this.writer = writer;
                this.Indent();

                // write the name
                this.writer.builder.Append(name);
                this.writer.builder.Append(' ');

                this.builder = new(literalLength, formattedCount, this.writer.builder, provider);
            }

            public IndentInterpolatedStringHandler(int literalLength, int formattedCount, Writer writer, IFormatProvider? provider, string? name, bool quoted)
            {
                this.writer = writer;
                this.Indent();

                // write the name
                this.writer.builder.Append(name);
                this.writer.builder.Append(' ');
                if (quoted)
                {
                    this.writer.builder.Append('\"');
                }

                this.builder = new(literalLength, formattedCount, this.writer.builder, provider);
            }

            public readonly void AppendLiteral(string s) => this.builder.AppendLiteral(s);

            public readonly void AppendFormatted<T>(T t)
            {
                // any multi-line string should be indented appropriately
                if (t is string { } ts && ts.Contains('\n', StringComparison.Ordinal))
                {
                    // count back to the last new line
                    var newLine = this.writer.options.NewLine;
                    var stringBuilder = this.writer.builder;
                    var start = 0;
                    for (var i = stringBuilder.Length - newLine.Length; i >= 0; i--)
                    {
                        if (IsAtNewLine(stringBuilder, i, newLine))
                        {
                            start = i + newLine.Length;
                            break;
                        }
                    }

                    var extraIndent = stringBuilder.Length - start + 1;

                    var split = ts.Split('\n');

                    this.builder.AppendFormatted(split[0].TrimEnd('\r'));
                    for (var i = 1; i < split.Length; i++)
                    {
                        this.builder.AppendLiteral(newLine);
                        for (var j = 0; j < extraIndent; j++)
                        {
                            this.builder.AppendLiteral(" ");
                        }

                        this.builder.AppendFormatted(split[i].TrimEnd('\r'));
                    }

                    return;

                    static bool IsAtNewLine(StringBuilder stringBuilder, int index, string newLine)
                    {
                        for (int i = 0; i < newLine.Length; i++)
                        {
                            if (stringBuilder[index + i] != newLine[i])
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                }

                this.builder.AppendFormatted(t);
            }

            public void AppendFormatted<T>(T value, string? format) => this.builder.AppendFormatted(value, format);

            public void AppendFormatted<T>(T value, int alignment) => this.builder.AppendFormatted(value, alignment);

            public void AppendFormatted<T>(T value, int alignment, string? format) => this.builder.AppendFormatted(value, alignment, format);

            public void AppendFormatted(ReadOnlySpan<char> value) => this.builder.AppendFormatted(value);

            public void AppendFormatted(ReadOnlySpan<char> value, int alignment = 0, string? format = null) => this.builder.AppendFormatted(value, alignment, format);

            public void AppendFormatted(string? value, int alignment = 0, string? format = null) => this.builder.AppendFormatted(value, alignment, format);

            public void AppendFormatted(object? value, int alignment = 0, string? format = null) => this.builder.AppendFormatted(value, alignment, format);
#pragma warning restore S1144, S3427

            private readonly void Indent()
            {
                for (var i = 0; i < this.writer.indent; i++)
                {
                    this.writer.builder.Append(this.writer.options.Indent);
                }
            }
        }
    }
}