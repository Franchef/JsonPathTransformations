using System.Text.Json;
using System.Text.Json.Nodes;

namespace JPT.SystemTextJson
{
    public class TextJsonJsonPathExtractor : JPT.IJsonPathExtractor
    {
        private readonly JsonNode _source;

        public TextJsonJsonPathExtractor(JsonNode source)
        {
            _source=source;
        }
        public object Extracts(string jsonPath)
        {
            return GetPropertyValue(jsonPath.Split('.').Skip(1), _source.Deserialize<JsonNode>());
        }

        private static string GetPropertyValue(IEnumerable<string> propertyPath, JsonNode node)
        {
            var property = propertyPath.First();
            if (propertyPath.Skip(1).Any())
                return GetPropertyValue(propertyPath.Skip(1), node[property]);
            else
                return node[property].GetValue<string>();
        }
    }
}
