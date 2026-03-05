namespace Altemiq.IO.MapFile.Tests;

public class SerializerTests
{
    [Test]
    public async Task Serialize_BasicMap_EmitsExpectedStructure()
    {
        var map = new Map
        {
            Name = "MS",
            Size = new(800, 600),
            Extent = new BoundingBox(-137, 29, -53, 88),
            Units = MapUnits.DD,
            ImageType = "png",
            Projection = new Projection { Parameters = { "init=epsg:4326" } },
            Web = new Web
            {
                ImagePath = "/var/www/tmp/",
                ImageUrl = "/tmp/",
                Metadata = { ["ows_enable_request"] = "*" }
            }
        };

        map.Layers.Add(new Layer
        {
            Name = "admin_countries",
            Status = MapStatus.On,
            Type = LayerType.Polygon,
            Data = "ne_10m_admin_0_countries",
            Classes =
        {
            new Class
            {
                Name = "Countries",
                Styles =
                {
                    new Style { Color = System.Drawing.Color.FromArgb(246, 241, 223) },
                    new Style { OutlineColor = System.Drawing.Color.FromArgb(0, 0, 0), Width = 1 }
                }
            }
        }
        });

        var n = TestHelpers.NormalizeMapfile(Serialization.MapfileSerializer.Serialize(map));

        using (Assert.Multiple())
        {
            await Assert.That(n).Contains("MAP");
            await Assert.That(n).Contains("END");
            await Assert.That(n).Contains("LAYER");
            await Assert.That(n).Contains("CLASS");
            await Assert.That(n).Contains("STYLE");
            await Assert.That(n).Contains("PROJECTION");
            await Assert.That(n).Contains("\"init=epsg:4326\"");
            await Assert.That(n).Contains("METADATA");
            await Assert.That(n).Contains("\"ows_enable_request\" \"*\"");
        }
    }

    [Test]
    public async Task Serialize_OutputFormat_IncludesDriverAndOptions()
    {
        var map = new Map
        {
            Name = "WithOF",
            Size = new(256, 256),
            OutputFormats =
        {
            new OutputFormat
            {
                Name = "png",
                Driver = "AGG/PNG",
                MimeType = "image/png",
                ImageMode = "RGB",
                Extension = "png",
                FormatOptions = { ["GAMMA"] = "0.75", ["QUANTIZE_COLORS"] = "256" }
            }
        }
        };

        var n = TestHelpers.NormalizeMapfile(Serialization.MapfileSerializer.Serialize(map));

        using (Assert.Multiple())
        {
            await Assert.That(n).Contains("OUTPUTFORMAT");
            await Assert.That(n).Contains("NAME \"png\"");
            await Assert.That(n).Contains("DRIVER AGG/PNG");
            await Assert.That(n).Contains("MIMETYPE \"image/png\"");
            await Assert.That(n).Contains("IMAGEMODE RGB");
            await Assert.That(n).Contains("EXTENSION \"png\"");
            await Assert.That(n).Contains("FORMATOPTION \"GAMMA=0.75\"");
            await Assert.That(n).Contains("FORMATOPTION \"QUANTIZE_COLORS=256\"");
        }
    }

    [Test]
    public async Task Serialize_Metadata_EscapesQuotes()
    {
        var map = new Map
        {
            Size = new(1, 1),
            Metadata =
            {
                ["wms_title"] = "The \"World\" Map",
            },
        };

        var n = TestHelpers.NormalizeMapfile(Serialization.MapfileSerializer.Serialize(map));
        await Assert.That(n).Contains("\"wms_title\" \"The \\\"World\\\" Map\"");
    }

    [Test]
    public async Task Serialize_Full_Wfs()
    {
        var builder = Builders.MapBuilder.New();

        builder
            .WithExtent(-180, -90, 180, 90)
            .WithImageColor(System.Drawing.Color.White)
            .WithWeb(() =>
                Builders.WebBuilder
                    .New()
                    .WithMetadata(new Dictionary<string, string>()
                    {
                        { "ows_title", "AIMS Clearance Map"},
                        {"ows_onlineresource", "${PROXY_HOST_URL}/maps/wfs"},
                        {"ows_srs", "EPSG:4326 EPSG:4269 EPSG:3978 EPSG:3857"},
                        {"ows_enable_request", "*"}
                    }))
            .WithProjection(Builders.ProjectionBuilder.New().WithEpsg(4326).Build)
            .WithLayers(() =>
            [
                Builders.LayerBuilder.New()
                    .WithGroup("catalog")
                    .WithName("dataset_las_boundary")
                    .WithConnectionType(ConnectionType.PostGIS)
                    .WithConnection("${CONNECTIONSTRINGS__POSTGRES__CATALOG}")
                    .WithProcessing(dictionary => dictionary.Add("CLOSE_CONNECTION", "DEFER"))
                    .WithStatus(MapStatus.On)
                    .WithMetadata(dictionary =>
                    {
                        dictionary.Add("wfs_enable_request", "*");
                        dictionary.Add("wfs_title", "dataset_las_boundary");
                        dictionary.Add("wfs_srs", "EPSG:4326");
                        dictionary.Add("gml_include_items", "all");
                        dictionary.Add("gml_featureid", "file_id");
                        dictionary.Add("gml_types", "auto");
                    })
                    .WithType(LayerType.Polygon)
                    .WithData(
                    """
                            geometry from (
                            SELECT
                                las.id AS file_id,
                                las.dataset_id,
                                las.organization,
                                las.file_name,
                                las.parent_file_name,
                                las.metadata,
                                las.version,
                                las.boundary::geometry as geometry
                            FROM dataset_files las
                            JOIN latest_dataset_files ON latest_dataset_files.file_id = las.id
                            WHERE data_type = 'DatasetLasFile'
                            ) as subquery using unique file_id using srid=4326
                            """)
                    .WithFilter("%org%")
                    .WithFilterItem("organization")
                    .WithValidation(dictionary =>
                    {
                        dictionary.Add("org", "^[a-zA-Z\\-]+$");
                        dictionary.Add("default_org", "Development");
                    })
                    .WithProjection(Builders.ProjectionBuilder.New().WithEpsg(4326).Build),
                Builders.LayerBuilder.New()
                    .WithGroup("catalog")
                    .WithName("dataset_tif_boundary")
                    .WithConnectionType(ConnectionType.PostGIS)
                    .WithConnection("${CONNECTIONSTRINGS__POSTGRES__CATALOG}")
                    .WithProcessing(dictionary => dictionary.Add(            "CLOSE_CONNECTION", "DEFER"))
                    .WithStatus(MapStatus.On)
                    .WithMetadata(dictionary =>
                    {
                        dictionary.Add(            "wfs_enable_request", "*");
                        dictionary.Add("wfs_title", "dataset_tif_boundary");
                        dictionary.Add("wfs_srs", "EPSG:4326");
                        dictionary.Add("gml_include_items", "all");
                        dictionary.Add("gml_featureid", "file_id");
                        dictionary.Add("gml_types", "auto");
                    })
                    .WithType(LayerType.Polygon)
                    .WithData(
                    """
                         geometry from (
                           SELECT
                             tif.id AS file_id,
                             tif.dataset_id,
                             tif.organization,
                             tif.file_name,
                             tif.parent_file_name,
                             tif.metadata,
                             tif.version,
                             tif.boundary::geometry as geometry
                           FROM dataset_files tif
                           JOIN latest_dataset_files ON latest_dataset_files.file_id = tif.id
                           WHERE data_type = 'DatasetTifFile'
                         ) as subquery using unique file_id using srid=4326
                         """)
                    .WithFilter("%org%")
                    .WithFilterItem("organization")
                    .WithValidation(dictionary =>
                    {
                        dictionary.Add("org", "^[a-zA-Z\\-]+$");
                        dictionary.Add("default_org", "Development");
                    })
                    .WithProjection(Builders.ProjectionBuilder.New().WithEpsg(4326).Build),
                Builders.LayerBuilder.New()
                    .WithGroup("review")
                    .WithName("dataset_review_region")
                    .WithConnectionType(ConnectionType.PostGIS)
                    .WithConnection("${CONNECTIONSTRINGS__POSTGRES__REVIEW}")
                    .WithProcessing(dictionary => dictionary.Add("CLOSE_CONNECTION", "DEFER"))
                    .WithStatus(MapStatus.On)
                    .WithMetadata(dictionary =>
                    {
                        dictionary.Add("wfs_enable_request", "*");
                        dictionary.Add("wfs_title", "dataset_review_region");
                        dictionary.Add("wfs_srs", "EPSG:4326");
                        dictionary.Add("gml_include_items", "all");
                        dictionary.Add("gml_featureid", "review_id");
                        dictionary.Add("gml_types", "auto");
                    })
                    .WithType(LayerType.Polygon)
                    .WithData(
                    """
                         geometry from (
                           SELECT
                             id AS review_id,
                             dataset_id,
                             organization,
                             created_by,
                             created_at,
                             status,
                             assigned_to,
                             last_opened_at,
                             last_closed_at,
                             region::geometry as geometry
                           FROM reviews
                           WHERE dataset_id IS NOT NULL
                             AND status <> 'Deleted'
                         ) as subquery using unique review_id using srid=4326
                         """)
                    .WithFilter("%org%")
                    .WithFilterItem("organization")
                    .WithValidation(dictionary =>
                    {
                        dictionary.Add("org", "^[a-zA-Z\\-]+$");
                        dictionary.Add("default_org", "Development");
                    })
                    .WithProjection(Builders.ProjectionBuilder.New().WithEpsg(4326).Build)
            ]);

        var serialized = TestHelpers.NormalizeMapfile(Serialization.MapfileSerializer.Serialize(builder.Build()));

        await Assert.That(serialized).IsEqualTo(TestHelpers.NormalizeMapfile(
            """
               MAP
                 EXTENT -180 -90 180 90
                 IMAGECOLOR 255 255 255
                 WEB
                   METADATA
                     "ows_title"          "AIMS Clearance Map"
                     "ows_onlineresource" "${PROXY_HOST_URL}/maps/wfs"
                     "ows_srs"            "EPSG:4326 EPSG:4269 EPSG:3978 EPSG:3857"
                     "ows_enable_request" "*"
                   END # METADATA
                 END # WEB
                 PROJECTION
                   "init=epsg:4326"
                 END # PROJECTION
                 LAYER
                   GROUP "catalog"
                   NAME "dataset_las_boundary"
                   CONNECTIONTYPE POSTGIS
                   CONNECTION "${CONNECTIONSTRINGS__POSTGRES__CATALOG}"
                   PROCESSING "CLOSE_CONNECTION=DEFER"
                   STATUS ON
                   METADATA
                     "wfs_enable_request" "*"
                     "wfs_title"          "dataset_las_boundary"
                     "wfs_srs"            "EPSG:4326"
                     "gml_include_items"  "all"
                     "gml_featureid"      "file_id"
                     "gml_types"          "auto"
                   END # METADATA
                   TYPE POLYGON
                   DATA "geometry from (
                           SELECT
                             las.id AS file_id,
                             las.dataset_id,
                             las.organization,
                             las.file_name,
                             las.parent_file_name,
                             las.metadata,
                             las.version,
                             las.boundary::geometry as geometry
                           FROM dataset_files las
                           JOIN latest_dataset_files ON latest_dataset_files.file_id = las.id
                           WHERE data_type = 'DatasetLasFile'
                         ) as subquery using unique file_id using srid=4326"
                   FILTER "%org%"
                   FILTERITEM "organization"
                   VALIDATION
                     'org'         '^[a-zA-Z\-]+$'
                     'default_org' 'Development'
                   END # VALIDATION
                   PROJECTION
                     "init=epsg:4326"
                   END # PROJECTION
                 END # LAYER
                 LAYER
                   GROUP "catalog"
                   NAME "dataset_tif_boundary"
                   CONNECTIONTYPE POSTGIS
                   CONNECTION "${CONNECTIONSTRINGS__POSTGRES__CATALOG}"
                   PROCESSING "CLOSE_CONNECTION=DEFER"
                   STATUS ON
                   METADATA
                     "wfs_enable_request" "*"
                     "wfs_title"          "dataset_tif_boundary"
                     "wfs_srs"            "EPSG:4326"
                     "gml_include_items"  "all"
                     "gml_featureid"      "file_id"
                     "gml_types"          "auto"
                   END # METADATA
                   TYPE POLYGON
                   DATA "geometry from (
                           SELECT
                             tif.id AS file_id,
                             tif.dataset_id,
                             tif.organization,
                             tif.file_name,
                             tif.parent_file_name,
                             tif.metadata,
                             tif.version,
                             tif.boundary::geometry as geometry
                           FROM dataset_files tif
                           JOIN latest_dataset_files ON latest_dataset_files.file_id = tif.id
                           WHERE data_type = 'DatasetTifFile'
                         ) as subquery using unique file_id using srid=4326"
                   FILTER "%org%"
                   FILTERITEM "organization"
                   VALIDATION
                     'org'         '^[a-zA-Z\-]+$'
                     'default_org' 'Development'
                   END # VALIDATION
                   PROJECTION
                     "init=epsg:4326"
                   END # PROJECTION
                 END # LAYER
                 LAYER
                   GROUP "review"
                   NAME "dataset_review_region"
                   CONNECTIONTYPE POSTGIS
                   CONNECTION "${CONNECTIONSTRINGS__POSTGRES__REVIEW}"
                   PROCESSING "CLOSE_CONNECTION=DEFER"
                   STATUS ON
                   METADATA
                     "wfs_enable_request" "*"
                     "wfs_title"          "dataset_review_region"
                     "wfs_srs"            "EPSG:4326"
                     "gml_include_items"  "all"
                     "gml_featureid"      "review_id"
                     "gml_types"          "auto"
                   END # METADATA
                   TYPE POLYGON
                   DATA "geometry from (
                           SELECT
                             id AS review_id,
                             dataset_id,
                             organization,
                             created_by,
                             created_at,
                             status,
                             assigned_to,
                             last_opened_at,
                             last_closed_at,
                             region::geometry as geometry
                           FROM reviews
                           WHERE dataset_id IS NOT NULL
                             AND status <> 'Deleted'
                         ) as subquery using unique review_id using srid=4326"
                   FILTER "%org%"
                   FILTERITEM "organization"
                   VALIDATION
                     'org'         '^[a-zA-Z\-]+$'
                     'default_org' 'Development'
                   END # VALIDATION
                   PROJECTION
                     "init=epsg:4326"
                   END # PROJECTION
                 END # LAYER
               END # MAP
               """));
    }

    [Test]
    public async Task Serialize_Full_Wms()
    {
        var builder = Builders.MapBuilder.New();

        builder
            .WithName("WMS")
            .WithSize(1224, 683)
            .WithUnits(MapUnits.Meters)
            .WithImageColor(System.Drawing.Color.White)
            .WithFontSet("/etc/mapserver/styles/fonts/fonts.txt")
            .WithSymbolSet("/etc/mapserver/styles/symbols/symbols.sym")
            .WithImageType("png24")
            .WithMaxSize(4096)
            .WithWeb(() =>
                Builders.WebBuilder.New()
                    .WithFooterTemplate("/etc/mapserver/styles/templates/footer.html")
                    .WithHeaderTemplate("/etc/mapserver/styles/templates/header.html")
                    .WithImagePath("/ms4w/tmp/ms_tmp/")
                    .WithImageUrl("/ms_tmp/")
                    .WithMetadata(dictionary =>
                    {
                        dictionary.Add("wms_title", "AIMS Clearance Map");
                        dictionary.Add("wms_onlineresource", "${PROXY_HOST_URL}/maps/wms");
                        dictionary.Add("wms_srs", "EPSG:4326 EPSG:4269 EPSG:3978 EPSG:3857");
                        dictionary.Add("wms_enable_request", "*");
                        dictionary.Add("wms_feature_info_mime_type", "text/html");
                        dictionary.Add("tile_map_edge_buffer", "8");
                    }))
            .WithConfig(dictionary => dictionary.Add("MS_ERRORFILE", "stderr"))
            .WithOutputFormats(() =>
                [
                    Builders.OutputFormatBuilder.New()
                        .WithName("png24")
                        .WithMimeType(System.Net.Mime.MediaTypeNames.Image.Png)
                        .WithDriver("AGG/PNG")
                        .WithExtension("png")
                        .WithImageMode("RGB")
                        .WithTransparent(true)
                        .Build()
                ])
            .WithProjection(() =>
                Builders.ProjectionBuilder.New()
                    .WithParameters(
                    [
                        "proj=longlat",
                        "ellps=GRS80",
                        "towgs84=0,0,0,0,0,0,0",
                        "no_defs",
                        ])
                    .Build())
            .WithLegend(() =>
                Builders.LegendBuilder.New()
                    .WithKeySize(20, 10)
                    .WithKeySpacing(5, 5)
                    .WithLabel(() =>
                        Builders.LabelBuilder.New()
                            .WithSize("MEDIUM")
                            .WithOffset(0, 0)
                            .WithShadowSize(1, 1)
                            .WithType("BITMAP")
                            .Build())
                    .Build())
            .WithQueryMap(() =>
                Builders.QueryMapBuilder.New()
                    .WithSize(-1, -1)
                    .WithStatus(MapStatus.Off)
                    .WithStyle(QueryMapStyle.Hilite)
                    .Build())
            .WithScaleBar(() =>
                Builders.ScaleBarBuilder.New()
                    .WithIntervals(4)
                    .WithLabel(() =>
                        Builders.LabelBuilder.New()
                            .WithSize("MEDIUM")
                            .WithOffset(0, 0)
                            .WithShadowSize(1, 1)
                            .WithType("BITMAP")
                            .Build())
                    .WithSize(200, 3)
                    .WithStatus(MapStatus.Off)
                    .WithUnits(MapUnits.Miles)
                    .Build())
            .WithLayers(() =>
            [
                Builders.LayerBuilder.New()
                    .WithName("dataset_las_boundary")
                    .WithConnectionType(ConnectionType.PostGIS)
                    .WithConnection("${CONNECTIONSTRINGS__POSTGRES__CATALOG}")
                    .WithProcessing(dictionary => dictionary.Add( "CLOSE_CONNECTION", "DEFER"))
                    .WithStatus(MapStatus.On)
                    .WithMetadata(dictionary =>
                    {
                        dictionary.Add("wms_title", "dataset_las_boundary");
                        dictionary.Add("wms_srs", "EPSG:4326");
                        dictionary.Add("wms_enable_request", "*");
                        dictionary.Add("gml_include_items", "all");
                        dictionary.Add("gml_featureid", "fid");
                        dictionary.Add("gml_types", "auto");
                        dictionary.Add("ows_title", "dataset_las_boundary");
                        dictionary.Add("ows_enable_request", "*");
                        dictionary.Add("ows_include_items", "all");
                        dictionary.Add("ows_geometry_type", "polygon");
                        dictionary.Add("ows_geometries", "geometry");
                    })
                    .WithType(LayerType.Polygon)
                    .WithTolerance(10)
                    .WithToleranceUnits(MapUnits.Pixels)
                    .WithTemplate("/etc/mapserver/styles/templates/file.html")
                    .WithUnits(MapUnits.Meters)
                    .WithData(
                        """
                         geometry from (
                           SELECT
                               las.id AS fid,
                               las.dataset_id,
                               las.organization,
                               las.file_name,
                               las.metadata,
                               las.version,
                               las.boundary::geometry as geometry,
                               (colour.b24 >> 16) % 256 || ' ' || (colour.b24 >> 8) % 256 || ' ' || colour.b24 % 256 AS colour_rgb
                           FROM dataset_files las
                           JOIN latest_dataset_files ON latest_dataset_files.file_id = las.id
                           JOIN LATERAL ( SELECT abs(uuid_hash(las.dataset_id)) % 16777216 AS b24 ) colour ON true
                           WHERE data_type ILIKE 'DatasetLasFile'
                         ) as subquery using unique fid using srid=4326
                         """)
                    .WithFilter("%org%")
                    .WithFilterItem("organization")
                    .WithValidation(dictionary =>
                    {
                        dictionary.Add("org", "^[a-zA-Z\\-]+$");
                        dictionary.Add("default_org", "Development");
                    })
                    .WithProjection(Builders.ProjectionBuilder.New().WithEpsg(4326).Build)
                    .WithClasses(() =>
                    [
                        Builders.ClassBuilder.New()
                            .WithStyles(() =>
                            [
                                Builders.StyleBuilder.New().WithColor(new Attribute("colour_rgb")).WithOpacity(50),
                                Builders.StyleBuilder.New().WithOutlineColor(new Attribute("colour_rgb")).WithWidth(3),
                            ])
                    ])
             ])
            .Build();

        var serialized = TestHelpers.NormalizeMapfile(Serialization.MapfileSerializer.Serialize(builder.Build()));

        await Assert.That(serialized).IsEqualTo(TestHelpers.NormalizeMapfile(
            """
               MAP
                 NAME "WMS"
                 SIZE 1224 683
                 UNITS METERS
                 IMAGECOLOR 255 255 255
                 FONTSET "/etc/mapserver/styles/fonts/fonts.txt"
                 SYMBOLSET "/etc/mapserver/styles/symbols/symbols.sym"
                 IMAGETYPE "png24"
                 MAXSIZE 4096
                 WEB
                   FOOTER "/etc/mapserver/styles/templates/footer.html"
                   HEADER "/etc/mapserver/styles/templates/header.html"
                   IMAGEPATH "/ms4w/tmp/ms_tmp/"
                   IMAGEURL "/ms_tmp/"
                   METADATA
                     "wms_title"                  "AIMS Clearance Map"
                     "wms_onlineresource"         "${PROXY_HOST_URL}/maps/wms"
                     "wms_srs"                    "EPSG:4326 EPSG:4269 EPSG:3978 EPSG:3857"
                     "wms_enable_request"         "*"
                     "wms_feature_info_mime_type" "text/html"
                     "tile_map_edge_buffer"       "8"
                   END # METADATA
                 END # WEB
                 CONFIG "MS_ERRORFILE" "stderr"
                 OUTPUTFORMAT
                   NAME "png24"
                   MIMETYPE "image/png"
                   DRIVER AGG/PNG
                   EXTENSION "png"
                   IMAGEMODE RGB
                   TRANSPARENT TRUE
                 END # OUTPUTFORMAT
                 PROJECTION
                   "proj=longlat"
                   "ellps=GRS80"
                   "towgs84=0,0,0,0,0,0,0"
                   "no_defs"
                 END # PROJECTION
                 LEGEND
                   KEYSIZE 20 10
                   KEYSPACING 5 5
                   LABEL
                     SIZE MEDIUM
                     OFFSET 0 0
                     SHADOWSIZE 1 1
                     TYPE BITMAP
                   END # LABEL
                   STATUS OFF
                 END # LEGEND
                 QUERYMAP
                   SIZE -1 -1
                   STATUS OFF
                   STYLE HILITE
                 END # QUERYMAP
                 SCALEBAR 
                   INTERVALS 4
                   LABEL
                     SIZE MEDIUM
                     OFFSET 0 0
                     SHADOWSIZE 1 1
                     TYPE BITMAP
                   END # LABEL
                   SIZE 200 3
                   STATUS OFF
                   UNITS MILES
                 END # SCALEBAR
                 LAYER
                   NAME "dataset_las_boundary"
                   CONNECTIONTYPE POSTGIS
                   CONNECTION "${CONNECTIONSTRINGS__POSTGRES__CATALOG}"
                   PROCESSING "CLOSE_CONNECTION=DEFER"
                   STATUS ON
                   METADATA
                     "wms_title"          "dataset_las_boundary"
                     "wms_srs"            "EPSG:4326"
                     "wms_enable_request" "*"
                     "gml_include_items"  "all"
                     "gml_featureid"      "fid"
                     "gml_types"          "auto"
                     "ows_title"          "dataset_las_boundary"
                     "ows_enable_request" "*"
                     "ows_include_items"  "all"
                     "ows_geometry_type"  "polygon"
                     "ows_geometries"     "geometry"
                   END # METADATA
                   TYPE POLYGON
                   TOLERANCE 10
                   TOLERANCEUNITS PIXELS
                   TEMPLATE "/etc/mapserver/styles/templates/file.html"
                   UNITS METERS
                   DATA "geometry from (
                           SELECT
                               las.id AS fid,
                               las.dataset_id,
                               las.organization,
                               las.file_name,
                               las.metadata,
                               las.version,
                               las.boundary::geometry as geometry,
                               (colour.b24 >> 16) % 256 || ' ' || (colour.b24 >> 8) % 256 || ' ' || colour.b24 % 256 AS colour_rgb
                           FROM dataset_files las
                           JOIN latest_dataset_files ON latest_dataset_files.file_id = las.id
                           JOIN LATERAL ( SELECT abs(uuid_hash(las.dataset_id)) % 16777216 AS b24 ) colour ON true
                           WHERE data_type ILIKE 'DatasetLasFile'
                         ) as subquery using unique fid using srid=4326"
                   FILTER "%org%"
                   FILTERITEM "organization"
                   VALIDATION
                     'org'         '^[a-zA-Z\-]+$'
                     'default_org' 'Development'
                   END # VALIDATION
                   PROJECTION
                     "init=epsg:4326"
                   END # PROJECTION
                   CLASS
                     STYLE
                       COLOR [colour_rgb]
                       OPACITY 50
                     END # STYLE
                     STYLE
                       OUTLINECOLOR [colour_rgb]
                       WIDTH 3
                     END # STYLE
                   END # CLASS
                 END # LAYER
               END # MAP
               """));
    }
}
