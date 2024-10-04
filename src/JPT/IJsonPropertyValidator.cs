namespace JPT
{
    public interface IJsonPropertyValidator
    {
        public  string PropertyPath { get; init; }
        bool PropertyIsValid();
    }
}
