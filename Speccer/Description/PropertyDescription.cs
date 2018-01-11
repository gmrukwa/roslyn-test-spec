using System;

namespace Speccer.Description
{
    public class PropertyDescription
    {
        public string Name { get; }
        public Type Type { get; }
        public bool HasSetter { get; }

        public PropertyDescription(string name, Type type, bool hasSetter)
        {
            Name = name;
            Type = type;
            HasSetter = hasSetter;
        }
    }
}
