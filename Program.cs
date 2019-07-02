using System;
using ExpressionInterpreter.Parse;
using ExpressionInterpreter.Lex;
using ExpressionInterpreter.Interpret;

namespace ExpressionInterpreter 
{
    class Program
    {
        static void Main(string[] args)
        {
            string text;
            while(true){
                try
                {
                    Console.Write("calc> ");
                    text = Console.ReadLine();
                }
                catch (Exception)
                {
                    break;
                }
            
				if(text == null)
					continue;

                Lexer lexer = new Lexer(text);
                Parser parser = new Parser(lexer);

                Interpreter interpreter = new Interpreter(parser);
				int result = interpreter.Interpret();
				Console.WriteLine(result);
            }
        }
    }
}
