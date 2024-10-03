using AutoFixture.Xunit2;
using JPT;
using JPT.Newtonsoft;
using Newtonsoft.Json.Linq;

using Xunit.Abstractions;

namespace JPTTests
{
    public class NewtonsoftTests(ITestOutputHelper testOutputHelper)
    {
        [Theory]
        [AutoData]
        public void AddProperty(string field)
        {
            var jsonDocument = new JObject();
            jsonDocument = JPT.Newtonsoft.JsonTransformer.CreateProperty(jsonDocument, field);
            testOutputHelper.WriteLine(jsonDocument.ToString());
            Assert.Contains(field, jsonDocument.ToString());
        }

        [Theory]
        [InlineData(@"TestCases\01_PropertyRename\source.json")]
        public void BookToAuthor(string filePath)
        {
            /// Arrange
            var json = System.IO.File.ReadAllText(filePath);
            var configuration = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("$.book.author", "$.author")
            };

            IJsonTransformer sut = new JPT.Newtonsoft.JsonTransformer();
            sut.ConfigureTransformations(configuration);

            /// Act
            var output = sut.Transform(json);

            /// Assert
            Assert.Contains("\"author\": \"who wrote the book\"", output);
            testOutputHelper.WriteLine(output);
        }

        [Theory]
        [InlineData(@"TestCases\01_PropertyRename\source.json")]
        public void IJsonPathExtractor(string filePath)
        {
            /// Arrange
            var json = System.IO.File.ReadAllText(filePath);
            IJsonPathExtractor jsonPathExtractor = new NetwonsofJsonPathExtractor(JObject.Parse(json));

            /// Actvar 
            var value = jsonPathExtractor.Extracts("$.book.author");

            /// Assert
            Assert.Contains("who wrote the book", value.ToString());
        }
    }
}