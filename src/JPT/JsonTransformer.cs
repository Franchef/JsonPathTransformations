namespace JPT
{
    public abstract class JsonTransformer : IJsonTransformer
    {
        protected List<KeyValuePair<string, string>> _transformations;

        public DestinationPathTree PathTree { get; private set; } = DestinationPathTree.End;

        public void ConfigureTransformations(List<KeyValuePair<string, string>> transformations)
        {
            if (transformations.Any(kv => !kv.Key.StartsWith('$') || !kv.Value.StartsWith('$')))
                throw new ArgumentException("Invalid JSONPath configuration");
            _transformations = transformations;
            PathTree = new DestinationPathTree();
            foreach (var destination in transformations.Select(kv => kv.Value))
            {
                PathTree.AddDestinationPath(destination);
            }
        }

        public abstract string Transform(string text);
    }
}
