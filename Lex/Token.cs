using System;

namespace IronPascal.Lex
{
    public class Token
    {
        public readonly TokenKind Type;
        public readonly string Value;

        public Token(TokenKind type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString() => $"Token({Type}, {Value})";
    }
}