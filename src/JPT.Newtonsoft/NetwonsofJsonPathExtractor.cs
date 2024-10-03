using Newtonsoft.Json.Linq;

namespace JPT.Newtonsoft
{
    public class NetwonsofJsonPathExtractor : JPT.IJsonPathExtractor
    {
        private readonly JObject _source;

        public NetwonsofJsonPathExtractor(JObject source)
        {
            _source=source;
        }
        public object Extracts(string jsonPath)
        {
            return _source.SelectToken(jsonPath).Value<object>();
        }
    }
}
