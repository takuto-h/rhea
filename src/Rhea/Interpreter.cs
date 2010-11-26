using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Rhea
{
    public class Interpreter
    {
        private IEnv mEnv;
        
        public Interpreter(IEnv env)
        {
            mEnv = env;
        }
        
        public void InterpretInteractively()
        {
            int currentLineNumber = 1;
            while (true)
            {
                int startLineNumber = currentLineNumber;
                Console.Write("irh:{0:d3}> ", currentLineNumber++);
                string input = Console.ReadLine();
                if (input == "")
                {
                    continue;
                }
                else if (input == ":q")
                {
                    break;
                }
                else if (input[input.Length - 1] == ':')
                {
                    input = InputMultiline(input, ref currentLineNumber);
                    InterpretString(input, startLineNumber);
                }
                else
                {
                    InterpretString(input, startLineNumber);
                }
            }
        }
        
        private string InputMultiline(string firstLine, ref int currentLineNumber)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}", firstLine, Environment.NewLine);
            string input;
            do
            {
                Console.Write("irh:{0:d3}* ", currentLineNumber++);
                input = Console.ReadLine();
                sb.AppendFormat("{0}{1}", input, Environment.NewLine);
            }
            while (input != "end");
            return sb.ToString();
        }
        
        private bool InterpretString(string input, int startLineNumber)
        {
            TextReader reader = new StringReader(input);
            return Interpret(new SourceReader("<interactive>", startLineNumber, 1, reader), true);
        }
        
        public bool InterpretFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("file not found: {0}", fileName);
                return false;
            }
            using (TextReader reader = new StreamReader(fileName))
            {
                return Interpret(new SourceReader(fileName, 1, 1, reader), false);
            }
        }
        
        private bool Interpret(SourceReader reader, bool interactive)
        {
            try
            {
                Lexer lexer = new Lexer(reader);
                Parser parser = new Parser(lexer);
                while (true)
                {
                    ISList<IInsn> insns = Compile(parser);
                    if (insns == null)
                    {
                        return false;
                    }
                    IValue result = Run(insns);
                    if (result == null)
                    {
                        return false;
                    }
                    if (interactive)
                    {
                        Console.WriteLine(" => {0}", result);
                    }
                }
            }
            catch (EndOfStreamException)
            {
                return true;
            }
        }
        
        private ISList<IInsn> Compile(Parser parser)
        {
            try
            {
                IExpr expr = parser.Parse();
                if (expr == null)
                {
                    throw new EndOfStreamException();
                }
                Compiler compiler = new Compiler();
                expr.Compile(compiler);
                return compiler.GetResult();
            }
            catch (RheaException e)
            {
                Console.WriteLine("{0}: {1}", e.Info, e.Message);
                return null;
            }
        }
        
        private IValue Run(ISList<IInsn> insns)
        {
            VM vm = new VM(
                insns,
                SList.Nil<IValue>(),
                mEnv,
                SList.Nil<KeyValuePair<IValue, IValue>>()
            );
            while (true)
            {
                try
                {
                    return vm.Run();
                }
                catch (RheaException e)
                {
                    Console.WriteLine("{0}: {1}", e.Info, e.Message);
                    ValueCont cont = new ValueCont(
                        SList.Nil<IInsn>(),
                        SList.Nil<IValue>(),
                        vm.Env,
                        SList.Nil<KeyValuePair<IValue, IValue>>()
                    );
                    vm.SetCont(null, cont, e.Info);
                }
            }
        }
    }
}
