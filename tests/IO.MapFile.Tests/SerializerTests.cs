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
            .Extent(-180, -90, 180, 90)
            .ImageColor(System.Drawing.Color.White)
            .UseWeb(web =>
                web
                    .Metadata("ows_title", "AIMS Clearance Map")
                    .Metadata("ows_onlineresource", "${PROXY_HOST_URL}/maps/wfs")
                    .Metadata("ows_srs", "EPSG:4326 EPSG:4269 EPSG:3978 EPSG:3857")
                    .Metadata("ows_enable_request", "*"))
            .AddProjection(p => p.Epsg(4326))
            .AddLayer(layer =>
                layer
                    .Group("catalog")
                    .Name("dataset_las_boundary")
                    .ConnectionType(ConnectionType.PostGIS)
                    .Connection("${CONNECTIONSTRINGS__POSTGRES__CATALOG}")
                    .Processing("CLOSE_CONNECTION", "DEFER")
                    .Status(MapStatus.On)
                    .Metadata("wfs_enable_request", "*")
                    .Metadata("wfs_title", "dataset_las_boundary")
                    .Metadata("wfs_srs", "EPSG:4326")
                    .Metadata("gml_include_items", "all")
                    .Metadata("gml_featureid", "file_id")
                    .Metadata("gml_types", "auto")
                    .Type(LayerType.Polygon)
                    .Data(
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
                    .Filter("%org%")
                    .FilterItem("organization")
                    .Validation("org", "^[a-zA-Z\\-]+$")
                    .Validation("default_org", "Development")
                    .AddProjection(p => p.Epsg(4326)))
            .AddLayer(layer =>
                layer
                    .Group("catalog")
                    .Name("dataset_tif_boundary")
                    .ConnectionType(ConnectionType.PostGIS)
                    .Connection("${CONNECTIONSTRINGS__POSTGRES__CATALOG}")
                    .Processing("CLOSE_CONNECTION", "DEFER")
                    .Status(MapStatus.On)
                    .Metadata("wfs_enable_request", "*")
                    .Metadata("wfs_title", "dataset_tif_boundary")
                    .Metadata("wfs_srs", "EPSG:4326")
                    .Metadata("gml_include_items", "all")
                    .Metadata("gml_featureid", "file_id")
                    .Metadata("gml_types", "auto")
                    .Type(LayerType.Polygon)
                    .Data(
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
                    .Filter("%org%")
                    .FilterItem("organization")
                    .Validation("org", "^[a-zA-Z\\-]+$")
                    .Validation("default_org", "Development")
                    .AddProjection(p => p.Epsg(4326)))
            .AddLayer(layer =>
                layer
                    .Group("review")
                    .Name("dataset_review_region")
                    .ConnectionType(ConnectionType.PostGIS)
                    .Connection("${CONNECTIONSTRINGS__POSTGRES__REVIEW}")
                    .Processing("CLOSE_CONNECTION", "DEFER")
                    .Status(MapStatus.On)
                    .Metadata("wfs_enable_request", "*")
                    .Metadata("wfs_title", "dataset_review_region")
                    .Metadata("wfs_srs", "EPSG:4326")
                    .Metadata("gml_include_items", "all")
                    .Metadata("gml_featureid", "review_id")
                    .Metadata("gml_types", "auto")
                    .Type(LayerType.Polygon)
                    .Data(
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
                    .Filter("%org%")
                    .FilterItem("organization")
                    .Validation("org", "^[a-zA-Z\\-]+$")
                    .Validation("default_org", "Development")
                    .AddProjection(p => p.Epsg(4326)));

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
            .Name("WMS")
            .Size(1224, 683)
            .Units(MapUnits.Meters)
            .ImageColor(System.Drawing.Color.White)
            .FontSet("/etc/mapserver/styles/fonts/fonts.txt")
            .SymbolSet("/etc/mapserver/styles/symbols/symbols.sym")
            .ImageType("png24")
            .MaxSize(4096)
            .UseWeb(web =>
                web
                    .FooterTemplate("/etc/mapserver/styles/templates/footer.html")
                    .HeaderTemplate("/etc/mapserver/styles/templates/header.html")
                    .ImagePath("/ms4w/tmp/ms_tmp/")
                    .ImageUrl("/ms_tmp/")
                    .Metadata("wms_title", "AIMS Clearance Map")
                    .Metadata("wms_onlineresource", "${PROXY_HOST_URL}/maps/wms")
                    .Metadata("wms_srs", "EPSG:4326 EPSG:4269 EPSG:3978 EPSG:3857")
                    .Metadata("wms_enable_request", "*")
                    .Metadata("wms_feature_info_mime_type", "text/html")
                    .Metadata("tile_map_edge_buffer", "8"))
            .Config("MS_ERRORFILE", "stderr")
            .AddOutputFormat(outputFormat =>
                outputFormat
                    .Name("png24")
                    .MimeType(System.Net.Mime.MediaTypeNames.Image.Png)
                    .Driver("AGG/PNG")
                    .Extension("png")
                    .ImageMode("RGB")
                    .Transparent(true))
            .AddProjection(projection =>
                projection
                    .Line("proj=longlat")
                    .Line("ellps=GRS80")
                    .Line("towgs84=0,0,0,0,0,0,0")
                    .Line("no_defs"))
            .UseLegend(legend =>
                legend
                    .KeySize(20, 10)
                    .KeySpacing(5, 5)
                    .Label(label =>
                        label
                            .Size("MEDIUM")
                            .Offset(0, 0)
                            .ShadowSize(1, 1)
                            .Type("BITMAP")))
            .UseQueryMap(queryMap =>
                queryMap
                    .Size(-1, -1)
                    .Status(MapStatus.Off)
                    .Style(QueryMapStyle.Hilite))
            .UseScaleBar(scaleBar =>
                scaleBar
                    .Intervals(4)
                    .Label(label =>
                        label
                            .Size("MEDIUM")
                            .Offset(0, 0)
                            .ShadowSize(1, 1)
                            .Type("BITMAP"))
                    .Size(200, 3)
                    .Status(MapStatus.Off)
                    .Units(MapUnits.Miles))
            .AddLayer(layer =>
                layer
                    .Name("dataset_las_boundary")
                    .ConnectionType(ConnectionType.PostGIS)
                    .Connection("${CONNECTIONSTRINGS__POSTGRES__CATALOG}")
                    .Processing("CLOSE_CONNECTION", "DEFER")
                    .Status(MapStatus.On)
                    .Metadata("wms_title", "dataset_las_boundary")
                    .Metadata("wms_srs", "EPSG:4326")
                    .Metadata("wms_enable_request", "*")
                    .Metadata("gml_include_items", "all")
                    .Metadata("gml_featureid", "fid")
                    .Metadata("gml_types", "auto")
                    .Metadata("ows_title", "dataset_las_boundary")
                    .Metadata("ows_enable_request", "*")
                    .Metadata("ows_include_items", "all")
                    .Metadata("ows_geometry_type", "polygon")
                    .Metadata("ows_geometries", "geometry")
                    .Type(LayerType.Polygon)
                    .Tolerance(10)
                    .ToleranceUnits(MapUnits.Pixels)
                    .Template("/etc/mapserver/styles/templates/file.html")
                    .Units(MapUnits.Meters)
                    .Data(
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
                    .Filter("%org%")
                    .FilterItem("organization")
                    .Validation("org", "^[a-zA-Z\\-]+$")
                    .Validation("default_org", "Development")
                    .AddProjection(projection => projection.Epsg(4326))
                    .AddClass(@class =>
                        @class
                            .AddStyle(style =>
                                style
                                    .Color(new Attribute("colour_rgb"))
                                    .Opacity(50))
                            .AddStyle(style =>
                                style
                                    .OutlineColor(new Attribute("colour_rgb"))
                                    .Width(3))));

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
