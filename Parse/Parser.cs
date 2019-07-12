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
            Eat(TokenKind.KeyProgram);
            // TODO: dando erro já que não permiti que vars divessem numeros
            Variable varNode = Variable(); // cuz the prog name follow the same rules as the variable
            string progName = varNode.Value;
            Eat(TokenKind.Semi);
            Block blockNode = Block();
            AST programNode = new Program(progName, blockNode);
            Eat(TokenKind.Dot);
            return programNode;
        }

        Block Block()
        {
            List<AST> declarationNodes = Declarations();
            AST compoundStatementNode = CompountStatement();
            return new Block(declarationNodes, compoundStatementNode);
        }

        List<AST> Declarations()
        {
            List<AST> declarations = new List<AST>();
            if (CurrentToken.Type == TokenKind.KeyVar)
            {
                Eat(TokenKind.KeyVar);
                while (CurrentToken.Type == TokenKind.Id)
                {
                    declarations.AddRange(VariableDeclaration());
                    Eat(TokenKind.Semi);
                }
            }
            return declarations;
        }

        List<AST> VariableDeclaration()
        {
            List<AST> varNodes = new List<AST> { new Variable(CurrentToken) };
            Eat(TokenKind.Id);
            while(CurrentToken.Type == TokenKind.Comma)
            {
                Eat(TokenKind.Comma);
                varNodes.Add(new Variable(CurrentToken));
                Eat(TokenKind.Id);
            }

            Eat(TokenKind.Colon);
            AST typeNode = TypeSpec();
            var varDeclarations = new List<AST>();

            foreach (var varNode in varNodes)
                varDeclarations.Add(new VariableDeclaration(varNode, typeNode));

            return varDeclarations;
        }

        AST TypeSpec()
        {
            Token token = CurrentToken;
            if (CurrentToken.Type == TokenKind.Int)
                Eat(TokenKind.Int);
            else
            {
                Eat(TokenKind.Real);
            }
            return new TypeNode(token);
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

        Variable Variable()
        {
            Variable node = new Variable(CurrentToken);
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

            while (new[] { TokenKind.Mul, TokenKind.IntDiv, TokenKind.FloatDiv }.Contains(CurrentToken.Type))
            {
                Token operation = CurrentToken;
                if (operation.Type == TokenKind.Mul)
                    Eat(TokenKind.Mul);

                if (operation.Type == TokenKind.IntDiv)
                    Eat(TokenKind.IntDiv);

                if (operation.Type == TokenKind.FloatDiv)
                    Eat(TokenKind.FloatDiv);

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
                case TokenKind.IntConst: // TODO: ver p q serve o Int e se ele ainda é usado
                    Eat(TokenKind.IntConst);
                    return new Number(token);
                case TokenKind.RealConst:
                    Eat(TokenKind.RealConst);
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