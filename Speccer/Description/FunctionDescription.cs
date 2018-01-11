using System;
using System.Collections.Generic;
using System.Linq;

namespace Speccer.Description
{
    public class FunctionDescription
    {
        public string Name { get; }
        public Type ReturnType { get; }
        public IEnumerable<Type> Arguments { get; }

        public FunctionDescription(string name, Type returnType, IEnumerable<Type> arguments)
        {
            Name = name;
            ReturnType = returnType;
            Arguments = arguments.ToList();
        }
    }
}
