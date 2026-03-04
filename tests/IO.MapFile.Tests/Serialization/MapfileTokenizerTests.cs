namespace Altemiq.IO.MapFile.Serialization.Tests;

public class MapfileTokenizerTests
{
    [Test]
    public async Task TokenizeWfs()
    {
        var tokenizer = new MapfileTokenizer(
            """
                 MAP

                   EXTENT -180 -90 180 90
                   
                   IMAGECOLOR 255 255 255
                   
                   #
                   # Start of web interface definition
                   #
                   WEB
                     METADATA
                       "ows_title"          "AIMS Clearance Map"
                       "ows_onlineresource" "${PROXY_HOST_URL}/maps/wfs"
                       "ows_srs"            "EPSG:4326 EPSG:4269 EPSG:3978 EPSG:3857"
                       "ows_enable_request" "*"
                     END
                   END
                   
                   PROJECTION
                     "init=epsg:4326"
                   END
                   
                   #
                   # Start of layer definitions
                   #
                   
                   ##################
                   # Dataset Las Boundary
                   ##################
                   LAYER
                     GROUP "catalog"
                     NAME "dataset_las_boundary"
                     CONNECTIONTYPE POSTGIS
                     CONNECTION "${CONNECTIONSTRINGS__POSTGRES__CATALOG}"
                     PROCESSING "CLOSE_CONNECTION=DEFER"
                     STATUS ON
                     METADATA
                       "wfs_enable_request"  "*"
                       "wfs_title"	          "dataset_las_boundary"
                       "wfs_srs"	            "EPSG:4326"
                       "gml_include_items"	  "all"
                       "gml_featureid"       "file_id"
                       "gml_types"           "auto"
                     END
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
                       # %org% substitutions can only have letters and hyphens
                       'org' '^[a-zA-Z\-]+$'
                       'default_org' 'Development'
                     END # VALIDATION
                   
                     PROJECTION
                       "init=epsg:4326"
                     END # PROJECTION
                   END # LAYER
                   
                   ##################
                   # Dataset Tif Boundary
                   ##################
                   LAYER
                     GROUP "catalog"
                     NAME "dataset_tif_boundary"
                     CONNECTIONTYPE POSTGIS
                     CONNECTION "${CONNECTIONSTRINGS__POSTGRES__CATALOG}"
                     PROCESSING "CLOSE_CONNECTION=DEFER"
                     STATUS ON
                     METADATA
                       "wfs_enable_request"  "*"
                       "wfs_title"	          "dataset_tif_boundary"
                       "wfs_srs"	            "EPSG:4326"
                       "gml_include_items"	  "all"
                       "gml_featureid"       "file_id"
                       "gml_types"           "auto"
                     END
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
                       # %org% substitutions can only have letters and hyphens
                       'org' '^[a-zA-Z\-]+$'
                       'default_org' 'Development'
                     END # VALIDATION
                   
                     PROJECTION
                       "init=epsg:4326"
                     END # PROJECTION
                   END # LAYER
                   
                   ##################
                   # Dataset Review Region
                   ##################
                   LAYER
                     GROUP "review"
                     NAME "dataset_review_region"
                     CONNECTIONTYPE POSTGIS
                     CONNECTION "${CONNECTIONSTRINGS__POSTGRES__REVIEW}"
                     PROCESSING "CLOSE_CONNECTION=DEFER"
                     STATUS ON
                     METADATA
                       "wfs_enable_request"  "*"
                       "wfs_title"	          "dataset_review_region"
                       "wfs_srs"	            "EPSG:4326"
                       "gml_include_items"	  "all"
                       "gml_featureid"       "review_id"
                       "gml_types"           "auto"
                     END
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
                       # %org% substitutions can only have letters and hyphens
                       'org' '^[a-zA-Z\-]+$'
                       'default_org' 'Development'
                     END # VALIDATION
                   
                     PROJECTION
                       "init=epsg:4326"
                     END # PROJECTION
                   END # LAYER
               END # MAP
               """);

        // test the tokenization
        await Assert.That(tokenizer.TryReadNext(out _)).IsTrue();
    }
}
