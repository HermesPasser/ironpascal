using System;
using IronPascal.Lex;

namespace IronPascal.Parse
{
    public class Assign : AST
    {
        public readonly AST Left;
        public readonly AST Right;
        public readonly Token Operation;
        public readonly Token token;

        public Assign(AST left, Token op, AST right)
        {
            Left = left;
            Operation = token = op;
            Right = right;
        }
    }
}
