using System;
using IronPascal.Lex;

namespace IronPascal.Parse
{
    public class BinaryOperation : AST
    {
        public readonly AST Left;
        public readonly AST Right;
        public readonly Token Operation;
        Token token;

        public BinaryOperation(AST left, Token op, AST right)
        {
            Left = left;
            Right = right;
            Operation = token = op;
        }
    }
}