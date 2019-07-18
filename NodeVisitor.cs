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
            return visitor.Invoke(this, new object[]{ node });
        }

        // necessário? cs override e abstract
        public int GenericVisit(AST node) => throw new NotImplementedException($"No Visit{node.GetType()} method found");
    }
}