namespace Altemiq.IO.MapFile.Tests;

public class BuilderTests_TUnit
{
    [Test]
    public async Task MapBuilder_CreatesGraph_AsExpected()
    {
        var map = Builders.MapBuilder.New()
            .WithName("World")
            .WithSize(800, 600)
            .WithExtent(-137, 29, -53, 88)
            .WithUnits(MapUnits.DD)
            .WithImageType("png")
            .WithProjection(() => Builders.ProjectionBuilder.New().WithEpsg(4326).Build())
            .WithWeb(() => Builders.WebBuilder.New()
                .WithImagePath("/var/www/tmp/")
                .WithImageUrl("/tmp/")
                .WithMetadata(() => new Dictionary<string, string>() { { "ows_enable_request", "*" } })
                .Build())
            .WithOutputFormats(() =>
            [
                Builders.OutputFormatBuilder.New()
                    .WithName("png")
                    .WithDriver("AGG/PNG")
                    .WithMimeType(System.Net.Mime.MediaTypeNames.Image.Png)
                    .WithImageMode("RGB")
                    .Build()
                ])
            .WithLayers(
            [
                Builders.LayerBuilder.New()
                .WithName("admin_countries")
                .WithStatus(MapStatus.On)
                .WithType(LayerType.Polygon)
                .WithData("ne_10m_admin_0_countries")
                .WithClasses(
                [
                    Builders.ClassBuilder.New()
                        .WithName("Countries")
                        .WithStyles(() =>
                        [
                            Builders.StyleBuilder.New().WithColor(246, 241, 223),
                            Builders.StyleBuilder.New().WithOutlineColor(0, 0, 0).WithWidth(1)
                            ])
                    ])
                ])
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
            .WithName("MapFromBuilder")
            .WithSize(256, 256)
            .WithProjection(() => Builders.ProjectionBuilder.New().WithEpsg(3857).Build())
            .WithLayers(() =>
            [
                Builders.LayerBuilder
                    .New()
                    .WithName("states")
                    .WithStatus(MapStatus.On)
                    .WithType(LayerType.Line)
                    .WithData("states_line")
                    .WithClasses(() =>
                    [
                        Builders.ClassBuilder
                            .New()
                            .WithStyles(() =>
                            [
                                Builders.StyleBuilder.New()
                                    .WithColor(System.Drawing.Color.Black)
                                    .WithWidth(1)
                                    ]
                            )
                        ])
                    ])
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