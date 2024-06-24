namespace JPT
{
    public record DestinationPathTree
    {
        public const DestinationPathTree End = null!;

        readonly Dictionary<string, DestinationPathTree> _propertiesAndSubproperties = new();

        public void AddDestinationPath(string destinationPath)
        {
            AddProperties(destinationPath.Split('.').Skip(1).ToArray());
        }
        public IEnumerable<string> GetProperties()
            => _propertiesAndSubproperties.Keys;
        public bool HasSubproperties(string key)
            => _propertiesAndSubproperties.TryGetValue(key, out DestinationPathTree? value) && value != DestinationPathTree.End;
        public DestinationPathTree GetSubPathTree(string key)
            => _propertiesAndSubproperties.TryGetValue(key, out DestinationPathTree? value) ? value : DestinationPathTree.End;

        protected void AddProperties(string[] properties)
        {
            var propertyAtThisLevel = properties.First();
            var subProperties = properties.Skip(1).ToArray();
            if (_propertiesAndSubproperties.TryGetValue(propertyAtThisLevel, out DestinationPathTree? value))
            {
                if (subProperties.Length > 0 && value == DestinationPathTree.End)
                {
                    _propertiesAndSubproperties.Add(propertyAtThisLevel, new DestinationPathTree());
                    _propertiesAndSubproperties[propertyAtThisLevel].AddProperties(subProperties);
                }
            }
            else
            {
                if (subProperties.Length == 0)
                    _propertiesAndSubproperties.Add(propertyAtThisLevel, DestinationPathTree.End);
                else
                {
                    _propertiesAndSubproperties.Add(propertyAtThisLevel, new DestinationPathTree());
                    _propertiesAndSubproperties[propertyAtThisLevel].AddProperties(subProperties);
                }
            }
        }
    }
}
