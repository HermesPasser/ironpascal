using System;
using System.Reflection;
using IronPascal.Parse;

namespace IronPascal.Interpret
{
    public class NodeVisitor
    {
        public object Visit(AST node)
        {
            string methodName = $"Visit{node.GetType().Name}";
            MethodInfo visitor = GetType().GetMethod(methodName); // the method should de public
            if (visitor == null)
                throw new NotImplementedException($"No Visit{node.GetType()} method found");
            return visitor.Invoke(this, new object[]{ node });
        }
    }
}