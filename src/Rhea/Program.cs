using System;

namespace Rhea
{
    class Program
    {
        private static void Usage()
        {
            Console.WriteLine("usage: rhea [file]");
        }
        
        static void Main(string[] args)
        {
            Interpreter interp = new Interpreter();
            switch (args.Length)
            {
            case 0:
                interp.InterpretInteractively();
                break;
            case 1:
                interp.InterpretFile(args[0]);
                break;
            default:
                Usage();
                break;
            }
        }
    }
}
