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

        public object VisitBinaryOperation(BinaryOperation node)
        {
            double left = (double)Visit(node.Left);
            double right = (double)Visit(node.Right);

            switch (node.Operation.Type)
            {
                // TODO: aside from IntDiv, every operation can return a double with decimal places
                case TokenKind.Plus:     
                    return left + right;
                case TokenKind.Minus:
                    return left - right;
                case TokenKind.Mul:
                    return left * right;
                case TokenKind.IntDiv:
                    return Math.Truncate(left / right);
                case TokenKind.FloatDiv: 
                    return left / right;
            }

            throw new ArgumentException($"TokenKind.{node.Operation.Type} is not supported.");
        }

        public double VisitNumber(Number node) => node.Value;
        
        public object VisitUnaryOperation(UnaryOperation node)
        {
            var operation = node.Operation.Type;
            double result = (double) Visit(node.Expression);
            return operation == TokenKind.Plus ? +result : -result;
        }

        public double VisitCompound(Compound node)
        {
            foreach (var child in node.Children)
                Visit(child);
            return 0;
        }

        public double VisitNoOperation(NoOperation node) => 0;

        public double VisitAssign(Assign node)
        {
            string varName = ((Variable)node.Left).Value;
            GlobalScope[varName] = Visit(node.Right);
            return 0; // since Visit() must return something
        }

        public object VisitVariable(Variable node)
        {
            string varName = node.Value;
            if (GlobalScope.ContainsKey(varName))
                return GlobalScope[varName];

            // TODO: make a NameErrorException
            throw new Exception($"NameError: {varName} not found");
        }

        public object VisitTypeNode(TypeNode node) => null;

        public object VisitVariableDeclaration(AST node) => null;

        public object VisitBlock(Block node)
        {
            foreach (var decl in node.Declarations)
                Visit(decl);
            return Visit(node.CompoundStatement); // no need to return 
        }

        public object VisitProgram(Parse.Program node) => Visit(node.block);
        
        public object Interpret() => Visit(parser.Parse());
    }
}