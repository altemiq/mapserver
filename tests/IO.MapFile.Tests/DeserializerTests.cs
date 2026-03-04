namespace Altemiq.IO.MapFile.Tests;

using System.Runtime.CompilerServices;

public class DeserializerTests
{

    [Test]
    public async Task Parse_Comments_AreIgnored()
    {
        var src =
            """
            /* Global comment */
            MAP
              # inline comment
              NAME "World"
              SIZE 800 600
              EXTENT -137 29 -53 88
              UNITS DD
              PROJECTION
                "init=epsg:4326"
              END

              LAYER
                NAME "admin_countries"
                STATUS ON
                TYPE POLYGON
                DATA "ne_10m_admin_0_countries"
                CLASS
                  NAME "Countries"
                  STYLE
                    COLOR 246 241 223
                  END
                  STYLE
                    OUTLINECOLOR 0 0 0
                    WIDTH 1
                  END
                END
              END
            END
            """;

        var map = Serialization.MapfileSerializer.Deserialize(src);

        await Assert.That(map.Name).IsEqualTo("World");
        await Assert.That(map.Size.Width).IsEqualTo(800);
        await Assert.That(map.Size.Height).IsEqualTo(600);
        await Assert.That(map.Units).IsEqualTo(MapUnits.DD);

        await Assert.That(map.Projection).IsNotNull();
        await Assert.That(map.Projection!.Parameters).Contains("init=epsg:4326");

        await Assert.That(map.Layers.Count).IsEqualTo(1);
        var layer = map.Layers[0];

        using (Assert.Multiple())
        {
            await Assert.That(layer.Name).IsEqualTo("admin_countries");
            await Assert.That(layer.Status).IsEqualTo(MapStatus.On);
            await Assert.That(layer.Type).IsEqualTo(LayerType.Polygon);
            await Assert.That(layer.Data).IsEqualTo("ne_10m_admin_0_countries");
            await Assert.That(layer.Classes.Count).IsEqualTo(1);
            await Assert.That(layer.Classes[0].Styles.Count).IsEqualTo(2);
        }
    }

    [Test]
    public async Task RoundTrip_SerializeParse_IsStableOnStructure()
    {
        var src =
            """
            MAP
              NAME "MS"
              SIZE 256 256
              IMAGETYPE "png"
              PROJECTION
                "init=epsg:3857"
              END

              OUTPUTFORMAT
                NAME "png"
                DRIVER AGG/PNG
                MIMETYPE "image/png"
                IMAGEMODE RGB
                EXTENSION "png"
                FORMATOPTION "GAMMA=0.75"
              END

              LAYER
                NAME "states"
                TYPE POLYGON
                DATA "states"
                CLASS
                  NAME "Default"
                  STYLE
                    COLOR 200 200 200
                    OUTLINECOLOR 80 80 80
                  END
                END
              END
            END
            """;

        var map = Serialization.MapfileSerializer.Deserialize(src);
        var text1 = Serialization.MapfileSerializer.Serialize(map);
        var map2 = Serialization.MapfileSerializer.Deserialize(text1);
        var text2 = Serialization.MapfileSerializer.Serialize(map2);

        await Assert.That(TestHelpers.StripWhitespace(text2))
            .IsEqualTo(TestHelpers.StripWhitespace(text1));
    }

    [Test]
    public async Task Parse_Metadata_And_ConnectionOptions()
    {
        var src =
            """
            MAP
              SIZE 256 256

              LAYER
                NAME "geojson"
                TYPE POLYGON
                CONNECTIONTYPE OGR
                CONNECTION "/data/cities.geojson"
                CONNECTIONOPTIONS
                  "FLATTEN_NESTED_ATTRIBUTES" "YES"
                END
                METADATA
                  "wms_title" "Cities"
                END
                CLASS
                  STYLE
                    COLOR 255 0 0
                  END
                END
              END
            END
            """;

        var map = Serialization.MapfileSerializer.Deserialize(src);
        var layer = map.Layers[0];

        using (Assert.Multiple())
        {
            await Assert.That(layer.ConnectionType).IsEqualTo(ConnectionType.Ogr);
            await Assert.That(layer.Connection).IsEqualTo("/data/cities.geojson");
            await Assert.That(layer.ConnectionOptions).ContainsKey("FLATTEN_NESTED_ATTRIBUTES");
            await Assert.That(layer.ConnectionOptions["FLATTEN_NESTED_ATTRIBUTES"]).IsEqualTo("YES");
            await Assert.That(layer.Metadata["wms_title"]).IsEqualTo("Cities");
        }
    }

    [Test]
    public async Task Parse_Full_Wms()
    {
        var map = Serialization.MapfileSerializer.Deserialize(
            """
                 MAP
                   NAME "WMS"
                   STATUS ON
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
                       "wms_title"          "AIMS Clearance Map"
                       "wms_onlineresource" "${PROXY_HOST_URL}/maps/wms"
                       "wms_srs"            "EPSG:4326 EPSG:4269 EPSG:3978 EPSG:3857"
                       "wms_enable_request" "*"
                       "wms_feature_info_mime_type" "text/html"
                       "tile_map_edge_buffer" "8"
                     END
                   END # Web
                   
                   CONFIG "MS_ERRORFILE" "stderr"
                   OUTPUTFORMAT
                     NAME "png24"
                     MIMETYPE "image/png"
                     DRIVER "AGG/PNG"
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
                   
                   #
                   # Start of layer definitions
                   #
                   
                   ##################
                   # Dataset Las Boundaries
                   ##################
                   LAYER
                     NAME "dataset_las_boundary"
                     CONNECTIONTYPE POSTGIS
                     CONNECTION "${CONNECTIONSTRINGS__POSTGRES__CATALOG}"
                     PROCESSING "CLOSE_CONNECTION=DEFER"
                     STATUS ON
                     METADATA
                       "wms_title"	        "dataset_las_boundary"
                       "wms_srs"	            "EPSG:4326"
                       "wms_enable_request"  "*"
                       "gml_include_items"	"all"
                       "gml_featureid"       "fid"
                       "gml_types"           "auto"
                       "ows_title"	        "dataset_las_boundary"
                       "ows_enable_request"  "*"
                       "ows_include_items"	"all" 
                       "ows_geometry_type"	"polygon"
                       "ows_geometries"  	"geometry"
                     END
                     TYPE POLYGON
                     TOLERANCE 10
                     TOLERANCEUNITS pixels
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
                       # %org% substitutions can only have letters and hyphens
                       'org' '^[a-zA-Z\-]+$'
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
                 END # mapfile
                 """);

        await Assert.That(map.Name).IsEqualTo("WMS");
        await Assert.That(map.Layers.First().Classes.First().Styles.First().Color).IsEqualTo(new Attribute("colour_rgb"));
    }

}