using System;

namespace ExpressionInterpreter.Lex
{
    class Token
    {
        public TokenKind Type;
        public string Value;

        public Token(TokenKind type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString() => $"Token({Type}, {Value})";
    }
}