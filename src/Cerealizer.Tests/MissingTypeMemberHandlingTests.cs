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
    public void MissingMemberDeserialize()
    {
        Product product = new Product();

        product.Name = "Apple";
        product.ExpiryDate = new DateTime(2008, 12, 28);
        product.Price = 3.99M;
        product.Sizes = new string[] { "Small", "Medium", "Large" };

        string output = JsonConvert.SerializeObject(product, Formatting.Indented);
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
        Action deserializeObject = () => result = (ProductLong)JsonConvertStrict.DeserializeObject(output, typeof(ProductLong), new StrictJsonSerializerSettings());
        deserializeObject.Should().Throw<JsonSerializationException>().WithMessage(
            $"Could not find property 'MyPrice' on object of type '{nameof(ProductLong)}' in JSON payload.");

        result.Should().BeNull();
    }
}