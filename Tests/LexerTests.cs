using IronPascal.Lex;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IronPascalTests
{

    [TestClass]
    public class LexerTests
    {
        Lexer MakeLexer(string text)
        {
            return new Lexer(text);
        }

        [TestMethod]
        public void TestInteger()
        {
            var le = MakeLexer("234");
            var tok = le.NextToken();

            Assert.AreEqual(tok.Type, TokenKind.Int);
            Assert.AreEqual(tok.Value, 234);
        }

        [TestMethod]
        public void TestMul()
        {
            var lex = MakeLexer("*");
            var tok = lex.NextToken();
            Assert.AreEqual(tok.Type, TokenKind.Mul);
            Assert.AreEqual(tok.Value, "*");
        }

        [TestMethod]
        public void TestDiv()
        {
            var lex = MakeLexer("/");
            var tok = lex.NextToken();
            Assert.AreEqual(tok.Type, TokenKind.Div);
            Assert.AreEqual(tok.Value, "/");
        }

        [TestMethod]
        public void TestPlus()
        {
            var lex = MakeLexer("+");
            var tok = lex.NextToken();
            Assert.AreEqual(tok.Type, TokenKind.Plus);
            Assert.AreEqual(tok.Value, "+");
        }

        [TestMethod]
        public void TestMinus()
        {
            var lex = MakeLexer("-");
            var tok = lex.NextToken();
            Assert.AreEqual(tok.Type, TokenKind.Minus);
            Assert.AreEqual(tok.Value, "-");
        }

        [TestMethod]
        public void TestLParen()
        {
            var lex = MakeLexer("(");
            var tok = lex.NextToken();
            Assert.AreEqual(tok.Type, TokenKind.LParen);
            Assert.AreEqual(tok.Value, "(");
        }

        [TestMethod]
        public void TestRParen()
        {
            var lex = MakeLexer(")");
            var tok = lex.NextToken();
            Assert.AreEqual(tok.Type, TokenKind.RParen);
            Assert.AreEqual(tok.Value, ")");
        }

        [TestMethod]
        public void TestNewTokens()
        {
            var records = new Tuple<string, TokenKind, string>[]
            {
                new Tuple<string, TokenKind, string>(":=", TokenKind.Assign, ":="),
                new Tuple<string, TokenKind, string>(".", TokenKind.Dot, "."),
                new Tuple<string, TokenKind, string>("Number", TokenKind.Id, "Number"),
                new Tuple<string, TokenKind, string>(";", TokenKind.Semi, ";"),
                new Tuple<string, TokenKind, string>("END", TokenKind.KeyEnd, "END")        
            };

            foreach (var item in records)
            {
                string text = item.Item1, tokVal = item.Item3;
                TokenKind tokType = item.Item2;
                Lexer lex = MakeLexer(text);
                Token tok = lex.NextToken();

                Assert.AreEqual(tok.Type, tokType);
                Assert.AreEqual(tok.Value, tokVal);
            }
        }
    }
}
