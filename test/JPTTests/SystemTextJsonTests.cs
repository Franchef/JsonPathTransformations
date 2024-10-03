﻿using AutoFixture.Xunit2;
using JPT;
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

        [Fact]
        public void BookToAuthor()
        {
            /// Arrange
            var json = System.IO.File.ReadAllText(@"TestCases\01_PropertyRename\source.json");
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
    }
}