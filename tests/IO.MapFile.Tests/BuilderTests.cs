namespace Altemiq.IO.MapFile.Tests;

public class BuilderTests_TUnit
{
    [Test]
    public async Task MapBuilder_CreatesGraph_AsExpected()
    {
        var map = Builders.MapBuilder.New()
            .Name("World")
            .Size(800, 600)
            .Extent(-137, 29, -53, 88)
            .Units(MapUnits.DD)
            .ImageType("png")
            .AddProjection(b => b.Epsg(4326))
            .UseWeb(w => w
                .ImagePath("/var/www/tmp/")
                .ImageUrl("/tmp/")
                .Metadata("ows_enable_request", "*"))
            .AddOutputFormat(of => of
                .Name("png")
                .Driver("AGG/PNG")
                .MimeType(System.Net.Mime.MediaTypeNames.Image.Png)
                .ImageMode("RGB"))
            .AddLayer(l => l
                .Name("admin_countries")
                .Status(MapStatus.On)
                .Type(LayerType.Polygon)
                .Data("ne_10m_admin_0_countries")
                .AddClass(cls => cls
                    .Name("Countries")
                    .AddStyle(s => s.Color(246, 241, 223))
                    .AddStyle(s => s.OutlineColor(0, 0, 0).Width(1))))
            .Build();

        using (Assert.Multiple())
        {
            await Assert.That(map.Name).IsEqualTo("World");
            await Assert.That(map.Projection!.Parameters).Contains("init=epsg:4326");
            await Assert.That(map.Web!.Metadata["ows_enable_request"]).IsEqualTo("*");
            await Assert.That(map.OutputFormats).Contains(of => of.Name is "png" && of.Driver is "AGG/PNG");
            await Assert.That(map.Layers).Contains(l => l.Name is "admin_countries");
        }
    }

    [Test]
    public async Task Builder_ThenSerializer_ProducesValidMapfile()
    {
        var map = Builders.MapBuilder.New()
            .Name("MapFromBuilder")
            .Size(256, 256)
            .AddProjection(p => p.Epsg(3857))
            .AddLayer(l => l
                .Name("states")
                .Status(MapStatus.On)
                .Type(LayerType.Line)
                .Data("states_line")
                .AddClass(c => c.AddStyle(s => s.Color(System.Drawing.Color.Black).Width(1))))
            .Build();

        var n = TestHelpers.NormalizeMapfile(Serialization.MapfileSerializer.Serialize(map));

        using (Assert.Multiple())
        {
            await Assert.That(n).Contains("MAP");
            await Assert.That(n).Contains("LAYER");
            await Assert.That(n).Contains("TYPE LINE");
            await Assert.That(n).Contains("DATA \"states_line\"");
        }
    }
}