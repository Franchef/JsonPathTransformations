namespace JPT
{
    public abstract class JsonTransformer : IJsonTransfoermer
    {
        protected List<KeyValuePair<string, string>> _transformations;

        public void ConfigureTransformations(List<KeyValuePair<string, string>> transformations)
        {
            _transformations = transformations;
        }

        public abstract string Transform(string text);

    }
}
