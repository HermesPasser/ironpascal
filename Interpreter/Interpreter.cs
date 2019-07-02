using System;
using ExpressionInterpreter.Parse;
using ExpressionInterpreter.Lex;

namespace ExpressionInterpreter.Interpret
{
    class Interpreter : NodeVisitor
    {
        private Parser parser;
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

        public int VisitNumber(Number node) => node.Value;
        
        public int Interpret()
        {
            var tree = parser.Parse();
            return Visit(tree);
        }
    }
}