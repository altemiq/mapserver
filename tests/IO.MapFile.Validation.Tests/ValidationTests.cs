namespace Altemiq.IO.MapFile.Validation.Tests;

public class ValidationTests
{
    [Test]
    public async Task MapValidator_SizeAndExtent_Rules()
    {
        var m = new Map
        {
            Name = "Invalid",
            Size = new(0, -1),
            Extent = new BoundingBox(10, 10, 0, 0)
        };

        var result = new MapValidator().Validate(m);

        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors)
            .Contains(e => e.ErrorMessage.Contains("SIZE width must be > 0"))
            .Contains(e => e.ErrorMessage.Contains("EXTENT must satisfy"));
    }

    [Test]
    public async Task MapValidator_WarnsWhenNoImageTypeOrOutputFormat()
    {
        var m = new Map { Size = new(256, 256) };

        var result = new MapValidator().Validate(m);

        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors)
            .DoesNotContain(e => e.Severity is Severity.Error)
            .Contains(e => e.ErrorMessage.Contains("Neither IMAGETYPE nor OUTPUTFORMAT"));
    }

    [Test]
    public async Task LayerValidator_RequiresDataOrConnection()
    {
        var layer = new Layer
        {
            Name = "Cities",
            Type = LayerType.Point,
            Status = MapStatus.On
        };

        var r = new LayerValidator().Validate(layer);

        await Assert.That(r.IsValid).IsFalse();
        await Assert.That(r.Errors).Contains( e => e.ErrorMessage.Contains("Specify DATA or (CONNECTIONTYPE + CONNECTION)"));
    }

    [Test]
    public async Task LayerValidator_AllowsDataOrConnection()
    {
        var l1 = new Layer { Name = "WithData", Type = LayerType.Polygon, Data = "countries" };
        var l2 = new Layer { Name = "WithConn", Type = LayerType.Raster, ConnectionType = ConnectionType.Wms, Connection = "https://example/wms" };

        await Assert.That(new LayerValidator().Validate(l1).IsValid).IsTrue();
        await Assert.That(new LayerValidator().Validate(l2).IsValid).IsTrue();
    }

    [Test]
    public async Task LayerValidator_ProcessingWarnsOnNonKeyValue()
    {
        var layer = new Layer { Name = "Proc", Type = LayerType.Polygon, Data = "x" };
        layer.Processing.Add("SOMEFLAG"); // no '='

        var r = new LayerValidator().Validate(layer);

        await Assert.That(r.IsValid).IsFalse();
        await Assert.That(r.Errors)
            .DoesNotContain(e => e.Severity is FluentValidation.Severity.Error)
            .Contains(e => e.ErrorMessage.Contains("PROCESSING entry should be"));
    }

    [Test]
    public async Task StyleValidator_OpacityRange()
    {
        var style = new Style { Opacity = 120 };
        var r = new StyleValidator().Validate(style);

        await Assert.That(r.IsValid).IsFalse();
        await Assert.That(r.Errors).Contains(e => e.ErrorMessage.Contains("OPACITY must be between 0 and 100"));
    }

    [Test]
    public async Task LabelValidator_SizesAndPriorities()
    {
        var label = new Label { Size = "0", MinDistance = -5, MinFeatureSize = -1 };
        var r = new LabelValidator().Validate(label);
        await Assert.That(r.IsValid).IsFalse();
        await Assert.That(r.Errors).Contains(e => e.ErrorMessage.Contains("must be"));
    }

    [Test]
    public async Task FullMap_ValidateWithFluent_ExtensionHelpers()
    {
        var map = new Map
        {
            Name = "OK",
            Size = new(256, 256),
            ImageType = "png",
            Projection = new Projection { Parameters = { "init=epsg:4326" } }
        };
        map.Layers.Add(new Layer
        {
            Name = "states",
            Type = LayerType.Polygon,
            Data = "states",
            Classes = { new Class { Styles = { new Style { Color = System.Drawing.Color.FromArgb(200, 200, 200) } } } }
        });

        await Assert.That(map.Validate().IsValid).IsTrue();
        await Assert.That(map.ThrowIfInvalid).ThrowsNothing();
    }
}