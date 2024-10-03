using AutoFixture.Xunit2;
using JPT;
using JPT.SystemTextJson;
using System.Text.Json.Nodes;

using Xunit.Abstractions;

namespace JPTTests
{
    public class SystemTextJsonTests(ITestOutputHelper testOutputHelper)
    {
        [Theory]
        [AutoData]
        public void AddProperty(string field)
        {
            var jsonDocument = JsonNode.Parse("{}");
            jsonDocument = JPT.SystemTextJson.JsonTransformer.CreateProperty(jsonDocument, field);
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

            IJsonTransformer sut = new JPT.SystemTextJson.JsonTransformer();
            sut.ConfigureTransformations(configuration);

            /// Act
            var output = sut.Transform(text: json);

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
            IJsonPathExtractor jsonPathExtractor = new TextJsonJsonPathExtractor(JsonNode.Parse(json));

            /// Actvar 
            var value = jsonPathExtractor.Extracts("$.book.author");

            /// Assert
            Assert.Contains("who wrote the book", value.ToString());
        }
    }
}