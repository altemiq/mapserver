// -----------------------------------------------------------------------
// <copyright file="Layer.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Represents a Mapfile <c>LAYER</c>: a combination of data + styling (via <c>CLASS</c> / <c>STYLE</c>).
/// </summary>
public sealed class Layer
{
    /// <summary>
    /// Gets or sets the unique layer name (<c>NAME</c>) used in requests and template bindings.
    /// </summary>
    public string Name { get; set; } = "layer";

    /// <summary>
    /// Gets or sets the layer geometry type (<c>TYPE</c>). Required for rendering.
    /// </summary>
    public LayerType Type { get; set; } = LayerType.Polygon;

    /// <summary>
    /// Gets or sets the layer drawing status (<c>STATUS</c>) for CGI / rendering pipelines.
    /// </summary>
    public MapStatus Status { get; set; } = MapStatus.Off;

    /// <summary>
    /// Gets or sets the optional logical group name (<c>GROUP</c>) to toggle multiple layers together.
    /// </summary>
    public string? Group { get; set; }

    /// <summary>
    /// Gets or sets the class group to consider at render time (<c>CLASSGROUP</c>).
    /// </summary>
    public string? ClassGroup { get; set; }

    /// <summary>
    /// Gets or sets the data source selector (<c>DATA</c>): path or table/column expression (e.g., PostGIS/Oracle/OGR).
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// Gets or sets the connection string for remote data sources (<c>CONNECTION</c>).
    /// </summary>
    public string? Connection { get; set; }

    /// <summary>
    /// Gets or sets the connection type (<c>CONNECTIONTYPE</c>).
    /// </summary>
    [DefaultValue(ConnectionType.Local)]
    public ConnectionType ConnectionType { get; set; } = ConnectionType.Local;

    /// <summary>
    /// Gets the optional key/value options passed to drivers (<c>CONNECTIONOPTIONS</c>).
    /// </summary>
    public IDictionary<string, string> ConnectionOptions { get; } =
        new Dictionary<string, string>(StringComparer.Ordinal);

    /// <summary>
    /// Gets the bind values for parameterized SQL in PostGIS/Oracle (<c>BINDVALS</c>), preventing injection.
    /// </summary>
    public IDictionary<string, string> BindVals { get; } =
        new Dictionary<string, string>(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets the attribute used for simple class lookup (<c>CLASSITEM</c>).
    /// </summary>
    public string? ClassItem { get; set; }

    /// <summary>
    /// Gets or sets the attribute filter evaluated before class expressions (<c>FILTER</c>).
    /// </summary>
    public string? Filter { get; set; }

    /// <summary>
    /// Gets or sets the attribute key used with simple <see cref="Filter"/> expressions (<c>FILTERITEM</c>).
    /// </summary>
    public string? FilterItem { get; set; }

    /// <summary>
    /// Gets or sets the attribute used for labeling (<c>LABELITEM</c>).
    /// </summary>
    public string? LabelItem { get; set; }

    /// <summary>
    /// Gets or sets the context expression to control labeling (<c>LABELREQUIRES</c>).
    /// </summary>
    public string? LabelRequires { get; set; }

    /// <summary>Gets or sets the maximum scale denominator at which this layer is labeled (<c>LABELMINSCALEDENOM</c>).</summary>
    public double? LabelMinScaleDenom { get; set; }

    /// <summary>Gets or sets the minimum scale denominator at which this layer is labeled (<c>LABELMAXSCALEDENOM</c>).</summary>
    public double? LabelMaxScaleDenom { get; set; }

    /// <summary>Gets or sets the maximum scale denominator at which this LAYER is drawn (<c>MINSCALEDENOM</c>).</summary>
    public double? MinScaleDenom { get; set; }

    /// <summary>Gets or sets the minimum scale denominator at which this LAYER is drawn (<c>MAXSCALEDENOM</c>).</summary>
    public double? MaxScaleDenom { get; set; }

    /// <summary>Gets or sets the minimum map width (geographic units) at which the layer is drawn (<c>MINGEOWIDTH</c>).</summary>
    public double? MinGeoWidth { get; set; }

    /// <summary>Gets or sets the maximum map width (geographic units) at which the layer is drawn (<c>MAXGEOWIDTH</c>).</summary>
    public double? MaxGeoWidth { get; set; }

    /// <summary>Gets or sets the minimum feature size in pixels (<c>MINFEATURESIZE</c>).</summary>
    public int? MinFeatureSize { get; set; }

    /// <summary>Gets or sets the limit of features to draw in the current window (<c>MAXFEATURES</c>).</summary>
    public int? MaxFeatures { get; set; }

    /// <summary>Gets or sets the template header for query result output (<c>HEADER</c>).</summary>
    public string? Header { get; set; }

    /// <summary>Gets or sets the template footer for query result output (<c>FOOTER</c>).</summary>
    public string? Footer { get; set; }

    /// <summary>Gets or sets the template for query result output (<c>TEMPLATE</c>).</summary>
    public string? Template { get; set; }

    /// <summary>Gets or sets the layer logic expression controlling draw (<c>REQUIRES</c>).</summary>
    public string? Requires { get; set; }

    /// <summary>
    /// Gets or sets the layer mask (<c>MASK</c>): only render where this layer intersects features of the named mask layer.
    /// </summary>
    public string? Mask { get; set; }

    /// <summary>Gets or sets the override layer units for size‑related operations (<c>UNITS</c>).</summary>
    public MapUnits? Units { get; set; }

    /// <summary>Gets or sets the override units for size‑related operations (<c>SIZEUNITS</c>).</summary>
    public MapUnits? SizeUnits { get; set; }

    /// <summary>Gets the free‑form rendering directives (<c>PROCESSING</c> key=value; multiple entries supported).</summary>
    public IList<string> Processing { get; } = [];

    /// <summary>Gets the arbitrary metadata (<c>METADATA</c>) used by OGC services and templates.</summary>
    public IDictionary<string, string> Metadata { get; } =
        new Dictionary<string, string>(StringComparer.Ordinal);

    /// <summary>Gets the runtime substitution validators (<c>VALIDATION</c> entries).</summary>
    public IDictionary<string, string> Validation { get; } =
        new Dictionary<string, string>(StringComparer.Ordinal);

    /// <summary>Gets or sets the layer‑level projection (<c>PROJECTION</c>).</summary>
    public Projection? Projection { get; set; }

    /// <summary>
    /// Gets or sets the layer‑level extent (<c>EXTENT</c>)—optional optimization to avoid computing extents.
    /// </summary>
    public BoundingBox? Extent { get; set; }

    /// <summary>
    /// Gets or sets the optional geometry transform (<c>GEOMTRANSFORM</c>).
    /// </summary>
    public string? GeomTransform { get; set; }

    /// <summary>Gets or sets the grid definition for graticules (<c>GRID</c>).</summary>
    public Grid? Grid { get; set; }

    /// <summary>Gets or sets the leader line configuration for labels (<c>LEADER</c>).</summary>
    public Leader? Leader { get; set; }

    /// <summary>Gets the declared <c>JOIN</c> objects available after queries.</summary>
    public IList<Join> Joins { get; } = [];

    /// <summary>Gets or sets the layer‑level compositing pipeline (<c>COMPOSITE</c>).</summary>
    public Composite? Composite { get; set; }

    /// <summary>Gets the child class definitions (<c>CLASS</c>) controlling selection and symbolization.</summary>
    public IList<Class> Classes { get; } = [];

    // ------------------------------
    // Query tolerance (8.0 — still on LAYER; IDENTIFY was added in 8.6)
    // ------------------------------

    /// <summary>
    /// Gets or sets the sensitivity for point‑based queries (<c>TOLERANCE</c>), given in <see cref="ToleranceUnits"/>.
    /// </summary>
    public double? Tolerance { get; set; }

    /// <summary>
    /// Gets or sets the units for <see cref="Tolerance"/> (<c>TOLERANCEUNITS</c>).
    /// </summary>
    [DefaultValue(MapUnits.Pixels)]
    public MapUnits? ToleranceUnits { get; set; }

    // ------------------------------
    // Legacy / version-gated members
    // ------------------------------

    /// <summary>
    /// Gets or sets the legacy DUMP flag. (Removed in MapServer 8.0) Use LAYER <c>METADATA</c> instead.
    /// </summary>
    [Obsolete("Removed in MapServer 8.0. Replace with appropriate LAYER METADATA keys.")]
    public bool? Dump { get; set; }

    /// <summary>
    /// Gets or sets the IDENTIFY block placeholder (not available in MapServer 8.0; introduced in 8.6).
    /// </summary>
    [Obsolete("Not available in MapServer 8.0. IDENTIFY was introduced in 8.6.")]
    public Identify? Identify { get; set; }
}