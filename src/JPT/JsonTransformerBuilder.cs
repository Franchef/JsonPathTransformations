namespace JPT
{
    public abstract class JsonTransformerBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        List<KeyValuePair<string, string>> _transformConfiguration = new();

        protected JsonTransformerBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        JsonTransformerBuilder AddTransformation(string source, string destination)
        {
            _transformConfiguration.Add(new KeyValuePair<string, string>(source, destination));
            return this;
        }

        IJsonTransfoermer Build()
        {
            var transformer = _serviceProvider.GetService(typeof(IJsonTransfoermer)) as IJsonTransfoermer;
            transformer.ConfigureTransformations(_transformConfiguration);
            return transformer;
        }
    }
}
