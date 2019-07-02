using System;
/*
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 */
namespace CInterpreter 
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = null;
            while(true){
                try
                {
                    Console.Write("calc> ");
                    text = Console.ReadLine();
                }
                catch (Exception e) // só pega exceções de I/O
                {
                    break;
                }
            
				if(text == null)
					continue;

				Interpreter interpreter = new Interpreter(text);
				int result = interpreter.Expr();
				Console.WriteLine(result);
            }
        }
    }
}
