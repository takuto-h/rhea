namespace Rhea
{
    public class EnvDefault : EnvGlobal
    {
        public EnvDefault()
        {
            this.AddVariable("p", Subrs.Primitive.P, 1);
            this.AddVariable("dynamic_wind", Subrs.Primitive.DynamicWind, 3);
            this.AddVariable("callcc", Subrs.Primitive.Callcc, 1);
            this.AddVariable("load", Subrs.Primitive.Load, 1);
            
            this.AddVariable("Int", Klasses.Int);
            this.AddMethod(Klasses.Int, "+", Subrs.Int.Add, 2);
            this.AddMethod(Klasses.Int, "-", Subrs.Int.Sub, 2);
            this.AddMethod(Klasses.Int, "*", Subrs.Int.Mul, 2);
            this.AddMethod(Klasses.Int, "==", Subrs.Int.Eq, 2);
            
            this.AddVariable("True", Klasses.True);
            this.AddVariable("true", ValueTrue.Instance);
            this.AddMethod(Klasses.True, "match_bool", Subrs.True.MatchBool, 3);
            
            this.AddVariable("False", Klasses.False);
            this.AddVariable("false", ValueFalse.Instance);
            this.AddMethod(Klasses.False, "match_bool", Subrs.False.MatchBool, 3);
            
            this.AddVariable("UndefinedObject", Klasses.UndefinedObject);
            this.AddVariable("undefined_object", ValueUndef.Instance);
            
            this.AddVariable("Symbol", Klasses.Symbol);
            this.AddVariable("make_symbol", Subrs.Symbol.MakeSymbol, 1);
            
            this.AddVariable("Array", Klasses.Array);
            this.AddVariable("make_array", Subrs.Array.MakeArray, allowRest: true);
            
            this.AddVariable("Hash", Klasses.Hash);
            this.AddVariable("make_hash", Subrs.Hash.MakeHash, allowKeys: true);
            
            this.AddVariable("String", Klasses.String);
            this.AddVariable("Closure", Klasses.Closure);
            this.AddVariable("Subr", Klasses.Subr);
            this.AddVariable("Cont", Klasses.Cont);
            this.AddVariable("Func", Klasses.Func);
            this.AddVariable("Object", Klasses.Object);
            
            this.AddVariable("make_instance", Subrs.Instance.MakeInstance, 2);
        }
    }
}
