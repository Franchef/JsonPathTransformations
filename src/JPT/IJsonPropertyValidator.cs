using System.Text.RegularExpressions;

namespace JPT
{
    public interface IJsonPropertyValidator
    {
        public  string PropertyPath { get; init; }
        bool PropertyIsValid();
    }

    public class JsonPropertyRegexValidator : IJsonPropertyValidator
    {
        private readonly IJsonPathExtractor _jsonPathExtractor;

        public JsonPropertyRegexValidator(IJsonPathExtractor jsonPathExtractor)
        {
            _jsonPathExtractor=jsonPathExtractor;
        }

        public required string PropertyPath { get; init; }
        public required string ReGexPattern { get; init; }

        public bool PropertyIsValid()
        {
            var regex = new Regex(ReGexPattern);
            var value = $"{_jsonPathExtractor.Extracts(PropertyPath)}";
            return regex.IsMatch(value);
        }
    }
}
