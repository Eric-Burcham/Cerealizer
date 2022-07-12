namespace Cerealizer.Tests
{
    using FluentAssertions;

    using Newtonsoft.Json;

    using TestObjects;

    public class MissingTypeMemberHandlingTests
    {
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
                result = (ProductLong)StrictJsonConvert.DeserializeObject(
                    output,
                    typeof(ProductLong),
                    new StrictJsonSerializerSettings());

            deserializeObject.Should().Throw<JsonSerializationException>().WithMessage($"Could not find these properties on object of type '{typeof(ProductLong).FullName}' in JSON payload:  MyPrice.");

            result.Should().BeNull();
        }

        [Test]
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
                result = (Product)StrictJsonConvert.DeserializeObject(
                    output,
                    typeof(Product),
                    new StrictJsonSerializerSettings());

            deserializeObject.Should().NotThrow();

            result.Should().NotBeNull();
        }
    }
}
