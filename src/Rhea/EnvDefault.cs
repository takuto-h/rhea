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
            this.AddVariable("False", Klasses.False);
            this.AddVariable("Nil", Klasses.Nil);
            
            this.AddVariable("Symbol", Klasses.Symbol);
            this.AddVariable("make_symbol", Subrs.Symbol.MakeSymbol, 1);
            
            this.AddVariable("Array", Klasses.Array);
            this.AddVariable("make_array", Subrs.Array.MakeArray, allowRest: true);
            this.AddMethod(Klasses.Array, "[]", Subrs.Array.GetItem, 2);
            this.AddMethod(Klasses.Array, "[]=", Subrs.Array.SetItem, 3);
            
            this.AddVariable("Hash", Klasses.Hash);
            this.AddVariable("make_hash", Subrs.Hash.MakeHash, allowKeys: true);
            this.AddMethod(Klasses.Hash, "[]", Subrs.Hash.GetItem, 2);
            this.AddMethod(Klasses.Hash, "[]=", Subrs.Hash.SetItem, 3);
            
            this.AddVariable("String", Klasses.String);
            this.AddVariable("Closure", Klasses.Closure);
            this.AddVariable("Subr", Klasses.Subr);
            this.AddVariable("Cont", Klasses.Cont);
            this.AddVariable("Func", Klasses.Func);
            
            this.AddVariable("Object", Klasses.Object);
            this.AddMethod(Klasses.Object, "classes", Subrs.Object.Klasses, 1);
            
            this.AddVariable("make_instance", Subrs.Instance.MakeInstance, 2);
        }
    }
}
