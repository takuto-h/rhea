using System.Collections.Generic;

namespace Rhea
{
    public class ValueClosure : IValueFunc
    {
        private IList<ValueSymbol> mParams;
        private ISList<IInsn> mInsns;
        private IEnv mEnv;
        private SourceInfo mInfo;
        
        public ValueSymbol Klass
        {
            get { return ValueSymbol.Intern("Closure"); }
        }
        
        public ValueClosure(
            IList<ValueSymbol> paras,
            ISList<IInsn> insns,
            IEnv env,
            SourceInfo info
        )
        {
            mParams = paras;
            mInsns = insns;
            mEnv = env;
            mInfo = info;
        }
        
        public void Call(IList<IValue> args, VM vm, SourceInfo info)
        {
            ValueCont cont = vm.GetDynamicContext();
            vm.SetStaticContext(
                Assign(args, info).Append(mInsns),
                SList.List<IValue>(cont),
                new EnvLocal(mEnv)
            );
        }
        
        private ISList<IInsn> Assign(IList<IValue> args, SourceInfo info)
        {
            Stack<IInsn> insnStack = new Stack<IInsn>();
            IEnumerator<IValue> argsEtor = args.GetEnumerator();
            foreach (ValueSymbol selector in mParams)
            {
                if (!argsEtor.MoveNext())
                {
                    throw new RheaException(
                        this.WrongNumberOfArguments(mParams.Count, args.Count), info
                    );
                }
                insnStack.Push(new InsnPush(argsEtor.Current));
                insnStack.Push(new InsnDefVar(selector, mInfo));
                insnStack.Push(InsnPop.Instance);
            }
            if (argsEtor.MoveNext())
            {
                throw new RheaException(
                    this.WrongNumberOfArguments(mParams.Count, args.Count), info
                );
            }
            return insnStack.ToSList();
        }
        
        public string Show()
        {
            return string.Format("#<closure {0}>", mInfo);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
