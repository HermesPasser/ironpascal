using System;

namespace CInterpreter
{
    enum TokenKind
    {
        Int, 
        Plus, 
        Eof
    }
    
    class Token
    {
        public TokenKind Type;
        public string Value;
        //TODO: value talvez virar um object ou <T> j? que 
        // o valor varia e tipo
        public Token(TokenKind type, string value)
        {
            Type = type;
            Value = value;
        }

        string ToString()
        {
            return $"Token({Type}, {Value})";
        }
    }
}