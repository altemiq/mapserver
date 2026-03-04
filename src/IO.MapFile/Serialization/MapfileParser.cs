// -----------------------------------------------------------------------
// <copyright file="MapfileParser.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Serialization;

using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Altemiq.IO.MapFile.Types;

/// <summary>
/// Parses MapServer mapfiles into the strongly-typed model.
/// </summary>
/// <param name="text">The input text.</param>
public ref struct MapfileParser(ReadOnlySpan<char> text)
{
    private static readonly IReadOnlySet<string> DefaultStopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "END" };

    private static readonly IReadOnlySet<string> LayerGeotransformStopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "COLOR", "OUTLINECOLOR", "SYMBOL", "WIDTH", "SIZE", "ANGLE", "PATTERN", "GAP", "OFFSET", "OPACITY", "MINSCALEDENOM", "MAXSCALEDENOM", "END",
    };

    private static readonly IReadOnlySet<string> StyleGeotransformStopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "COLOR", "OUTLINECOLOR", "SYMBOL", "WIDTH", "SIZE", "ANGLE", "PATTERN", "GAP", "OFFSET", "OPACITY", "MINSCALEDENOM", "MAXSCALEDENOM", "END",
    };

    private static readonly IReadOnlySet<string> CompositeCompFilterStopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "COMPFILTER", "COMPOP", "OPACITY", "END",
    };

    private static readonly IReadOnlySet<string> ClassExpressionStopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "STYLE", "LABEL", "MINSCALEDENOM", "MAXSCALEDENOM", "MINGEOWIDTH", "MAXGEOWIDTH", "FALLBACK", "DEBUG", "END",
    };

    private TokenReader tokenReader = new(new MapfileTokenizer(text));

    /// <summary>
    /// Parses a map instance.
    /// </summary>
    /// <returns>The map instance.</returns>
    public Map ParseMap()
    {
        // Expect MAP ... END
        this.tokenReader.ExpectKeyword("MAP");
        var map = new Map();

        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("NAME"))
            {
                map.Name = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("STATUS"))
            {
                map.Status = this.ReadStatus();
            }
            else if (this.tokenReader.TryAcceptKeyword("SIZE"))
            {
                map.Size = this.ReadSize();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXSIZE"))
            {
                map.MaxSize = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("EXTENT"))
            {
                map.Extent = this.ReadExtent();
            }
            else if (this.tokenReader.TryAcceptKeyword("UNITS"))
            {
                map.Units = this.ReadUnits();
            }
            else if (this.tokenReader.TryAcceptKeyword("IMAGECOLOR"))
            {
                map.ImageColor = this.ReadColor();
            }
            else if (this.tokenReader.TryAcceptKeyword("IMAGETYPE"))
            {
                map.ImageType = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("SYMBOLSET"))
            {
                map.SymbolSet = this.tokenReader.ReadString();
            }
            else if (this.tokenReader.TryAcceptKeyword("FONTSET"))
            {
                map.FontSet = this.tokenReader.ReadString();
            }
            else if (this.tokenReader.TryAcceptKeyword("SHAPEPATH"))
            {
                map.ShapePath = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("DEBUG"))
            {
                map.DebugLevel = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("CONFIG"))
            {
                var key = this.tokenReader.ReadStringOrIdentifier();
                map.Config[key] = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("METADATA"))
            {
                this.ReadDictionary(map.Metadata);
            }
            else if (this.tokenReader.TryAcceptKeyword("PROJECTION"))
            {
                map.Projection = this.ReadProjection();
            }
            else if (this.tokenReader.TryAcceptKeyword("WEB"))
            {
                map.Web = this.ReadWeb();
            }
            else if (this.tokenReader.TryAcceptKeyword("OUTPUTFORMAT"))
            {
                map.OutputFormats.Add(this.ReadOutputFormat());
            }
            else if (this.tokenReader.TryAcceptKeyword("REFERENCE"))
            {
                map.Reference = this.ReadReference();
            }
            else if (this.tokenReader.TryAcceptKeyword("LEGEND"))
            {
                map.Legend = this.ReadLegend();
            }
            else if (this.tokenReader.TryAcceptKeyword("SCALEBAR"))
            {
                map.ScaleBar = this.ReadScaleBar();
            }
            else if (this.tokenReader.TryAcceptKeyword("QUERYMAP"))
            {
                map.QueryMap = this.ReadQueryMap();
            }
            else if (this.tokenReader.TryAcceptKeyword("LAYER"))
            {
                map.Layers.Add(this.ReadLayer());
            }
            else
            {
                // Unknown keyword at MAP level -> skip value/expression or nested block defensively
                this.SkipUnknownMapLevelEntry();
            }
        }

        return map;
    }

    private Web ReadWeb()
    {
        var web = new Web();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("BROWSEFORMAT"))
            {
                web.BrowseFormat = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("LEGENDFORMAT"))
            {
                web.LegendFormat = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("EMPTY"))
            {
                web.EmptyUrl = this.tokenReader.ReadString();
            }
            else if (this.tokenReader.TryAcceptKeyword("ERROR"))
            {
                web.ErrorUrl = this.tokenReader.ReadString();
            }
            else if (this.tokenReader.TryAcceptKeyword("HEADER"))
            {
                web.HeaderTemplate = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("FOOTER"))
            {
                web.FooterTemplate = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("IMAGEPATH"))
            {
                web.ImagePath = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("IMAGEURL"))
            {
                web.ImageUrl = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXSCALEDENOM"))
            {
                web.MaxScaleDenom = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXTEMPLATE"))
            {
                web.MaxTemplate = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("METADATA"))
            {
                this.ReadDictionary(web.Metadata);
            }
            else if (this.tokenReader.TryAcceptKeyword("VALIDATION"))
            {
                this.ReadDictionary(web.Metadata);
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return web;
    }

    private OutputFormat ReadOutputFormat()
    {
        var of = new OutputFormat();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("NAME"))
            {
                of.Name = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("DRIVER"))
            {
                of.Driver = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("MIMETYPE"))
            {
                of.MimeType = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("EXTENSION"))
            {
                of.Extension = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("IMAGEMODE"))
            {
                of.ImageMode = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("TRANSPARENT"))
            {
                of.Transparent = this.tokenReader.ReadBoolean();
            }
            else if (this.tokenReader.TryAcceptKeyword("FORMATOPTION"))
            {
                // Accept "KEY=VALUE" or ("KEY" "VALUE")
                var token = this.tokenReader.Peek();
                if (token.Type is TokenType.String && token.Lexeme.IndexOf('=') >= 0)
                {
                    var kv = this.tokenReader.ReadString();
                    var i = kv.IndexOf('=');
                    if (i > 0)
                    {
                        of.FormatOptions[kv.Substring(0, i)] = kv[(i + 1)..];
                    }
                }
                else
                {
                    var k = this.tokenReader.ReadStringOrIdentifier();
                    var v = this.tokenReader.ReadStringOrIdentifier();
                    of.FormatOptions[k] = v;
                }
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return of;
    }

    private Projection ReadProjection()
    {
        var projection = new Projection();
        if (this.tokenReader.TryAcceptKeyword("AUTO"))
        {
            projection.Auto = true;
            this.tokenReader.ExpectKeyword("END");
            return projection;
        }

        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            // PROJECTION parameters are generally quoted lines; accept identifiers too.
            var p = this.tokenReader.ReadStringOrIdentifier();
            projection.Parameters.Add(p);
        }

        return projection;
    }

    private Reference ReadReference()
    {
        var rf = new Reference();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("COLOR"))
            {
                rf.Color = this.ReadColor();
            }
            else if (this.tokenReader.TryAcceptKeyword("OUTLINECOLOR"))
            {
                rf.OutlineColor = this.ReadColor();
            }
            else if (this.tokenReader.TryAcceptKeyword("EXTENT"))
            {
                rf.Extent = this.ReadExtent();
            }
            else if (this.tokenReader.TryAcceptKeyword("IMAGE"))
            {
                rf.Image = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("SIZE"))
            {
                rf.Size = this.ReadSize();
            }
            else if (this.tokenReader.TryAcceptKeyword("MARKER"))
            {
                rf.Marker = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("MARKERSIZE"))
            {
                rf.MarkerSize = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MINBOXSIZE"))
            {
                rf.MinBoxSize = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXBOXSIZE"))
            {
                rf.MaxBoxSize = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("STATUS"))
            {
                rf.Status = this.ReadStatus();
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return rf;
    }

    private Legend ReadLegend()
    {
        var lg = new Legend();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("STATUS"))
            {
                lg.Status = this.ReadStatus();
            }
            else if (this.tokenReader.TryAcceptKeyword("IMAGECOLOR"))
            {
                lg.ImageColor = this.ReadColor();
            }
            else if (this.tokenReader.TryAcceptKeyword("OUTLINECOLOR"))
            {
                lg.OutlineColor = this.ReadColor();
            }
            else if (this.tokenReader.TryAcceptKeyword("KEYSIZE"))
            {
                lg.KeySize = this.ReadSize();
            }
            else if (this.tokenReader.TryAcceptKeyword("KEYSPACING"))
            {
                lg.KeySpacing = this.ReadSize();
            }
            else if (this.tokenReader.TryAcceptKeyword("POSITION"))
            {
                lg.Position = this.ReadCornerPosition();
            }
            else if (this.tokenReader.TryAcceptKeyword("POSTLABELCACHE"))
            {
                lg.PostLabelCache = this.tokenReader.ReadBoolean();
            }
            else if (this.tokenReader.TryAcceptKeyword("TEMPLATE"))
            {
                lg.Template = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("TRANSPARENT"))
            {
                lg.Transparent = this.tokenReader.ReadBoolean();
            }
            else if (this.tokenReader.TryAcceptKeyword("LABEL"))
            {
                lg.Label = this.ReadLabel();
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return lg;
    }

    private ScaleBar ReadScaleBar()
    {
        var sb = new ScaleBar();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("STATUS"))
            {
                sb.Status = this.ReadStatus();
            }
            else if (this.tokenReader.TryAcceptKeyword("IMAGECOLOR"))
            {
                sb.ImageColor = this.ReadColor();
            }
            else if (this.tokenReader.TryAcceptKeyword("BACKGROUNDCOLOR"))
            {
                sb.BackColor = this.ReadColor();
            }
            else if (this.tokenReader.TryAcceptKeyword("COLOR"))
            {
                sb.Color = this.ReadColor();
            }
            else if (this.tokenReader.TryAcceptKeyword("OUTLINECOLOR"))
            {
                sb.OutlineColor = this.ReadColor();
            }
            else if (this.tokenReader.TryAcceptKeyword("SIZE"))
            {
                sb.Size = this.ReadSize();
            }
            else if (this.tokenReader.TryAcceptKeyword("POSITION"))
            {
                sb.Position = this.ReadCornerPosition();
            }
            else if (this.tokenReader.TryAcceptKeyword("ALIGN"))
            {
                sb.Align = this.ReadAlign();
            }
            else if (this.tokenReader.TryAcceptKeyword("INTERVALS"))
            {
                sb.Intervals = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("OFFSET"))
            {
                sb.Offset = this.ReadPoint();
            }
            else if (this.tokenReader.TryAcceptKeyword("UNITS"))
            {
                sb.Units = this.ReadUnits();
            }
            else if (this.tokenReader.TryAcceptKeyword("TRANSPARENT"))
            {
                sb.Transparent = this.tokenReader.ReadBoolean();
            }
            else if (this.tokenReader.TryAcceptKeyword("LABEL"))
            {
                sb.Label = this.ReadLabel();
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return sb;
    }

    private QueryMap ReadQueryMap()
    {
        var qm = new QueryMap();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("STATUS"))
            {
                qm.Status = this.ReadStatus();
            }
            else if (this.tokenReader.TryAcceptKeyword("COLOR"))
            {
                qm.Color = this.ReadColor();
            }
            else if (this.tokenReader.TryAcceptKeyword("SIZE"))
            {
                qm.Size = this.ReadSize();
            }
            else if (this.tokenReader.TryAcceptKeyword("STYLE"))
            {
                qm.Style = this.tokenReader.ReadStringOrIdentifier().ToLowerInvariant() switch
                {
                    "selected" => QueryMapStyle.Selected,
                    "normal" => QueryMapStyle.Normal,
                    _ => QueryMapStyle.Hilite,
                };
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return qm;
    }

    private Layer ReadLayer()
    {
        var ly = new Layer();

        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("NAME"))
            {
                ly.Name = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("TYPE"))
            {
                ly.Type = this.ReadLayerType();
            }
            else if (this.tokenReader.TryAcceptKeyword("STATUS"))
            {
                ly.Status = this.ReadStatus();
            }
            else if (this.tokenReader.TryAcceptKeyword("GROUP"))
            {
                ly.Group = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("CLASSGROUP"))
            {
                ly.ClassGroup = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("DATA"))
            {
                ly.Data = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("CONNECTION"))
            {
                ly.Connection = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("CONNECTIONTYPE"))
            {
                ly.ConnectionType = this.ReadConnectionType();
            }
            else if (this.tokenReader.TryAcceptKeyword("CONNECTIONOPTIONS"))
            {
                this.ReadDictionary(ly.ConnectionOptions);
            }
            else if (this.tokenReader.TryAcceptKeyword("BINDVALS"))
            {
                this.ReadDictionary(ly.BindVals);
            }
            else if (this.tokenReader.TryAcceptKeyword("CLASSITEM"))
            {
                ly.ClassItem = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("FILTER"))
            {
                ly.Filter = this.ReadExpressionOrString(DefaultStopWords);
            }
            else if (this.tokenReader.TryAcceptKeyword("FILTERITEM"))
            {
                ly.FilterItem = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("LABELITEM"))
            {
                ly.LabelItem = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("LABELREQUIRES"))
            {
                ly.LabelRequires = this.ReadExpressionOrString(DefaultStopWords);
            }
            else if (this.tokenReader.TryAcceptKeyword("LABELMINSCALEDENOM"))
            {
                ly.LabelMinScaleDenom = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("LABELMAXSCALEDENOM"))
            {
                ly.LabelMaxScaleDenom = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MINSCALEDENOM"))
            {
                ly.MinScaleDenom = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXSCALEDENOM"))
            {
                ly.MaxScaleDenom = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MINGEOWIDTH"))
            {
                ly.MinGeoWidth = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXGEOWIDTH"))
            {
                ly.MaxGeoWidth = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MINFEATURESIZE"))
            {
                ly.MinFeatureSize = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXFEATURES"))
            {
                ly.MaxFeatures = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("HEADER"))
            {
                ly.Header = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("FOOTER"))
            {
                ly.Footer = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("TEMPLATE"))
            {
                ly.Template = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("REQUIRES"))
            {
                ly.Requires = this.ReadExpressionOrString(DefaultStopWords);
            }
            else if (this.tokenReader.TryAcceptKeyword("MASK"))
            {
                ly.Mask = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("UNITS"))
            {
                ly.Units = this.ReadUnits();
            }
            else if (this.tokenReader.TryAcceptKeyword("SIZEUNITS"))
            {
                ly.SizeUnits = this.ReadUnits();
            }
            else if (this.tokenReader.TryAcceptKeyword("PROCESSING"))
            {
                ly.Processing.Add(this.tokenReader.ReadStringOrIdentifier());
            }
            else if (this.tokenReader.TryAcceptKeyword("METADATA"))
            {
                this.ReadDictionary(ly.Metadata);
            }
            else if (this.tokenReader.TryAcceptKeyword("VALIDATION"))
            {
                this.ReadDictionary(ly.Validation);
            }
            else if (this.tokenReader.TryAcceptKeyword("PROJECTION"))
            {
                ly.Projection = this.ReadProjection();
            }
            else if (this.tokenReader.TryAcceptKeyword("EXTENT"))
            {
                ly.Extent = this.ReadExtent();
            }
            else if (this.tokenReader.TryAcceptKeyword("GEOMTRANSFORM"))
            {
                ly.GeomTransform = this.ReadExpressionOrString(LayerGeotransformStopWords);
            }
            else if (this.tokenReader.TryAcceptKeyword("GRID"))
            {
                ly.Grid = this.ReadGrid();
            }
            else if (this.tokenReader.TryAcceptKeyword("IDENTIFY"))
            {
                ly.Identify = this.ReadIdentify();
            }
            else if (this.tokenReader.TryAcceptKeyword("LEADER"))
            {
                ly.Leader = this.ReadLeader();
            }
            else if (this.tokenReader.TryAcceptKeyword("JOIN"))
            {
                ly.Joins.Add(this.ReadJoin());
            }
            else if (this.tokenReader.TryAcceptKeyword("COMPOSITE"))
            {
                ly.Composite = this.ReadComposite();
            }
            else if (this.tokenReader.TryAcceptKeyword("CLASS"))
            {
                ly.Classes.Add(this.ReadClass());
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return ly;
    }

    private Composite ReadComposite()
    {
        var c = new Composite();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("COMPOP"))
            {
                c.CompOp = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("COMPFILTER"))
            {
                // Could be something like blur(4) translate(2,2) etc.
                // Accept as either a quoted string or accumulate tokens until next keyword/END.
                var expr = this.ReadExpressionOrString(CompositeCompFilterStopWords);
                if (!string.IsNullOrWhiteSpace(expr))
                {
                    foreach (var filter in SplitFilters(expr))
                    {
                        c.CompFilters.Add(filter);
                    }
                }
            }
            else if (this.tokenReader.TryAcceptKeyword("OPACITY"))
            {
                c.Opacity = (int)this.tokenReader.ReadNumber();
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return c;

        static IEnumerable<string> SplitFilters(string s)
        {
            // naive split: allow multiple filters separated by spaces; keep function calls intact
            // e.g., "blur(4) translate(2,2)" -> ["blur(4)","translate(2,2)"]
            var list = new List<string>();
            var sb = new StringBuilder();
            int paren = 0;
            foreach (var ch in s)
            {
                if (ch is '(')
                {
                    paren++;
                }

                if (ch is ')')
                {
                    paren = Math.Max(0, paren - 1);
                }

                if (char.IsWhiteSpace(ch) && paren == 0)
                {
                    if (sb.Length > 0)
                    {
                        list.Add(sb.ToString());
                        sb.Clear();
                    }
                }
                else
                {
                    sb.Append(ch);
                }
            }

            if (sb.Length > 0)
            {
                list.Add(sb.ToString());
            }

            return list;
        }
    }

    private Join ReadJoin()
    {
        var j = new Join();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("NAME"))
            {
                j.Name = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("TABLE"))
            {
                j.Table = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("FROM"))
            {
                j.From = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("TO"))
            {
                j.To = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("TEMPLATE"))
            {
                j.Template = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("HEADER"))
            {
                j.Header = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("FOOTER"))
            {
                j.Footer = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("CONNECTIONTYPE"))
            {
                j.ConnectionType = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("CONNECTION"))
            {
                j.Connection = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("TYPE"))
            {
                var t = this.tokenReader.ReadStringOrIdentifier();
                j.OneToMany = t.Equals("ONE-TO-MANY", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return j;
    }

    private Identify ReadIdentify()
    {
        var id = new Identify();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("TOLERANCE"))
            {
                id.Tolerance = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("TOLERANCEUNITS"))
            {
                id.ToleranceUnits = this.ReadUnits() ?? MapUnits.Pixels;
            }
            else if (this.tokenReader.TryAcceptKeyword("CLASSAUTO"))
            {
                id.ClassAuto = true;
            }
            else if (this.tokenReader.TryAcceptKeyword("CLASSGROUP"))
            {
                id.ClassGroup = this.tokenReader.ReadStringOrIdentifier();
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return id;
    }

    private Leader ReadLeader()
    {
        var leader = new Leader();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("GRIDSTEP"))
            {
                leader.GridStep = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXDISTANCE"))
            {
                leader.MaxDistance = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("STYLE"))
            {
                leader.Style = this.ReadStyle();
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return leader;
    }

    private Grid ReadGrid()
    {
        var grid = new Grid();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("LABELFORMAT"))
            {
                var v = this.tokenReader.ReadStringOrIdentifier();
                grid.LabelFormat = v.ToUpperInvariant() switch
                {
                    "DD" => GridLabelFormat.DD,
                    "DDMM" => GridLabelFormat.DDMM,
                    "DDMMSS" => GridLabelFormat.DDMMSS,
                    _ => GridLabelFormat.Custom,
                };
                if (grid.LabelFormat == GridLabelFormat.Custom)
                {
                    grid.LabelFormatCustom = v;
                }
            }
            else if (this.tokenReader.TryAcceptKeyword("MINARCS"))
            {
                grid.MinArcs = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXARCS"))
            {
                grid.MaxArcs = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MININTERVAL"))
            {
                grid.MinInterval = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXINTERVAL"))
            {
                grid.MaxInterval = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MINSUBDIVIDE"))
            {
                grid.MinSubdivide = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXSUBDIVIDE"))
            {
                grid.MaxSubdivide = this.tokenReader.ReadNumber();
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return grid;
    }

    private Class ReadClass()
    {
        var cls = new Class();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("NAME"))
            {
                cls.Name = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("GROUP"))
            {
                cls.Group = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("EXPRESSION"))
            {
                cls.Expression = this.ReadExpressionOrString(ClassExpressionStopWords);
            }
            else if (this.tokenReader.TryAcceptKeyword("MINSCALEDENOM"))
            {
                cls.MinScaleDenom = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXSCALEDENOM"))
            {
                cls.MaxScaleDenom = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MINGEOWIDTH"))
            {
                cls.MinGeoWidth = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXGEOWIDTH"))
            {
                cls.MaxGeoWidth = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("FALLBACK"))
            {
                cls.Fallback = this.tokenReader.ReadBoolean();
            }
            else if (this.tokenReader.TryAcceptKeyword("DEBUG"))
            {
                cls.DebugLevel = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("STYLE"))
            {
                cls.Styles.Add(this.ReadStyle());
            }
            else if (this.tokenReader.TryAcceptKeyword("LABEL"))
            {
                cls.Labels.Add(this.ReadLabel());
            }
            else if (this.tokenReader.TryAcceptKeyword("VALIDATION"))
            {
                this.ReadDictionary(cls.Validation);
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return cls;
    }

    private Style ReadStyle()
    {
        var s = new Style();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("COLOR"))
            {
                s.Color = this.ReadColorOrAttribute();
            }
            else if (this.tokenReader.TryAcceptKeyword("OUTLINECOLOR"))
            {
                s.OutlineColor = this.ReadColorOrAttribute();
            }
            else if (this.tokenReader.TryAcceptKeyword("SYMBOL"))
            {
                s.Symbol = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("WIDTH"))
            {
                s.Width = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("SIZE"))
            {
                s.Size = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("ANGLE"))
            {
                var angle = this.tokenReader.ReadStringOrIdentifierOrNumber();
                if (angle.TryGetAsT1(out var d))
                {
                    s.Angle = d;
                }
                else if (angle.TryGetAsT0(out var st))
                {
                    s.Angle = st is "AUTO" ? Auto.Instance : new Attribute(st);
                }
            }
            else if (this.tokenReader.TryAcceptKeyword("PATTERN"))
            {
                foreach (var number in this.ReadNumberList())
                {
                    s.Pattern.Add(number);
                }
            }
            else if (this.tokenReader.TryAcceptKeyword("GAP"))
            {
                s.Gap = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("OFFSET"))
            {
                s.Offset = this.ReadPoint();
            }
            else if (this.tokenReader.TryAcceptKeyword("OPACITY"))
            {
                s.Opacity = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("GEOMTRANSFORM"))
            {
                s.GeomTransform = this.ReadExpressionOrString(StyleGeotransformStopWords);
            }
            else if (this.tokenReader.TryAcceptKeyword("MINSCALEDENOM"))
            {
                s.MinScaleDenom = this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MAXSCALEDENOM"))
            {
                s.MaxScaleDenom = this.tokenReader.ReadNumber();
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return s;
    }

    private Label ReadLabel()
    {
        var l = new Label();
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            if (this.tokenReader.TryAcceptKeyword("COLOR"))
            {
                l.Color = this.ReadColor();
            }
            else if (this.tokenReader.TryAcceptKeyword("OUTLINECOLOR"))
            {
                l.OutlineColor = this.ReadColor();
            }
            else if (this.tokenReader.TryAcceptKeyword("FONT"))
            {
                l.Font = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("TYPE"))
            {
                l.Type = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("SIZE"))
            {
                l.Size = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("ANGLE"))
            {
                l.Angle = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("ALIGN"))
            {
                l.Align = this.ReadAlign();
            }
            else if (this.tokenReader.TryAcceptKeyword("POSITION"))
            {
                l.Position = this.ReadLabelPosition();
            }
            else if (this.tokenReader.TryAcceptKeyword("PARTIALS"))
            {
                l.Partials = this.tokenReader.ReadBoolean();
            }
            else if (this.tokenReader.TryAcceptKeyword("FORCE"))
            {
                l.Force = this.tokenReader.ReadBoolean();
            }
            else if (this.tokenReader.TryAcceptKeyword("BUFFER"))
            {
                l.Buffer = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MINFEATURESIZE"))
            {
                l.MinFeatureSize = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("MINDISTANCE"))
            {
                l.MinDistance = (int)this.tokenReader.ReadNumber();
            }
            else if (this.tokenReader.TryAcceptKeyword("WRAP"))
            {
                l.Wrap = this.tokenReader.ReadStringOrIdentifier();
            }
            else if (this.tokenReader.TryAcceptKeyword("OFFSET"))
            {
                l.Offset = this.ReadPoint();
            }
            else if (this.tokenReader.TryAcceptKeyword("SHADOWSIZE"))
            {
                l.ShadowSize = this.ReadSize();
            }
            else if (this.tokenReader.TryAcceptKeyword("STYLE"))
            {
                l.Styles.Add(this.ReadStyle());
            }
            else
            {
                this.tokenReader.SkipLineOrUnknown();
            }
        }

        return l;
    }

    private void ReadDictionary(IDictionary<string, string> target)
    {
        // ?? ... END  -> expect pairs of "key" "value"
        while (!this.tokenReader.TryAcceptKeyword("END"))
        {
            var key = this.tokenReader.ReadStringOrIdentifier();
            target[key] = this.tokenReader.ReadStringOrIdentifier();
        }
    }

    private void SkipUnknownMapLevelEntry()
    {
        // If we see an unknown token, try to skip a potential value or nested block safely.
        if (this.tokenReader.TryAccept(TokenType.String) || this.tokenReader.TryAccept(TokenType.Number) || this.tokenReader.TryAccept(TokenType.Identifier))
        {
            return;
        }

        if (this.tokenReader.TryAcceptKeyword("{") || this.tokenReader.TryAcceptKeyword("("))
        {
            // naive skip until END or close token
            while (!this.tokenReader.IsEof && !this.tokenReader.TryAcceptKeyword("END"))
            {
                this.tokenReader.ReadAny();
            }
        }
        else
        {
            this.tokenReader.ReadAny(); // consume one token to avoid infinite loop
        }
    }

    private static System.Drawing.Color ParseHexColor(string hex)
    {
        // #RRGGBB or #RRGGBBAA
        hex = hex.Trim();
        if (hex.StartsWith('#'))
        {
            hex = hex[1..];
        }

        if (hex.Length == 6)
        {
            var r = Convert.ToInt32(hex[..2], 16);
            var g = Convert.ToInt32(hex.Substring(2, 2), 16);
            var b = Convert.ToInt32(hex.Substring(4, 2), 16);
            return System.Drawing.Color.FromArgb(r, g, b);
        }

        if (hex.Length == 8)
        {
            var r = Convert.ToInt32(hex[..2], 16);
            var g = Convert.ToInt32(hex.Substring(2, 2), 16);
            var b = Convert.ToInt32(hex.Substring(4, 2), 16);
            var a = Convert.ToInt32(hex.Substring(6, 2), 16);
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

        throw new FormatException($"Invalid hex color: #{hex}");
    }

    private ColorOrAttribute ReadColorOrAttribute()
    {
        var peek = this.tokenReader.Peek();

        // Accept hex ("#FFAACC" or "#FFAACCAA") or numeric triplets/quads.
        if (peek is { Type: TokenType.String, Lexeme: ['#', ..] })
        {
            return ParseHexColor(this.tokenReader.ReadString());
        }

        if (peek is { Type: TokenType.Attribute })
        {
            return this.tokenReader.ReadAttribute();
        }

        int r = (int)this.tokenReader.ReadNumber();
        int g = (int)this.tokenReader.ReadNumber();
        int b = (int)this.tokenReader.ReadNumber();

        // Optional alpha
        int a = 255;
        if (this.tokenReader.Peek().Type is TokenType.Number)
        {
            var maybeAlpha = (int)this.tokenReader.ReadNumber();
            if (maybeAlpha >= 0 && maybeAlpha <= 255)
            {
                a = maybeAlpha;
            }
            else
            {
                this.tokenReader.PushBackNumber(maybeAlpha); // put back if it wasn't alpha (best-effort)
            }
        }

        return System.Drawing.Color.FromArgb(a, r, g, b);
    }

    private System.Drawing.Color ReadColor()
    {
        // Accept hex ("#FFAACC" or "#FFAACCAA") or numeric triplets/quads.
        if (this.tokenReader.Peek().Type is TokenType.String && this.tokenReader.Peek().Lexeme.Length > 0 && this.tokenReader.Peek().Lexeme[0] is '#')
        {
            return ParseHexColor(this.tokenReader.ReadString());
        }

        int r = (int)this.tokenReader.ReadNumber();
        int g = (int)this.tokenReader.ReadNumber();
        int b = (int)this.tokenReader.ReadNumber();

        // Optional alpha
        int a = 255;
        if (this.tokenReader.Peek().Type is TokenType.Number)
        {
            var maybeAlpha = (int)this.tokenReader.ReadNumber();
            if (maybeAlpha >= 0 && maybeAlpha <= 255)
            {
                a = maybeAlpha;
            }
            else
            {
                this.tokenReader.PushBackNumber(maybeAlpha); // put back if it wasn't alpha (best-effort)
            }
        }

        return System.Drawing.Color.FromArgb(a, r, g, b);
    }

    private System.Drawing.Point ReadPoint()
    {
        var x = (int)this.tokenReader.ReadNumber();
        var y = (int)this.tokenReader.ReadNumber();
        return new(x, y);
    }

    private System.Drawing.Size ReadSize()
    {
        var width = (int)this.tokenReader.ReadNumber();
        var height = (int)this.tokenReader.ReadNumber();
        return new(width, height);
    }

    private BoundingBox ReadExtent()
    {
        var minx = this.tokenReader.ReadNumber();
        var miny = this.tokenReader.ReadNumber();
        var maxx = this.tokenReader.ReadNumber();
        var maxy = this.tokenReader.ReadNumber();
        return new BoundingBox(minx, miny, maxx, maxy);
    }

    private List<double> ReadNumberList()
    {
        var list = new List<double>();

        // pattern typically ends either at newline or next keyword; read numbers greedily.
        while (this.tokenReader.Peek().Type is TokenType.Number)
        {
            list.Add(this.tokenReader.ReadNumber());
        }

        return list;
    }

    private string ReadExpressionOrString(IReadOnlySet<string> stopWords)
    {
        // Prefer a single quoted string if present
        if (this.tokenReader.Peek() is { Type: TokenType.String })
        {
            return this.tokenReader.ReadString();
        }

        var sb = new StringBuilder();
        while (!this.tokenReader.IsEof)
        {
            var t = this.tokenReader.Peek();

            if ((t.Type is TokenType.Keyword || t.Type is TokenType.Identifier) && stopWords.Contains(t.Lexeme))
            {
                break;
            }

            this.tokenReader.ReadAny();
            if (sb.Length > 0)
            {
                sb.Append(' ');
            }

            sb.Append(t.Lexeme);
        }

        return sb.ToString();
    }

    private MapStatus ReadStatus() => this.tokenReader.ReadStringOrIdentifier().ToLowerInvariant() switch
    {
        "on" => MapStatus.On,
        "off" => MapStatus.Off,
        "default" => MapStatus.Default,
        "embed" => MapStatus.Embed,
        var v => throw new FormatException($"Unsupported STATUS value: {v}"),
    };

    private MapUnits? ReadUnits() => this.tokenReader.ReadStringOrIdentifier().ToLowerInvariant() switch
    {
        "pixels" or "px" => MapUnits.Pixels,
        "feet" => MapUnits.Feet,
        "inches" => MapUnits.Inches,
        "kilometers" or "km" => MapUnits.Kilometers,
        "meters" or "m" => MapUnits.Meters,
        "miles" => MapUnits.Miles,
        "nauticalmiles" => MapUnits.NauticalMiles,
        "dd" => MapUnits.DD,
        _ => null,
    };

    private LayerType ReadLayerType() => this.tokenReader.ReadStringOrIdentifier().ToLowerInvariant() switch
    {
        "point" => LayerType.Point,
        "line" => LayerType.Line,
        "polygon" => LayerType.Polygon,
        "raster" => LayerType.Raster,
        "annotation" => LayerType.Annotation,
        "circle" => LayerType.Circle,
        var v => throw new FormatException($"Invalid TYPE '{v}'"),
    };

    private ConnectionType ReadConnectionType() => this.tokenReader.ReadStringOrIdentifier().ToLowerInvariant() switch
    {
        "ogr" => ConnectionType.Ogr,
        "postgis" => ConnectionType.PostGIS,
        "oraclespatial" => ConnectionType.OracleSpatial,
        "raster" => ConnectionType.Raster,
        "wms" => ConnectionType.Wms,
        "wfs" => ConnectionType.Wfs,
        "union" => ConnectionType.Union,
        "plugin" => ConnectionType.Plugin,
        "contour" => ConnectionType.Contour,
        "kerneldensity" => ConnectionType.KernelDensity,
        "idw" => ConnectionType.Idw,
        "uvraster" => ConnectionType.UvRaster,
        "rasterlabel" => ConnectionType.RasterLabel,
        "sde" => ConnectionType.Sde,
        _ => ConnectionType.Local,
    };

    private CornerPosition ReadCornerPosition() => this.tokenReader.ReadStringOrIdentifier().ToUpperInvariant() switch
    {
        "UL" => CornerPosition.UL,
        "UC" => CornerPosition.UC,
        "UR" => CornerPosition.UR,
        "LL" => CornerPosition.LL,
        "LC" => CornerPosition.LC,
        "LR" => CornerPosition.LR,
        var v => throw new FormatException($"Invalid POSITION '{v}'"),
    };

    private HorizontalAlign ReadAlign() => this.tokenReader.ReadStringOrIdentifier().ToLowerInvariant() switch
    {
        "left" => HorizontalAlign.Left,
        "center" => HorizontalAlign.Center,
        "right" => HorizontalAlign.Right,
        _ => HorizontalAlign.None,
    };

    private LabelPosition ReadLabelPosition() => this.tokenReader.ReadStringOrIdentifier().ToUpperInvariant() switch
    {
        "CC" => LabelPosition.CC,
        "UC" => LabelPosition.UC,
        "LC" => LabelPosition.LC,
        "CL" => LabelPosition.CL,
        "CR" => LabelPosition.CR,
        "UL" => LabelPosition.UL,
        "UR" => LabelPosition.UR,
        "LL" => LabelPosition.LL,
        "LR" => LabelPosition.LR,
        _ => LabelPosition.Auto,
    };

    [StructLayout(LayoutKind.Auto)]
    private ref struct TokenReader
    {
        private MapfileTokenizer.TokenEnumerator enumerator;
        private Token current;
        private bool hasCurrent;

        // One-token pushback for numbers (for color alpha handling).
        private double? pushedNumber;

        public TokenReader(MapfileTokenizer lexer)
        {
            this.enumerator = lexer.GetEnumerator();
            this.ResetCurrent();
        }

        public bool IsEof => this.Peek().Type is TokenType.EOF;

        public Token Peek()
        {
            if (this.hasCurrent)
            {
                return this.current;
            }

            if (!this.enumerator.MoveNext())
            {
                this.current = new Token(TokenType.EOF, ReadOnlySpan<char>.Empty, 0, 0);
                this.hasCurrent = true;
            }
            else
            {
                this.current = this.enumerator.Current;
                this.hasCurrent = true;
            }

            return this.current;
        }

        public Token ReadAny()
        {
            var t = this.Peek();
            this.ResetCurrent();
            return t;
        }

        public bool TryAccept(TokenType type)
        {
            var p = this.Peek();
            if (p.Type == type)
            {
                this.ResetCurrent();
                return true;
            }

            return false;
        }

        public bool TryAcceptKeyword(string keyword)
        {
            var p = this.Peek();
            if ((p.Type is TokenType.Keyword || p.Type is TokenType.Identifier)
                && p.Lexeme.Equals(keyword.AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                this.ResetCurrent();
                return true;
            }

            return false;
        }

        public void ExpectKeyword(string keyword)
        {
            if (!this.TryAcceptKeyword(keyword))
            {
                throw new InvalidOperationException($"Expected keyword '{keyword}' near {this.Peek().ToString()}");
            }
        }

        public string ReadString()
        {
            if (this.Peek() is { Type: TokenType.String, Lexeme: var lexeme })
            {
                this.ResetCurrent();
                return lexeme.ToString();
            }

            throw new InvalidOperationException($"Expected string near {this.current.ToString()}");
        }

        public Attribute ReadAttribute()
        {
            if (this.Peek() is { Type: TokenType.Attribute, Lexeme: var lexeme })
            {
                this.ResetCurrent();
                return new Attribute(lexeme.ToString());
            }

            throw new InvalidOperationException($"Expected string near {this.current.ToString()}");
        }

        public string ReadStringOrIdentifier()
        {
            if (this.Peek() is { Type: TokenType.String or TokenType.Identifier or TokenType.Keyword or TokenType.Boolean, Lexeme: var lexeme })
            {
                this.ResetCurrent();
                return lexeme.ToString();
            }

            throw new InvalidOperationException($"Expected string/identifier near {this.current.ToString()}");
        }

        public double ReadNumber()
        {
            if (this.pushedNumber.HasValue)
            {
                var v = this.pushedNumber.Value;
                this.pushedNumber = null;
                return v;
            }

            if (this.Peek() is { Type: TokenType.Number, Lexeme: var lexeme })
            {
                this.ResetCurrent();
                return double.Parse(lexeme, CultureInfo.InvariantCulture);
            }

            throw new InvalidOperationException($"Expected number near {this.current.ToString()}");
        }

        public CodeOfChaos.Unions.Union<string, double> ReadStringOrIdentifierOrNumber()
        {
            if (this.TryReadStringOrIdentifier(out var stringValue))
            {
                return stringValue;
            }

            if (this.TryReadNumber(out var numberValue))
            {
                return numberValue;
            }

            throw new InvalidOperationException($"Expected string/identifier near {this.current.ToString()}");
        }

        public bool TryReadStringOrIdentifier([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out string? value)
        {
            if (this.Peek() is { Type: TokenType.String or TokenType.Identifier or TokenType.Keyword or TokenType.Boolean, Lexeme: var lexeme })
            {
                this.ResetCurrent();
                value = lexeme.ToString();
                return true;
            }

            value = default;
            return false;
        }

        public bool TryReadNumber(out double value)
        {
            if (this.pushedNumber.HasValue)
            {
                value = this.pushedNumber.Value;
                this.pushedNumber = null;
                return true;
            }

            if (this.Peek() is { Type: TokenType.Number, Lexeme: var lexeme })
            {
                this.ResetCurrent();
                value = double.Parse(lexeme, CultureInfo.InvariantCulture);
                return true;
            }

            value = default;
            return false;
        }

        public void PushBackNumber(double value) => this.pushedNumber = value;

        public bool ReadBoolean()
        {
            var v = this.ReadStringOrIdentifier();
            return v.Equals("TRUE", StringComparison.OrdinalIgnoreCase) || v.Equals("ON", StringComparison.OrdinalIgnoreCase);
        }

        // We don't have line boundaries exposed in the token stream, so just consume one token.
        public void SkipLineOrUnknown() => this.ReadAny();

        private void ResetCurrent()
        {
            this.hasCurrent = false;
            this.current = default;
        }
    }
}