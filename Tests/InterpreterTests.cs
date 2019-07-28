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
            return new Interpreter(p.Parse());
        }

        [TestMethod]
        public void TestArithmeticExpressions()
        {
            var records = new Tuple<string, double>[]
            {
                new Tuple<string, double>("3", 3),
                new Tuple<string, double>("2 + 7 * 4", 30),
                new Tuple<string, double>("7 - 8 / 4", 5),
                new Tuple<string, double>("14 + 2 * 3 - 6 / 2", 17),
                new Tuple<string, double>("7 + 3 * (10 / (12 / (3 + 1) - 1))", 22),
                new Tuple<string, double>("7 + 3 * (10 / (12 / (3 + 1) - 1)) / (2 + 3) - 5 - 3 + (8)", 10),
                new Tuple<string, double>("7 + (((3 + 2)))", 12),
                new Tuple<string, double>("- 3", -3),
                new Tuple<string, double>("+ 3", 3),
                new Tuple<string, double>("5 - - - + - 3", 8),
                new Tuple<string, double>("5 - - - + - (3 + 4) - +2", 10),
            };

            foreach (var item in records)
            {
                string expr = item.Item1;
                double result = item.Item2;
                Interpreter i = MakeInterpreter($@"
                    PROGRAM Test;
                    VAR
                        a : INTEGER;
                    BEGIN
                        a := {expr}
                    END.
                ");
                i.Interpret();
                Assert.AreEqual((double)i.GlobalMemory["a"], result);
            }
        }

        void InterpretInvalidSyntax(string text)
        {           
            try
            {
				var i = MakeInterpreter(text);
                i.Interpret();
                Assert.Fail();
            }
            catch (ParseException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void TestExpressionInvalidSyntax1()
        {
            InterpretInvalidSyntax(@"
                PROGRAM Test;
                VAR
                a : INTEGER;
                BEGIN
                   a := 10 * ;  {Invalid syntax}
                END."
            );
        }

        [TestMethod]
        public void TestExpressionInvalidSyntax2()
        {
            InterpretInvalidSyntax(@"
                PROGRAM Test;
                VAR
                   a : INTEGER;
                BEGIN
                   a := 1 (1 + 2); {Invalid syntax}
                END."
            );
        }

        [TestMethod]
        public void TestIntegerArithmeticExpressions()
        {
            var exprResults = new Tuple<string, double>[]
            {
                new Tuple<string, double>("3", 3),
                new Tuple<string, double>("2 + 7 * 4", 30),
                new Tuple<string, double>("7 - 8 DIV 4", 5),
                new Tuple<string, double>("14 + 2 * 3 - 6 DIV 2", 17),
                new Tuple<string, double>("7 + 3 * (10 DIV (12 DIV (3 + 1) - 1))", 22),
                new Tuple<string, double>("7 + 3 * (10 DIV (12 DIV (3 + 1) - 1)) DIV (2 + 3) - 5 - 3 + (8)", 10),
                new Tuple<string, double>("7 + (((3 + 2)))", 12),
                new Tuple<string, double>("- 3", -3),
                new Tuple<string, double>("+ 3", 3),
                new Tuple<string, double>("5 - - - + - 3", 8),
                new Tuple<string, double>("5 - - - + - (3 + 4) - +2", 10),
            };

            foreach (var item in exprResults)
            {
                var intr = MakeInterpreter($@"
                       PROGRAM Test;
                       VAR
                           a : INTEGER;
                       BEGIN
                           a := {item.Item1}
                       END.
                ");

                intr.Interpret();
                var v = intr.GlobalMemory["a"];
                Assert.AreEqual(intr.GlobalMemory["a"], item.Item2);
            }
        }

        [TestMethod]
        public void TestFloatArithmeticExpressions()
        {
            var exprResults = new Tuple<string, double>[]
            {
                new Tuple<string, double>("3.14", 3.14),
                new Tuple<string, double>("2.14 + 7 * 4", 30.14),
                new Tuple<string, double>("7.14 - 8 / 4", 5.14),
            };

            foreach (var item in exprResults)
            {
                var intr = MakeInterpreter($@"
                       PROGRAM Test;
                       VAR
                           a : REAL;
                       BEGIN
                           a := {item.Item1}
                       END.
                ");

                intr.Interpret();
                Assert.AreEqual(intr.GlobalMemory["a"],item.Item2);
            }
        }

        [TestMethod]
        public void TestProgram()
        {
            string text = @"
PROGRAM Part12;
VAR
   number : INTEGER;
   a, b   : INTEGER;
   y      : REAL;
PROCEDURE P1;
VAR
   a : REAL;
   k : INTEGER;
   PROCEDURE P2;
   VAR
      a, z : INTEGER;
   BEGIN {P2}
      z := 777;
   END;  {P2}
BEGIN {P1}
END;  {P1}
BEGIN {Part12}
   number := 2;
   a := number ;
   b := 10 * a + 10 * number DIV 4;
   y := 20 / 7 + 3.14
END.  {Part12}
";
            var i = MakeInterpreter(text);
            i.Interpret();
            var global = i.GlobalMemory;

            // parse because globalscope store as object(double)
            Assert.AreEqual(global.Count, 4);
            Assert.AreEqual((double)global["number"], 2);
            Assert.AreEqual((double)global["a"], 2);
            Assert.AreEqual((double)global["b"], 25);
            Assert.AreEqual((double)global["y"], 20d / 7 + 3.14); // without the suffix will consider 20 as int
        }
    }
}
