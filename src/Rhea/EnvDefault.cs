namespace Rhea
{
    public class EnvDefault : EnvGlobal
    {
        public EnvDefault()
        {
            this.AddVariable("p",            1, false, Subrs.Primitive.P);
            this.AddVariable("dynamic_wind", 3, false, Subrs.Primitive.DynamicWind);
            this.AddVariable("callcc",       1, false, Subrs.Primitive.Callcc);
            this.AddVariable("load",         1, false, Subrs.Primitive.Load);
            
            this.AddVariable("Int", Klasses.Int);
            this.AddMethod(Klasses.Int, "+", 2, false, Subrs.Int.Add);
            this.AddMethod(Klasses.Int, "-", 2, false, Subrs.Int.Sub);
            this.AddMethod(Klasses.Int, "*", 2, false, Subrs.Int.Mul);
            this.AddMethod(Klasses.Int, "==", 2, false, Subrs.Int.Eq);
            
            this.AddVariable("True", Klasses.True);
            this.AddVariable("true", ValueTrue.Instance);
            this.AddMethod(Klasses.True, "match_bool", 3, false, Subrs.True.MatchBool);
            
            this.AddVariable("False", Klasses.False);
            this.AddVariable("false", ValueFalse.Instance);
            this.AddMethod(Klasses.False, "match_bool", 3, false, Subrs.False.MatchBool);
            
            this.AddVariable("UndefinedObject", Klasses.UndefinedObject);
            this.AddVariable("undefined_object", ValueUndef.Instance);
            
            this.AddVariable("Symbol", Klasses.Symbol);
            this.AddVariable("make_symbol", 1, false, Subrs.Symbol.MakeSymbol);
            
            this.AddVariable("Array", Klasses.Array);
            this.AddVariable("make_array", 0, true, Subrs.Array.MakeArray);
            
            this.AddVariable("String", Klasses.String);
            this.AddVariable("Closure", Klasses.Closure);
            this.AddVariable("Subr", Klasses.Subr);
            this.AddVariable("Cont", Klasses.Cont);
            this.AddVariable("Func", Klasses.Func);
            this.AddVariable("Object", Klasses.Object);
        }
    }
}
