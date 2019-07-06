using System;
using System.Collections.Generic;
using IronPascal.Parse;
using IronPascal.Lex;

namespace IronPascal.Interpret
{
    public class Interpreter : NodeVisitor
    {
        private Parser parser;
        public Dictionary<string, object> GlobalScope = new Dictionary<string, object>();

        public Interpreter(Parser parser)
        {
            this.parser = parser;
        }

        public int VisitBinaryOperation(BinaryOperation node)
        {
            switch (node.Operation.Type)
            {
                case TokenKind.Plus:
                    return Visit(node.Left) + Visit(node.Right);
                case TokenKind.Minus:
                    return Visit(node.Left) - Visit(node.Right);
                case TokenKind.Mul:
                    return Visit(node.Left) * Visit(node.Right);
                case TokenKind.Div:
                    return Visit(node.Left) / Visit(node.Right);
            }

            throw new ArgumentException($"TokenKind.{node.Operation.Type} is not supported.");
        }

        public int VisitNumber(Number node) => (int)node.Value;
        
        public int VisitUnaryOperation(UnaryOperation node)
        {
            var operation = node.Operation.Type;
            int result = Visit(node.Expression);
            return operation == TokenKind.Plus ? +result : -result;
        }

        public int VisitCompound(Compound node)
        {
            foreach (var child in node.Children)
                Visit(child);
            return 0;
        }

        public int VisitNoOperation(NoOperation node) => 0;

        public int VisitAssign(Assign node)
        {
            string varName = ((Variable)node.Left).Value;
            GlobalScope[varName] = Visit(node.Right);
            return 0;
        }

        public object VisitVariable(Variable node)
        {
            string varName = node.Value;
            if (GlobalScope.ContainsKey(varName))
                return GlobalScope[varName];

            // TODO: make a NameErrorException
            throw new Exception($"NameError: {varName} not found");
        }

        public int Interpret() => Visit(parser.Parse());
    }
}