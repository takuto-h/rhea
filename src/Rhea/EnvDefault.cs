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
            this.AddVariable("load",         1, Subrs.Primitive.Load);
            
            this.AddVariable("Int", Klasses.Int);
            this.AddMethod(Klasses.Int, "+", 2, Subrs.Int.Add);
            this.AddMethod(Klasses.Int, "-", 2, Subrs.Int.Sub);
            this.AddMethod(Klasses.Int, "*", 2, Subrs.Int.Mul);
            this.AddMethod(Klasses.Int, "==",  2, Subrs.Int.Eq);
            
            this.AddVariable("True", Klasses.True);
            this.AddVariable("true", ValueTrue.Instance);
            this.AddMethod(Klasses.True, "match_bool", 3, Subrs.True.MatchBool);
            
            this.AddVariable("False", Klasses.False);
            this.AddVariable("false", ValueFalse.Instance);
            this.AddMethod(Klasses.False, "match_bool", 3, Subrs.False.MatchBool);
            
            this.AddVariable("None", Klasses.None);
            this.AddVariable("none", ValueNone.Instance);
            
            this.AddVariable("Symbol", Klasses.Symbol);
            this.AddVariable("String", Klasses.String);
            this.AddVariable("Closure", Klasses.Closure);
            this.AddVariable("Subr", Klasses.Subr);
            this.AddVariable("Cont", Klasses.Cont);
            this.AddVariable("Func", Klasses.Func);
            this.AddVariable("Object", Klasses.Object);
        }
    }
}
