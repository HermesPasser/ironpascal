using System;
using IronPascal.Lex;

namespace IronPascal.Parse
{
    public class Assign : AST
    {
        public readonly Variable Left;
        public readonly AST Right;
        public readonly Token Operation;
        public readonly Token token;

        // Variable := Number|BinOp
        public Assign(Variable left, Token op, AST right)
        {
            Left = left;
            Operation = token = op;
            Right = right;
        }
    }
}
