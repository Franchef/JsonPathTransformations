using Newtonsoft.Json.Linq;

namespace JPT.Newtonsoft
{
    public class JsonTransformer : JPT.JsonTransformer
    {
        public override string Transform(string text)
        {
            JObject source = JObject.Parse(text);
            return Transform(source).ToString();
        }

        public JObject Transform(JObject source)
        {
            JObject destination = new();
            CreateProperties(destination, PathTree);

            foreach (var transformation in _transformations)
            {
                var value = source.SelectToken(transformation.Key).Value<string>();
                SetPropertyValue(transformation.Value.Split('.').Skip(1), value, destination.Root);
            }

            return destination;
        }

        private static void SetPropertyValue(IEnumerable<string> propertyPath, string value, JToken token)
        {
            var property = propertyPath.First();
            if (propertyPath.Skip(1).Any())
            {
                SetPropertyValue(propertyPath.Skip(1), value, token[property]);
            }
            else
                token[property] = value;
        }

        private static void CreateProperties(JObject destination, DestinationPathTree pathTree)
        {
            foreach (var property in pathTree.GetProperties())
            {
                CreateProperty(destination, property);
                if (pathTree.HasSubproperties(property))
                    CreateProperties(destination[property], pathTree.GetSubPathTree(property));
            }
        }

        private static void CreateProperties(JToken destination, DestinationPathTree pathTree)
        {
            foreach (var property in pathTree.GetProperties())
            {
                CreateProperty(destination, property);
                if (pathTree.HasSubproperties(property))
                    CreateProperties(destination[property], pathTree.GetSubPathTree(property));
            }
        }
        public static JObject CreateProperty(JObject jobject, string propertyName)
        {
            jobject.Add(propertyName, null);
            return jobject;
        }

        public static JToken CreateProperty(JToken jobject, string propertyName)
        {
            jobject[propertyName] = null;
            return jobject;
        }

        //private void (JsonObject jobject, string property, string[] subproperty)

    }
}
