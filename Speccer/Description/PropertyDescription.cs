using System;

namespace Speccer.Description
{
    public class PropertyDescription
    {
        public string Name { get; }
        public string Type { get; }
        public bool HasSetter { get; }

        public PropertyDescription(string name, string type, bool hasSetter)
        {
            Name = name;
            Type = type;
            HasSetter = hasSetter;
        }
    }
}
