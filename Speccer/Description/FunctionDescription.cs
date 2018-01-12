using System;
using System.Collections.Generic;
using System.Linq;

namespace Speccer.Description
{
    public class FunctionDescription
    {
        public string Name { get; }
        public string ReturnType { get; }
        public IEnumerable<string> Arguments { get; }

        public FunctionDescription(string name, string returnType, IEnumerable<string> arguments)
        {
            Name = name;
            ReturnType = returnType;
            Arguments = arguments.ToList();
        }
    }
}
