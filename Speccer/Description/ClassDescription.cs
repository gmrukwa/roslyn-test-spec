using System.Collections.Generic;
using System.Linq;

namespace Speccer.Description
{
    public class ClassDescription
    {
        public string Name { get; }
        public string Namespace { get; }
        public IEnumerable<PropertyDescription> Properties { get; }
        public IEnumerable<FunctionDescription> Functions { get; }

        public ClassDescription(string name, string namesp, IEnumerable<PropertyDescription> properties,
            IEnumerable<FunctionDescription> functions)
        {
            Name = name;
            Namespace = namesp;
            Properties = properties.ToList();
            Functions = functions.ToList();
        }
    }
}
