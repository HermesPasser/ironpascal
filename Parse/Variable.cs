using System;
using IronPascal.Lex;

namespace IronPascal.Parse
{
    public class Variable : AST
    {
        readonly Token token;
        public readonly string Value;

        public Variable(Token token)
        {
            this.token = token;
            // TODO: i think it's better change from string to object
            Value = this.token.Value + ""; 
        }
    }
}
