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

            Assert.AreEqual(tok.Type, TokenKind.IntConst);
            Assert.AreEqual(tok.Value, 234.ToString());
        }


        [TestMethod]
        public void TestReal()
        {
            var le = MakeLexer("23.4");
            var tok = le.NextToken();

            Assert.AreEqual(tok.Type, TokenKind.RealConst);
            Assert.AreEqual(tok.Value, 23.4f.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        
        [TestMethod]
        public void TestTokens()
        {
            var records = new Tuple<string, TokenKind, string>[]
            {
                new Tuple<string, TokenKind, string>("234", TokenKind.IntConst, "234"),
                new Tuple<string, TokenKind, string>("3.14", TokenKind.RealConst, "3.14"),
                new Tuple<string, TokenKind, string>("*", TokenKind.Mul, "*"),
                new Tuple<string, TokenKind, string>("DIV", TokenKind.IntDiv, "DIV"),
                new Tuple<string, TokenKind, string>("/", TokenKind.FloatDiv, "/"),
                new Tuple<string, TokenKind, string>("+", TokenKind.Plus, "+"),
                new Tuple<string, TokenKind, string>("-", TokenKind.Minus, "-"),
                new Tuple<string, TokenKind, string>("(", TokenKind.LParen, "("),
                new Tuple<string, TokenKind, string>(")", TokenKind.RParen, ")"),
                new Tuple<string, TokenKind, string>(":=", TokenKind.Assign, ":="),
                new Tuple<string, TokenKind, string>(".", TokenKind.Dot, "."),
                new Tuple<string, TokenKind, string>("Number", TokenKind.Id, "Number"),
                new Tuple<string, TokenKind, string>(";", TokenKind.Semi, ";"),
                new Tuple<string, TokenKind, string>("BEGIN", TokenKind.KeyBegin, "BEGIN"),
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
