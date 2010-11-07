namespace Rhea
{
    public class EnvDefault : EnvGlobal
    {
        public EnvDefault()
        {
            this.AddVariable("p",            1, Subrs.Primitive.P);
            this.AddVariable("dynamic_wind", 3, Subrs.Primitive.DynamicWind);
            this.AddVariable("callcc",       1, Subrs.Primitive.Callcc);
            this.AddVariable("make_symbol",  1, Subrs.Primitive.MakeSymbol);
            this.AddVariable("make_object",  1, Subrs.Primitive.MakeObject);
            this.AddVariable("get_slot",     2, Subrs.Primitive.GetSlot);
            this.AddVariable("set_slot",     3, Subrs.Primitive.SetSlot);
            this.AddVariable("load",         1, Subrs.Primitive.Load);
            
            this.AddVariable("Int", ValueInt.GetKlass());
            this.AddMethod(ValueInt.GetKlass(), "__add__", 2, Subrs.Int.Add);
            this.AddMethod(ValueInt.GetKlass(), "__sub__", 2, Subrs.Int.Sub);
            this.AddMethod(ValueInt.GetKlass(), "__mul__", 2, Subrs.Int.Mul);
            this.AddMethod(ValueInt.GetKlass(), "__eq__",  2, Subrs.Int.Eq);
            
            this.AddVariable("True", ValueTrue.GetKlass());
            this.AddVariable("true", ValueTrue.Instance);
            this.AddMethod(ValueTrue.GetKlass(), "match_bool", 3, Subrs.True.MatchBool);
            
            this.AddVariable("False", ValueFalse.GetKlass());
            this.AddVariable("false", ValueFalse.Instance);
            this.AddMethod(ValueFalse.GetKlass(), "match_bool", 3, Subrs.False.MatchBool);
            
            this.AddVariable("Symbol", ValueSymbol.GetKlass());
            this.AddVariable("String", ValueString.GetKlass());
            this.AddVariable("Closure", ValueClosure.GetKlass());
            this.AddVariable("Subr", ValueSubr.GetKlass());
            this.AddVariable("Cont", ValueCont.GetKlass());
        }
    }
}
