using System;
using System.Collections.Generic;
using System.Globalization;

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
                ["PROGRAM"] = new Token(TokenKind.KeyProgram, "PROGRAM"),
                ["VAR"] = new Token(TokenKind.KeyVar, "VAR"),
                ["DIV"] = new Token(TokenKind.IntDiv, "DIV"),
                ["BEGIN"] = new Token(TokenKind.KeyBegin, "BEGIN"),
                ["INTEGER"] = new Token(TokenKind.Int, "INTEGER"), // usar int mesmo?
                ["REAL"] = new Token(TokenKind.Real, "REAL"),
                ["PROCEDURE"] = new Token(TokenKind.KeyProcedure, "PROCEDURE"),
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

        void SkipComment()
        {
            while (CurrentChar != '}')
                Advance();
            Advance(); // close }
        }

        // retornava int e agora token
        Token Number()
        {
            string result = "";
            while (CurrentChar.HasValue && char.IsDigit(CurrentChar.Value))
            {
                result += CurrentChar;
                Advance();
            }

            if (CurrentChar == '.')
            {
                result += CurrentChar;
                Advance();
                while (CurrentChar.HasValue && char.IsDigit(CurrentChar.Value))
                {
                    result += CurrentChar;
                    Advance();
                }

                return new Token(TokenKind.RealConst, result);
            }

            return new Token(TokenKind.IntConst, result);
        }

        Token Id()
        {
            string key = "";
            // TODO: remove latin characters and add _
            while(CurrentChar.HasValue && char.IsLetterOrDigit(CurrentChar.Value))
            {
                key += CurrentChar;
                Advance();
            }

            if (ReservedKeywords.ContainsKey(key.ToUpper()))
                return ReservedKeywords[key.ToUpper()];

            return new Token(TokenKind.Id, key);
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

                if (CurrentChar == '{')
                {
                    Advance();
                    SkipComment();
                    continue;
                }

                // TODO: stills accepts latin letters
                if (char.IsLetter(CurrentChar.Value))
                    return Id();

                if (char.IsDigit(CurrentChar.Value))
                    return Number();
                
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

                if (CurrentChar == ':')
                {
                    Advance();
                    return new Token(TokenKind.Colon, ":");
                }

                if (CurrentChar == ',')
                {
                    Advance(2);
                    return new Token(TokenKind.Comma, ",");
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
                    return new Token(TokenKind.FloatDiv, "/");
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