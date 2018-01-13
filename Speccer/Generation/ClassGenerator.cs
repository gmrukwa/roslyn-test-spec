using Speccer.Description;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Speccer.Generation
{
    public class ClassGenerator
    {
        private readonly ClassDescription _classDescription;
        private int _tabsAmount;

        public ClassGenerator(ClassDescription classDescription)
        {
            _classDescription = classDescription;
        }

        public string GenerateClass()
        {
            StringBuilder sb = new StringBuilder();
            _tabsAmount = 0;

            DefaultUsings(sb);
            BeginNamespace(sb, _classDescription.Namespace);
            BeginClass(sb, _classDescription.Name);

            foreach (var property in _classDescription.Properties)
                AppendProperty(sb, property);

            AppendLine(sb, "");

            foreach (var method in _classDescription.Functions)
                AppendMethod(sb, method);

            CloseBracket(sb);
            CloseBracket(sb);

            return sb.ToString();
        }

        private string TextWithTabs(string text)
        {
            string result = "";
            for (int i = 0; i < _tabsAmount; ++i)
                result += "\t";
            return result + text;
        }

        private void AppendLine(StringBuilder strBuilder, string line)
        {
            strBuilder.AppendLine(TextWithTabs(line));
        }

        private void OpenBracket(StringBuilder strBuilder)
        {
            AppendLine(strBuilder, "{");
            _tabsAmount++;
        }

        private void CloseBracket(StringBuilder strBuilder)
        {
            _tabsAmount--;
            AppendLine(strBuilder, "}");
        }

        private void DefaultUsings(StringBuilder strBuilder)
        {
            AppendLine(strBuilder, "using System;");
        }

        private void BeginNamespace(StringBuilder strBuilder, string namespaceString)
        {
            AppendLine(strBuilder, $"namespace {namespaceString}");
            OpenBracket(strBuilder);
        }

        private void BeginClass(StringBuilder strBuilder, string classString)
        {
            AppendLine(strBuilder, $"public class {classString}");
            OpenBracket(strBuilder);
        }

        private void AppendProperty(StringBuilder strBuilder, PropertyDescription propertyDescription)
        {
            string propertyStr = "public ";
            propertyStr += propertyDescription.Type + " ";
            propertyStr += propertyDescription.Name + " ";
            propertyStr += "{ get; ";
            if (propertyDescription.HasSetter)
                propertyStr += "set; ";
            propertyStr += "}";

            AppendLine(strBuilder, propertyStr);
        }

        private void AppendMethod(StringBuilder strBuilder, FunctionDescription functionDescription)
        {
            string methodStr = "public ";
            methodStr += functionDescription.ReturnType + " ";
            methodStr += functionDescription.Name + "(";
            if(functionDescription.Arguments.ToList().Count != 0)
            {
                int argNum = 0;
                foreach (var argType in functionDescription.Arguments)
                {
                    methodStr += argType + $" arg{argNum}, ";
                    argNum++;
                }
                int lastCommaIdx = methodStr.LastIndexOf(",");
                if (lastCommaIdx >= 0)
                    methodStr = methodStr.Substring(0, lastCommaIdx);
            }
            
            methodStr += ")";

            AppendLine(strBuilder, methodStr);
            OpenBracket(strBuilder);
            AppendLine(strBuilder, "throw new NotImplementedException();");
            CloseBracket(strBuilder);
        }
    }
}
