using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Rhea
{
    public class Interpreter
    {
        private IEnv mEnv;
        
        public Interpreter()
        {
            mEnv = new EnvGlobal();
            mEnv.AddVariable("p", 1, (args, vm, info) => {
                Console.WriteLine(args[0]);
                vm.Push(args[0]);
            });
            mEnv.AddVariable("dynamic_wind", 3, (args, vm, info) => {
                ValueCont cont = vm.GetCont();
                var winder = new KeyValuePair<IValue, IValue>(args[0], args[2]);
                var winders = SList.Cons(winder, vm.Winders);
                Stack<IInsn> insnStack = new Stack<IInsn>();
                insnStack.Push(new InsnCall(0, info));
                insnStack.Push(InsnPop.Instance);
                insnStack.Push(new InsnSetWinders(winders));
                insnStack.Push(new InsnPush(args[1]));
                insnStack.Push(new InsnCall(0, info));
                insnStack.Push(new InsnCall(1, info));
                vm.Insns = insnStack.ToSList();
                vm.Stack = SList.List<IValue>(args[0], cont);
            });
            mEnv.AddVariable("callcc", 1, (args, vm, info) => {
                IValueFunc func = args[0] as IValueFunc;
                if (func == null)
                {
                    throw new RheaException(
                        string.Format("function required, but got {0}", args[0]),
                        info
                    );
                }
                ValueCont cont = vm.GetCont();
                IList<IValue> newArgs = new List<IValue> { cont };
                func.Call(newArgs, vm, info);
            });
        }
        
        public void InterpretInteractively()
        {
            while (true)
            {
                Console.Write(">>> ");
                string input = Console.ReadLine();
                if (input == "")
                {
                    continue;
                }
                else if (input == ":q")
                {
                    break;
                }
                else if (input == ":l")
                {
                    Console.Write("file name: ");
                    string fileName = Console.ReadLine();
                    if (fileName == "" || fileName == ":q")
                    {
                        continue;
                    }
                    InterpretFile(fileName);
                }
                else if (input[input.Length - 1] == ':')
                {
                    input = InputMultiline(input);
                    InterpretString(input, true);
                }
                else
                {
                    InterpretString(input, true);
                }
            }
        }
        
        public void InterpretString(string input, bool interactive)
        {
            TextReader reader = new StringReader(input);
            Interpret(new SourceReader("<interactive>", reader), interactive);
        }
        
        private string InputMultiline(string firstLine)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}", firstLine, Environment.NewLine);
            string input;
            do
            {
                Console.Write("... ");
                input = Console.ReadLine();
                sb.AppendFormat("{0}{1}", input, Environment.NewLine);
            }
            while (input != "end");
            return sb.ToString();
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
                        SList.Nil<KeyValuePair<IValue, IValue>>()
                    );
                    IValue result = vm.Run();
                    if (result == null)
                    {
                        break;
                    }
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
