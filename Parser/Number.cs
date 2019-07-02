using System;
using ExpressionInterpreter.Lex;

namespace ExpressionInterpreter.Parse
{
    class Number : AST
    {
        Token token;

        public Number(Token token)
        {
            this.token = token;
        }

        public int Value => int.Parse(token.Value);
    }
}