// -----------------------------------------------------------------------
// <copyright file="Layer.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// LAYER – combines data + styling via CLASS blocks.
/// </summary>
public sealed class Layer
{
    public string Name { get; set; } = "layer";

    public LayerType Type { get; set; } = LayerType.Polygon;

    public MapStatus Status { get; set; } = MapStatus.Off;

    public string? Group { get; set; } // GROUP (for turning groups on/off)

    public string? ClassGroup { get; set; } // CLASSGROUP (limit classes at render)

    public string? Data { get; set; } // DATA (file or table/column expr)

    public string? Connection { get; set; } // CONNECTION string

    public ConnectionType ConnectionType { get; set; } = ConnectionType.Local;

    public IDictionary<string, string> ConnectionOptions { get; } = new Dictionary<string, string>(StringComparer.Ordinal);

    /// <summary>Gets key/value values used for variable binding (e.g., PostGIS/Oracle) in SQL statements.</summary>
    public IDictionary<string, string> BindVals { get; } = new Dictionary<string, string>(StringComparer.Ordinal);

    public string? ClassItem { get; set; } // CLASSITEM (attribute for simple EXPRESSIONs)

    public string? Filter { get; set; } // FILTER (MapServer expression or native)

    public string? FilterItem { get; set; } // FILTERITEM (simple FILTER attr)

    public string? LabelItem { get; set; } // LABELITEM (attribute used for labeling)

    public string? LabelRequires { get; set; } // LABELREQUIRES (layer logic expression)

    public double? LabelMinScaleDenom { get; set; } // LABELMINSCALEDENOM

    public double? LabelMaxScaleDenom { get; set; } // LABELMAXSCALEDENOM

    public double? MinScaleDenom { get; set; } // MINSCALEDENOM

    public double? MaxScaleDenom { get; set; } // MAXSCALEDENOM

    public double? MinGeoWidth { get; set; } // MINGEOWIDTH

    public double? MaxGeoWidth { get; set; } // MAXGEOWIDTH

    public int? MinFeatureSize { get; set; } // MINFEATURESIZE

    public int? MaxFeatures { get; set; } // MAXFEATURES

    public string? Header { get; set; } // result template header

    public string? Footer { get; set; } // result template footer

    public string? Template { get; set; } // result template

    public string? Requires { get; set; } // REQUIRES (layer logic expression)

    public string? Mask { get; set; } // MASK (layer name used as mask)

    /// <summary>Gets or sets override layer-level units for certain size-related style ops.</summary>
    public MapUnits? Units { get; set; } // UNITS

    /// <summary>Gets or sets override layer-level units for certain size-related style ops.</summary>
    public MapUnits? SizeUnits { get; set; } // SIZEUNITS

    /// <summary>Gets list of PROCESSING directives (free-form key=value strings).</summary>
    public IList<string> Processing { get; internal init; } = [];

    public IDictionary<string, string> Metadata { get; internal init; } = new Dictionary<string, string>(StringComparer.Ordinal);

    public IDictionary<string, string> Validation { get; internal init; } = new Dictionary<string, string>(StringComparer.Ordinal);

    public Projection? Projection { get; set; } // layer-level PROJECTION

    public BoundingBox? Extent { get; set; } // layer-level EXTENT (optimization hint)

    /// <summary>Gets or sets optional geometry transform expression (or "javascript://...").</summary>
    public string? GeomTransform { get; set; }

    public Grid? Grid { get; set; } // GRID (if used with TYPE LINE)

    public Identify? Identify { get; set; } // IDENTIFY (8.6+)

    public Leader? Leader { get; set; } // LEADER (for label leader lines)

    /// <summary>Gets feature-level JOINs (available after query).</summary>
    public IList<Join> Joins { get; internal init; } = [];

    /// <summary>Gets or sets cOMPOSITE blending pipeline for the entire layer render.</summary>
    public Composite? Composite { get; set; }

    /// <summary>Gets child classes (feature selection and style sets).</summary>
    public IList<Class> Classes { get; internal init; } = [];

    /// <summary>
    /// Gets or sets sensitivity for point queries (in <see cref="ToleranceUnits"/>). If layer type is
    /// POINT/LINE, default is 3; for others, default is 0.
    /// </summary>
    [Obsolete("Moved in MapServer 8.6; use LAYER IDENTITY for tolerance.")]
    public double? Tolerance { get; set; }

    /// <summary>
    /// Gets or sets the units of <see cref="Tolerance"/>: <c>pixels|feet|inches|kilometers|meters|miles|nauticalmiles|dd</c>.
    /// Default is <c>pixels</c>.
    /// </summary>
    [Obsolete("Moved in MapServer 8.6; use LAYER IDENTITY for tolerance.")]
    public MapUnits? ToleranceUnits { get; set; }
}