using System;
using System.Collections.Generic;
namespace IronPascal.Lex
{
    public class Lexer
    {
        public string Text;
        private int Position = 0;
        private char? CurrentChar;

        private Dictionary<string, Token> ReservedKeywords =
            new Dictionary<string, Token>
            {
                ["BEGIN"] = new Token(TokenKind.KeyBegin, "BEGIN"),
                ["END"] = new Token(TokenKind.KeyEnd, "END"),
            };

        public Lexer(string text)
        {
            Text = text;
            CurrentChar = Text[Position];
        }

        Exception ThrowError() => throw new LexerException($"Invalid character '{CurrentChar}'");

        void Advance(int times = 1)
        {
            for(int i = 0; i < times; i++)
                CurrentChar = (++Position > Text.Length - 1) ?
                    null : (char?)Text[Position];
        }

        public char? Peek() => (Position + 1 > Text.Length - 1) ? null : (char?)Text[Position + 1];
        
        void SkipWhiteSpace()
        {
            while (CurrentChar.HasValue && char.IsWhiteSpace(CurrentChar.Value))
                Advance();
        }

        Token Id()
        {
            string key = "";
            // TODO: remove latin characters
            while(CurrentChar.HasValue && char.IsLetter(CurrentChar.Value))
            {
                key += CurrentChar;
                Advance();
            }

            if (ReservedKeywords.ContainsKey(key))
                return ReservedKeywords[key];

            return new Token(TokenKind.Id, key);
        }

        int Integer()
        {
            string result = "";
            while (CurrentChar.HasValue && char.IsDigit(CurrentChar.Value))
            {
                result += CurrentChar;
                Advance();
            }
            return int.Parse(result);
        }
    
        public Token NextToken()
        {
            while(CurrentChar.HasValue)
            {
                if (char.IsWhiteSpace(CurrentChar.Value))
                {
                    SkipWhiteSpace();
                    continue;
                }

                // TODO: stills accepts latin letters
                if (char.IsLetter(CurrentChar.Value))
                    return Id();

                if (char.IsDigit(CurrentChar.Value))
                {
                    return new Token(TokenKind.Int, Integer());
                }

                if (CurrentChar == ':' && Peek() == '=')
                {
                    Advance(2);
                    return new Token(TokenKind.Assign, ":=");
                }

                if (CurrentChar == ';')
                {
                    Advance();
                    return new Token(TokenKind.Semi, ";");
                }

                if (CurrentChar == '+')
                {
                    Advance();
                    return new Token(TokenKind.Plus, "+");
                }

                if (CurrentChar == '-')
                {
                    Advance();
                    return new Token(TokenKind.Minus, "-");
                }

                if (CurrentChar == '*')
                {
                    Advance();
                    return new Token(TokenKind.Mul, "*");
                }
                
                if (CurrentChar == '/')
                {
                    Advance();
                    return new Token(TokenKind.Div, "/");
                }
                
                if (CurrentChar == '(')
                {
                    Advance();
                    return new Token(TokenKind.LParen, "(");
                }
                
                if (CurrentChar == ')')
                {
                    Advance();
                    return new Token(TokenKind.RParen, ")");
                }

                if (CurrentChar == '.')
                {
                    Advance();
                    return new Token(TokenKind.Dot, ".");
                }

                ThrowError();
            }

            return new Token(TokenKind.Eof, null);  
        }
    }
}