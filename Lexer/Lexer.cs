using System;

namespace ExpressionInterpreter.Lex
{
    class Lexer
    {
        public string Text;
        private int Position = 0;
        private char? CurrentChar;

        public Lexer(string text)
        {
            Text = text;
            CurrentChar = Text[Position];
        }
          
        Exception ThrowError() => throw new LexerException($"Invalid character '{CurrentChar}'");
        
        void Advance()
        {
            Position++;
            if (Position > Text.Length -1 )
                CurrentChar = null; // maybe use \x0 instead? and then change char? to char
            else
            {
                CurrentChar = Text[Position];
            }
        }
        void SkipWhiteSpace()
        {
            while (CurrentChar.HasValue && char.IsWhiteSpace(CurrentChar.Value))
                Advance();
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

                if (char.IsDigit(CurrentChar.Value))
                {
                    return new Token(TokenKind.Int, Integer().ToString());
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
                
                ThrowError();
            }

            return new Token(TokenKind.Eof, null);  
        }
    }
}