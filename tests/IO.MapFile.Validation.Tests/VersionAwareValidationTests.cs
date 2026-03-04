namespace Altemiq.IO.MapFile.Validation.Tests;

public class VersionAwareValidationTests
{
    [Test]
    [Arguments(7, 4, true)]   // 7.4 -> expect error
    [Arguments(8, 0, false)]  // 8.0 -> expect OK
    public async Task ConnectionOptions_Respect_Version(int major, int minor, bool expectError)
    {
        var layer = new Layer
        {
            Name = "ogr_layer",
            Type = LayerType.Polygon,
            ConnectionType = ConnectionType.Ogr,
            Connection = "/data/cities.geojson",
            ConnectionOptions =
            {
                ["FLATTEN_NESTED_ATTRIBUTES"] = "YES"
            },
        };

        var map = new Map { Size = new(256, 256), ImageType = System.Net.Mime.MediaTypeNames.Image.Png };
        map.Layers.Add(layer);

        var result = map.Validate(major, minor);

        if (expectError)
        {
            await Assert.That(result.IsValid).IsFalse();
            await Assert.That(result.ToString()).Contains("CONNECTIONOPTIONS");
        }
        else
        {
            await Assert.That(result.IsValid).IsTrue();
        }
    }

    // --- SDE removal in 7.0 ---

    [Test]
    [Arguments(6, 4, true)]   // pre-7.0 -> SDE allowed
    [Arguments(7, 0, false)]  // 7.0+ -> SDE should error
    [Arguments(8, 0, false)]  // 8.0+ -> still error
    public async Task Sde_NativeDriver_Rules_By_Version(int major, int minor, bool shouldBeValid)
    {
        var layer = new Layer
        {
            Name = "sde_layer",
            Type = LayerType.Polygon,
            ConnectionType = ConnectionType.Sde,
            Connection = "host,instance,db,user,pwd"
        };

        var map = new Map { Size = new(256, 256), ImageType = System.Net.Mime.MediaTypeNames.Image.Png };
        map.Layers.Add(layer);

        var result = map.Validate(major, minor);

        if (shouldBeValid)
        {
            await Assert.That(result.IsValid).IsTrue();
        }
        else
        {
            await Assert.That(result.IsValid).IsFalse();
            await Assert.That(result.ToString()).Contains("Native SDE driver was removed in MapServer 7.0");
        }
    }

    // --- Integration: both rules at once ---

    [Test]
    [Arguments(7, 4)] // 7.4 -> both errors (CONNECTIONOPTIONS + SDE)
    [Arguments(8, 0)] // 8.0 -> only SDE error
    public async Task Mixed_Features_Validation_By_Version(int major, int minor)
    {
        var ogr = new Layer
        {
            Name = "ogr_layer",
            Type = LayerType.Polygon,
            ConnectionType = ConnectionType.Ogr,
            Connection = "/data/cities.geojson"
        };
        ogr.ConnectionOptions["FLATTEN_NESTED_ATTRIBUTES"] = "YES";

        var sde = new Layer
        {
            Name = "sde_layer",
            Type = LayerType.Polygon,
            ConnectionType = ConnectionType.Sde,
            Connection = "host,instance,db,user,pwd"
        };

        var map = new Map { Size = new(256, 256) };
        map.Layers.Add(ogr);
        map.Layers.Add(sde);

        var result = map.Validate(major, minor);

        if (major is 7 && minor is 4)
        {
            // 7.4: both features invalid
            await Assert.That(result.IsValid).IsFalse();
            await Assert.That(result.ToString()).Contains("CONNECTIONOPTIONS");
            await Assert.That(result.ToString()).Contains("Native SDE driver was removed in MapServer 7.0");
        }
        else
        {
            // 8.0: CONNECTIONOPTIONS OK, SDE still invalid
            await Assert.That(result.IsValid).IsFalse();
            await Assert.That(result.ToString()).DoesNotContain("CONNECTIONOPTIONS");
            await Assert.That(result.ToString()).Contains("Native SDE driver was removed in MapServer 7.0");
        }
    }

}
