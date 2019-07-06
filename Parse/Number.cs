using System;
using IronPascal.Lex;

namespace IronPascal.Parse
{
    public class Number : AST
    {
        readonly Token token;

        public Number(Token token)
        {
            this.token = token;
        }

        public object Value => token.Value;
    }
}