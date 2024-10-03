namespace JPT
{
    public abstract class JsonTransformerBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        readonly List<KeyValuePair<string, string>> _transformConfiguration = new();

        protected JsonTransformerBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        JsonTransformerBuilder AddTransformation(string source, string destination)
        {
            _transformConfiguration.Add(new KeyValuePair<string, string>(source, destination));
            return this;
        }

        IJsonTransformer Build()
        {
            var transformer = _serviceProvider.GetService(typeof(IJsonTransformer)) as IJsonTransformer;
            transformer.ConfigureTransformations(_transformConfiguration);
            return transformer;
        }
    }
}
