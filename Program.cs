using IronPascal.Interpret;
using IronPascal.Lex;
using IronPascal.Parse;
using System;
using System.IO;
using System.Collections.Generic;

namespace IronPascal
{
    class Program
    {
        static void Calc()
        {
            string text;
            while (true)
            {
                try
                {
                    Console.Write("calc> ");
                    text = Console.ReadLine();
                }
                catch (Exception)
                {
                    break;
                }

                if (text == null)
                    continue;

                Lexer lexer = new Lexer($"PROGRAM calc; VAR result : REAL; BEGIN result := {text} END.");
                Interpreter interpreter = new Interpreter((new Parser(lexer)).Parse());
                interpreter.Interpret();
                Console.WriteLine(interpreter.GlobalMemory["result"]);
            }
        }

        static void Dump(Dictionary<string, object> dict)
        {
            Console.WriteLine('{');

            foreach (var key in dict.Keys)
                Console.Write($"\t'{key}': {dict[key]}\n");

            Console.WriteLine("}");
        }

        static void Main(string[] args)
        {
            
#if DEBUG
            args = new []{ "assignments.pas" };     
#endif
            if (args.Length < 1)
            {
                Console.WriteLine("Invalid argument.");
                Console.WriteLine("\t program <filepath> : execute pascal code.");
                Console.WriteLine("\t program -c : run a simple calculator.");
                return;
            }

            if (args[0] == "-c")
            {
                Calc();
                return;
            }

            string text = File.ReadAllText(args[0]);

            Lexer lexer = new Lexer(text);
            Parser parser = new Parser(lexer);
            var tree = parser.Parse();
            
            SemanticAnalyzer analyzer = new SemanticAnalyzer();
            try
			{				
				analyzer.Visit(tree);
			} 
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
            			
            Console.WriteLine(analyzer.SymTab);

            Interpreter interpreter = new Interpreter(tree);
            var res = interpreter.Interpret();
            Console.WriteLine();
            Console.WriteLine("Run-time GLOBAL_MEMORY contents:");
			
            Dump(interpreter.GlobalMemory);
            Console.ReadKey();
        }
    }
}
