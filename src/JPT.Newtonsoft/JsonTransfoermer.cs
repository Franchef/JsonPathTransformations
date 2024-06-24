using Newtonsoft;
using Newtonsoft.Json;
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
                var pathParts = transformation.Value.Split('.').Skip(1);

                var destinationProperty = destination.SelectToken("$") as JObject;

                foreach (var property in pathParts)
                {
                    destinationProperty.Add(property);
                    destinationProperty = destinationProperty[property] as JObject;
                }

                destinationProperty.Value<object>(token.Value<object>());
            }


            return destination.ToString();
        }

        //private void (JsonObject jobject, string property, string[] subproperty)

    }
}
