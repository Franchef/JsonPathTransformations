using FakeItEasy;
using JPT;

using Xunit.Abstractions;

namespace JPTTests
{
    public class JPTTests(ITestOutputHelper testOutputHelper)
    {
        [Theory]
        [InlineData("12345")]
        [InlineData(12345)]
        public void RegexValidator_ExpectsPropertyIsValid(object propertyValue)
        {
            /// Arrange
            var jsonPathExtractor = A.Dummy<IJsonPathExtractor>();
            A.CallTo(() => jsonPathExtractor.Extracts(A<string>.Ignored)).Returns(propertyValue);
            IJsonPropertyValidator sut = new JsonPropertyRegexValidator(jsonPathExtractor)
            {
                PropertyPath = "$.name",
                ReGexPattern = "[0-9]+"
            };

            /// Act
            var result = sut.PropertyIsValid();
            /// Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData(12345)]
        public void RegexValidator_ExpectsPropertyIsNotValid(object propertyValue)
        {
            /// Arrange
            var jsonPathExtractor = A.Dummy<IJsonPathExtractor>();
            A.CallTo(() => jsonPathExtractor.Extracts(A<string>.Ignored)).Returns(propertyValue);
            IJsonPropertyValidator sut = new JsonPropertyRegexValidator(jsonPathExtractor)
            {
                PropertyPath = "$.name",
                ReGexPattern = "[a-zA-Z]+"
            };

            /// Act
            var result = sut.PropertyIsValid();
            /// Assert
            Assert.False(result);
        }
    }
}