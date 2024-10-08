﻿namespace JPT
{
    public interface IJsonTransformer
    {
        string Transform(string text);

        void ConfigureTransformations(List<KeyValuePair<string, string>> transformations);
    }

    public record JsonTransformerConfiguration
    {
        public required List<KeyValuePair<string, string>> Transformations { get; init; }
    }
}
