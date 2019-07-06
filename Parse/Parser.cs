using System;
using System.Linq;
using System.Collections.Generic;
using IronPascal.Lex;

namespace IronPascal.Parse
{
    public class Parser
    {
        private Lexer lexer;
        private Token CurrentToken = null;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            CurrentToken = this.lexer.NextToken();
        }
        
        // no END; (primeiro que aparece) ele ta esperando ponto por algum motivo
        void ThrowError(TokenKind expected) => throw new ParseException($"Invalid syntax, expected {expected}, given {CurrentToken.Type}");
                
        void Eat(TokenKind tokenType)
        {
            if (CurrentToken.Type == tokenType)
                CurrentToken = lexer.NextToken();
            else
            {
                ThrowError(tokenType);
            }
        }

        AST Program()
        {
            AST node = CompountStatement();
            Eat(TokenKind.Dot);
            return node;
        }

        AST CompountStatement()
        {
            Eat(TokenKind.KeyBegin);
            List<AST> nodes = StatementList();
            Eat(TokenKind.KeyEnd);

            Compound root = new Compound();
            foreach (var node in nodes)
                root.Add(node);

            return root;
        }
        
        List<AST> StatementList()
        {
            AST node = Statement();
            List<AST> results = new List<AST> { node };

            while (CurrentToken.Type == TokenKind.Semi)
            {
                Eat(TokenKind.Semi);
                results.Add(Statement());
            }
            			
            if (CurrentToken.Type == TokenKind.Id) // 'cause is expecting a KeyEnd
                ThrowError(TokenKind.Id); // TODO: a msg de erro que isso gera não faz sentido
            return results;
        }

        AST Statement()
        {
            AST node;
            switch (CurrentToken.Type)
            {
                case TokenKind.KeyBegin:
                    node = CompountStatement();
                    break;
                case TokenKind.Id:
                    node = AssignmentStatement();
                    break;
                default:
                    node = Empty();
                    break;
            }
            return node;
        }
                
        AST AssignmentStatement()
        {
            AST left = Variable();
            Token token = CurrentToken;
            Eat(TokenKind.Assign);
            AST right = Expr();
            return new Assign(left, token, right);
        }

        AST Variable()
        {
            AST node = new Variable(CurrentToken);
            Eat(TokenKind.Id);
            return node;
        }

        AST Empty() => new NoOperation();

        AST Expr()
        {
            AST node = Term();

            while (new[] { TokenKind.Plus, TokenKind.Minus }.Contains(CurrentToken.Type))
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

        AST Term()
        {
            AST node = Factor();

            while (new[] { TokenKind.Mul, TokenKind.Div }.Contains(CurrentToken.Type))
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

        AST Factor()
        {
            Token token = CurrentToken;

            switch (token.Type)
            {
                case TokenKind.Plus:
                    Eat(TokenKind.Plus);
                    return new UnaryOperation(token, Factor());
                case TokenKind.Minus:
                    Eat(TokenKind.Minus);
                    return new UnaryOperation(token, Factor());
                case TokenKind.Int:
                    Eat(TokenKind.Int);
                    return new Number(token);
                case TokenKind.LParen:
                {
                    Eat(TokenKind.LParen);
                    AST node = Expr();
                    Eat(TokenKind.RParen);
                    return node;
                }
                default:
                    return Variable();
            }
        }

        public AST Parse()
        {
            AST node = Program();
            if (CurrentToken.Type != TokenKind.Eof)
                ThrowError(TokenKind.Eof);

            return node;
        }
    }
} 