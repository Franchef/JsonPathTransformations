using Xunit.Abstractions;

namespace JPTTests
{
    public class SystemTextJsonTests(ITestOutputHelper testOutputHelper)
    {
        [Fact]
        public void BookToAuthor()
        {
            /// Arrange
            var json = System.IO.File.ReadAllText(@"TestCases\01_PropertyRename\source.json");
            var configuration = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("$.book.author", "$.author")
            };

            var sut = new JPT.SystemTextJson.JsonTransfoermer();
            sut.ConfigureTransformations(configuration);

            /// Act
            var output = sut.Transform(json);

            /// Assert
            Assert.Contains("author", output);
            testOutputHelper.WriteLine(output);
        }
    }
}