using System;
using IronPascal.Lex;

namespace IronPascal.Parse
{
    public class UnaryOperation : AST
    {
        public readonly AST Expression;
        public readonly Token Operation;
        public UnaryOperation(Token op, AST expr)
        {
            Operation = op;
            Expression = expr;
        }
    }
}
