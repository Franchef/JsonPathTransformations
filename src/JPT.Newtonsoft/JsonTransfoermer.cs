using Newtonsoft.Json.Linq;

namespace JPT.Newtonsoft
{
    public class JsonTransfoermer : JPT.JsonTransformer
    {
        public override string Transform(string text)
        {
            JObject source = JObject.Parse(text);
            JObject destination = new();

            foreach (var transformation in _transformations)
            {
                JToken token = source.SelectToken(transformation.Key);
                var pathParts = transformation.Value.Split('.');

                JToken destinationProperty = destination;

                foreach (var property in pathParts)
                {
                    destinationProperty = destinationProperty[property];
                }

                destinationProperty.Value<object>(token.Value<object>());
            }


            return destination.ToString();
        }

        //private void (JsonObject jobject, string property, string[] subproperty)

    }
}
