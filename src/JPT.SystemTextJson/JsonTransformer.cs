using System.Text.Json;
using System.Text.Json.Nodes;

namespace JPT.SystemTextJson
{
    public class JsonTransformer : JPT.JsonTransformer
    {
        public override string Transform(string text)
        {
            var source = JsonNode.Parse(text)!;
            return Transform(source).ToString();
        }

        public JsonNode Transform(JsonNode source)
        {
            var destination = JsonNode.Parse("{}")!;
            CreateProperties(destination, PathTree);

            foreach (var transformation in _transformations)
            {
                var value = GetPropertyValue(transformation.Key.Split('.').Skip(1), source.Deserialize<JsonNode>());
                SetPropertyValue(transformation.Value.Split('.').Skip(1), value, destination.Root);
            }

            return destination;
        }

        private static void CreateProperties(JsonNode destination, DestinationPathTree pathTree)
        {
            foreach (var property in pathTree.GetProperties())
            {
                CreateProperty(destination, property);
                if (pathTree.HasSubproperties(property))
                    CreateProperties(destination[property], pathTree.GetSubPathTree(property));
            }
        }

        public static JsonNode CreateProperty(JsonNode node, string propertyName)
        {
            node[propertyName] = null;
            return node;
        }

        private static void SetPropertyValue(IEnumerable<string> propertyPath, string value, JsonNode node)
        {
            var property = propertyPath.First();
            if (propertyPath.Skip(1).Any())
            {
                SetPropertyValue(propertyPath.Skip(1), value, node[property]);
            }
            else
                node[property] = value;
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
