using System;
using System.Collections.Generic;
using IronPascal.Parse;
using IronPascal.Lex;

namespace IronPascal.Interpret
{
    public class Interpreter : NodeVisitor
    {
        private readonly AST Tree;
        public Dictionary<string, object> GlobalMemory = new Dictionary<string, object>();

        public Interpreter(AST tree)
        {
            Tree = tree;
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
            double result = (double)Visit(node.Expression);
            return operation == TokenKind.Plus ? +result : -result;
        }

        public void VisitCompound(Compound node)
        {
            foreach (var child in node.Children)
                Visit(child);
        }

        public void VisitNoOperation(NoOperation node) { }

        public void VisitAssign(Assign node)
        {
            string varName = ((Variable)node.Left).Value;
            GlobalMemory[varName] = Visit(node.Right);
        }

        public object VisitVariable(Variable node)
        {
            string varName = node.Value;
            if (GlobalMemory.ContainsKey(varName))
                return GlobalMemory[varName];

            // TODO: make a NameErrorException
            throw new Exception($"NameError: {varName} not found");
        }

        public void VisitTypeNode(TypeNode node) { }

        public void VisitVariableDeclaration(AST node) {}
		
        public void VisitBlock(Block node)
        {
            foreach (var decl in node.Declarations)
                Visit(decl);
            Visit(node.CompoundStatement);
        }
	
        public void VisitProcedureDeclaration(AST node)
        {
            //
			
        }
		
        public void VisitProgram(Parse.Program node) => Visit(node.block);
        
        public object Interpret() => Visit(Tree);
    }
}