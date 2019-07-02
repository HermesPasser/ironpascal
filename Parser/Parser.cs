using System;
using System.Linq;
using ExpressionInterpreter.Lex;

namespace ExpressionInterpreter.Parse
{
    class Parser
    {
        private Lexer lexer;
        private Token CurrentToken = null;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            CurrentToken = this.lexer.NextToken();
        }
        
        Exception ThrowError(TokenKind expected) => throw new ParseException($"Invalid syntax, expected {expected}, given {CurrentToken.Type}");
                
        void Eat(TokenKind tokenType)
        {
            if (CurrentToken.Type == tokenType)
                CurrentToken = lexer.NextToken();
            else
            {
                ThrowError(tokenType);
            }
        }
        AST Factor()
        {
            Token token = CurrentToken;
            if (token.Type == TokenKind.Int)
            {
                Eat(TokenKind.Int);
                return new Number(token);
            }

            else if (token.Type == TokenKind.LParen)
            {
                Eat(TokenKind.LParen);
                AST node = Expr();
                Eat(TokenKind.RParen);
                return node;
            }

            return null;
        }
            
        AST Term()
        {
            AST node = Factor();

            while(new []{TokenKind.Mul, TokenKind.Div}.Contains(CurrentToken.Type))
            {
                Token operation = CurrentToken;
                if (operation.Type == TokenKind.Mul)
                    Eat(TokenKind.Mul);

                if (operation.Type == TokenKind.Div)
                    Eat(TokenKind.Div);
                
                node = new BinaryOperation(node, operation, Factor());
            }

            return node;
        }

        AST Expr()
        {
            AST node = Term();

            while(new []{TokenKind.Plus, TokenKind.Minus}.Contains(CurrentToken.Type))
            {
                Token operation = CurrentToken;
                if (operation.Type == TokenKind.Plus)
                    Eat(TokenKind.Plus);

                else if (operation.Type == TokenKind.Minus)
                    Eat(TokenKind.Minus);
         
                node = new BinaryOperation(node, operation, Term());
            }

            return node;
        }

        public AST Parse() => Expr();
    }
}