using IronPascal.Interpret;
using IronPascal.Lex;
using IronPascal.Parse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IronPascalTests
{
    [TestClass]
    public class InterpreterTests
    {
        Interpreter MakeInterpreter(string text)
        {
            Parser p = new Parser(new Lexer(text));
            return new Interpreter(p);
        }

        [TestMethod]
        public void TestArithmeticExpressions()
        {
            var records = new Tuple<string, int>[]
            {
                new Tuple<string, int>("3", 3),
                new Tuple<string, int>("2 + 7 * 4", 30),
                new Tuple<string, int>("7 - 8 / 4", 5),
                new Tuple<string, int>("14 + 2 * 3 - 6 / 2", 17),
                new Tuple<string, int>("7 + 3 * (10 / (12 / (3 + 1) - 1))", 22),
                new Tuple<string, int>("7 + 3 * (10 / (12 / (3 + 1) - 1)) / (2 + 3) - 5 - 3 + (8)", 10),
                new Tuple<string, int>("7 + (((3 + 2)))", 12),
                new Tuple<string, int>("- 3", -3),
                new Tuple<string, int>("+ 3", 3),
                new Tuple<string, int>("5 - - - + - 3", 8),
                new Tuple<string, int>("5 - - - + - (3 + 4) - +2", 10),
            };

            foreach (var item in records)
            {
                string expr = item.Item1; 
                int result = item.Item2;
                Interpreter i = MakeInterpreter($"BEGIN a := {expr} END.");
                i.Interpret();
                Assert.AreEqual(i.GlobalScope["a"], result);
            }
        }

        void InterpretInvalidSyntax(string text)
        {
            var i = MakeInterpreter(text);
            try
            {
                i.Interpret();
                Assert.IsTrue(false);
            }
            catch (ParseException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void TestExpressionInvalidSyntax1()
        {
            InterpretInvalidSyntax(("BEGIN a := 10 * ; END."));
        }

        [TestMethod]
        public void TestExpressionInvalidSyntax2()
        {
            InterpretInvalidSyntax(("BEGIN a := 1 (1 + 2); END."));
        }

        [TestMethod]
        public void TestStatements()
        {
            // TODO: extract into a file
            string text = @"
BEGIN

    BEGIN
        number := 2;
        a := number;
        b := 10 * a + 10 * number / 4;
        c := a - - b
    END;

    x := 11;
END.
";
            var i = MakeInterpreter(text);
            i.Interpret();
            var global = i.GlobalScope;

            Assert.AreEqual(global.Count, 5);
            Assert.AreEqual(global["number"], 2);
            Assert.AreEqual(global["a"], 2);
            Assert.AreEqual(global["b"], 25);
            Assert.AreEqual(global["c"], 27);
            Assert.AreEqual(global["x"], 11);
        }
    }
}
