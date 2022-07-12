using Cerealizer.Tests.TestObjects;
using FluentAssertions;
using Newtonsoft.Json;

namespace Cerealizer.Tests;

public class MissingTypeMemberHandlingTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void WhenTheTargetTypeIsMissingAPropertyAnExceptionIsThrown()
    {
        var product = new Product
        {
            Name = "Apple",
            ExpiryDate = new DateTime(2008, 12, 28),
            Price = 3.99M,
            Sizes = new[] { "Small", "Medium", "Large" }
        };

        var output = JsonConvert.SerializeObject(product, Formatting.Indented);
        //{
        //  "Name": "Apple",
        //  "ExpiryDate": new Date(1230422400000),
        //  "Price": 3.99,
        //  "Sizes": [
        //    "Small",
        //    "Medium",
        //    "Large"
        //  ]
        //}

        ProductLong result = null;
        Action deserializeObject = () =>
            result = (ProductLong)JsonConvertStrict.DeserializeObject(output, typeof(ProductLong),
                new StrictJsonSerializerSettings());
        deserializeObject.Should().Throw<JsonSerializationException>().WithMessage(
            $"Could not find property 'MyPrice' on object of type '{nameof(ProductLong)}' in JSON payload.");

        result.Should().BeNull();
    }

    public void WhenTheTargetTypeIsNotMissingAPropertyNoExceptionIsThrown()
    {
        var product = new Product
        {
            Name = "Apple",
            ExpiryDate = new DateTime(2008, 12, 28),
            Price = 3.99M,
            Sizes = new[] { "Small", "Medium", "Large" }
        };

        var output = JsonConvert.SerializeObject(product, Formatting.Indented);
        //{
        //  "Name": "Apple",
        //  "ExpiryDate": new Date(1230422400000),
        //  "Price": 3.99,
        //  "Sizes": [
        //    "Small",
        //    "Medium",
        //    "Large"
        //  ]
        //}

        Product result = null;
        Action deserializeObject = () =>
            result = (Product)JsonConvertStrict.DeserializeObject(output, typeof(Product),
                new StrictJsonSerializerSettings());
        deserializeObject.Should().NotThrow();

        result.Should().NotBeNull();
    }
}