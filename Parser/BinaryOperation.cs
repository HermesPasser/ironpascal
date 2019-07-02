using System;
using ExpressionInterpreter.Lex;

namespace ExpressionInterpreter.Parse
{
    class BinaryOperation : AST
    {
        public AST Left;
        public AST Right;
        public Token Operation;
        Token token;

        public BinaryOperation(AST left, Token op, AST right)
        {
            Left = left;
            Right = right;
            Operation = token = op;
        }
    }
}