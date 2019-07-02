using System;

namespace CInterpreter
{
    class Interpreter
    {
        public string Text;
        private int Position = 0;
        private Token CurrentToken = null;

        public Interpreter(string text)
        {
            Text = text;
        }

        Exception ThrowError()
        {
            // TODO: criar custom exception
            throw new Exception("Error parsing input");
        }

        Token NextToken()
        {
            string text = Text;
            if (Position > text.Length - 1)
                return new Token(TokenKind.Eof, null);

            char currentChar = text[Position];

            if (Char.IsDigit(currentChar))
            {
                Position++;
                // TODO: handle parse fail?
				// supostamente eu faria parse e enviaria mas o typo ta string então não farei
                // return new Token(TokenKind.Int, Int.Parse(currentChar));
                return new Token(TokenKind.Int, currentChar + "");
            }

            if (currentChar == '+')
            {
                Position++;
                return new Token(TokenKind.Plus, currentChar + "");
            }

            ThrowError();
			return null;
        }

        void Eat(TokenKind tokenType)
        {
            if (CurrentToken.Type == tokenType)
                CurrentToken = NextToken();
            else
            {
                ThrowError();
            }
        }

        // TODO: talvez precisar retornar object ou <T> já que não será só int
        public int Expr()
        {
            CurrentToken = NextToken();
            Token left = CurrentToken;
            Eat(TokenKind.Int);

            Token operation = CurrentToken;
            Eat(TokenKind.Plus);

            Token right = CurrentToken;
            Eat(TokenKind.Int);
			
			// TODO: parse since is an string
            return Int32.Parse(left.Value) + Int32.Parse(right.Value);
        }
    }
}