using System;

namespace IronPascal.Lex
{
    public class Token
    {
        public readonly TokenKind Type;
        public readonly object Value;

        public Token(TokenKind type, object value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString() => $"Token({Type}, {Value})";
    }
}