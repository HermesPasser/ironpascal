using System;
using System.Reflection;
using ExpressionInterpreter.Parse;

namespace ExpressionInterpreter.Interpret
{
    class NodeVisitor
    {
        public int Visit(AST node)
        {
            string methodName = $"Visit{node.GetType().Name}";
            MethodInfo visitor = GetType().GetMethod(methodName); // the method should de public
            return (int)visitor.Invoke(this, new object[]{ node });
        }

        // necessário? cs override e abstract
        public int GenericVisit(AST node) => throw new NotImplementedException($"No Visit{node.GetType()} method found");
    }
}