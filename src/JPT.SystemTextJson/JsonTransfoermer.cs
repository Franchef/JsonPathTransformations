using System.Text.Json;
using System.Text.Json.Nodes;

namespace JPT.SystemTextJson
{
    public class JsonTransfoermer : JPT.JsonTransformer
    {
        public override string Transform(string text)
        {
            var source = JsonDocument.Parse(text)!;
            var destination = JsonNode.Parse("{ }")!;

            foreach (var transformation in _transformations)
            {
                var value = source.RootElement.GetProperty(transformation.Key);

                var pathParts = transformation.Value.Split('.');

                JsonNode destinationProperty = destination;

                foreach (var property in pathParts)
                {
                    destinationProperty = destinationProperty[property];
                }

                destinationProperty = value.GetRawText();
            }

            return destination.ToString();
        }
    }
}
