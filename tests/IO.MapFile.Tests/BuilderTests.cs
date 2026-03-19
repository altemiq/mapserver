namespace Altemiq.IO.MapFile.Tests;

public class BuilderTests_TUnit
{
    [Test]
    public async Task MapBuilder_CreatesGraph_AsExpected()
    {
        var map = new Builders.MapBuilder()
            .WithName("World")
            .WithSize(800, 600)
            .WithExtent(-137, 29, -53, 88)
            .WithUnits(MapUnits.DD)
            .WithImageType("png")
            .WithProjection(builder => builder.WithEpsg(4326))
            .WithWeb(builder => builder
                .WithImagePath("/var/www/tmp/")
                .WithImageUrl("/tmp/")
                .AddMetadata("ows_enable_request", "*"))
            .AddOutputFormat(builder => builder
                .WithName("png")
                .WithDriver("AGG/PNG")
                .WithMimeType(System.Net.Mime.MediaTypeNames.Image.Png)
                .WithImageMode("RGB"))
            .AddLayer(builder => builder
                .WithName("admin_countries")
                .WithStatus(MapStatus.On)
                .WithType(LayerType.Polygon)
                .WithData("ne_10m_admin_0_countries")
                .AddClass(builder => builder
                    .WithName("Countries")
                    .AddStyle(builder => builder.WithColor(System.Drawing.Color.FromArgb(246, 241, 223)))
                    .AddStyle(builder => builder.WithOutlineColor(System.Drawing.Color.FromArgb(0, 0, 0)).WithWidth(1))))
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
        var map = new Builders.MapBuilder()
            .WithName("MapFromBuilder")
            .WithSize(256, 256)
            .WithProjection(builder => builder.WithEpsg(3857))
            .AddLayer(builder => builder
                .WithName("states")
                .WithStatus(MapStatus.On)
                .WithType(LayerType.Line)
                .WithData("states_line")
                .AddClass(builder => builder
                    .AddStyle(builder =>
                    {
                        var withColor = builder.WithColor(System.Drawing.Color.Black);
                        var withWidth = withColor.WithWidth(1);
                    })))
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