using System;
using System.IO;
using System.Collections.Generic;

namespace Rhea
{
    public class Interpreter
    {
        private IEnv mEnv;
        
        public Interpreter()
        {
            mEnv = new EnvGlobal();
            mEnv.AddVariable("puts", 1, (args, vm, info) => {
                Console.WriteLine(args[0]);
                vm.Push(args[0]);
            });
            mEnv.AddVariable("dynamic_wind", 3, (args, vm, info) => {
                ValueCont cont = vm.GetCont();
                IValueFunc before = args[0] as IValueFunc;
                IValueFunc after = args[2] as IValueFunc;
                var winder = new KeyValuePair<IValueFunc, IValueFunc>(before, after);
                Stack<IInsn> insnStack = new Stack<IInsn>();
                insnStack.Push(new InsnCall(0, info));
                insnStack.Push(InsnPop.Instance);
                insnStack.Push(new InsnPushWinder(winder));
                insnStack.Push(new InsnPush(args[1]));
                insnStack.Push(new InsnCall(0, info));
                insnStack.Push(new InsnCall(1, info));
                vm.Insns = insnStack.ToSList();
                vm.Stack = SList.List<IValue>(before, cont);
            });
            mEnv.AddMethod("Int", "puts", 1, (args, vm, info) => {
                Console.WriteLine(args[0]);
                vm.Push(args[0]);
            });
        }
        
        public void InterpretInteractively()
        {
            while (true)
            {
                Console.Write("rhea> ");
                string input = Console.ReadLine();
                if (input == ":q")
                {
                    break;
                }
                else if (input == ":l")
                {
                    Console.Write("file name: ");
                    string fileName = Console.ReadLine();
                    if (fileName == "")
                    {
                        continue;
                    }
                    InterpretFile(fileName);
                }
                else
                {
                    TextReader reader = new StringReader(input);
                    Interpret(new SourceReader("<interactive>", reader), true);
                }
            }
        }
        
        public void InterpretFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("file not found: {0}", fileName);
                return;
            }
            using (TextReader reader = new StreamReader(fileName))
            {
                Interpret(new SourceReader(fileName, reader), false);
            }
        }
        
        public void Interpret(SourceReader reader, bool interactive)
        {
            try
            {
                Lexer lexer = new Lexer(reader);
                Parser parser = new Parser(lexer);
                while (true)
                {
                    IExpr expr = parser.Parse();
                    if (expr == null)
                    {
                        break;
                    }
                    Compiler compiler = new Compiler();
                    expr.Compile(compiler);
                    VM vm = new VM(
                        compiler.GetResult(),
                        SList.Nil<IValue>(),
                        mEnv,
                        SList.Nil<KeyValuePair<IValueFunc, IValueFunc>>()
                    );
                    IValue result = vm.Run();
                    if (interactive)
                    {
                        Console.WriteLine(" => {0}", result);
                    }
                }
            }
            catch (RheaException e)
            {
                Console.WriteLine("{0}: {1}", e.Info, e.Message);
            }
        }
    }
}
