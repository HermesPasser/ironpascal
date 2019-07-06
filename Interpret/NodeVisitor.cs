using System;
using System.Reflection;
using IronPascal.Parse;

namespace IronPascal.Interpret
{
    public class NodeVisitor
    {
        public int Visit(AST node)//TODO: SOMETHING WILL BREAK IF THIS RETURN INT, SHOULD NOT BE OBJECT?
        {
            string methodName = $"Visit{node.GetType().Name}";
            MethodInfo visitor = GetType().GetMethod(methodName); // the method should de public
            return (int)visitor.Invoke(this, new object[]{ node });
        }

        // necessário? cs override e abstract
        public int GenericVisit(AST node) => throw new NotImplementedException($"No Visit{node.GetType()} method found");
    }
}