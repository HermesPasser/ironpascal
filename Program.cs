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
                Interpreter interpreter = new Interpreter(new Parser(lexer));
                interpreter.Interpret();
                Console.WriteLine(interpreter.GlobalScope["result"]);
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
            // TODO: there's no information on GlobalScope to know if a var is
            // int or float then everything on globalscope has a double value
            // this can lead bugs since in no point i check if an int has decimal places acidentally

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

            Interpreter interpreter = new Interpreter(parser);
            interpreter.Interpret();
            Dump(interpreter.GlobalScope);
            Console.ReadKey();
        }
    }
}
