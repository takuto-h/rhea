namespace Rhea
{
    public static class ValueBool
    {
        public static IValueBool ToValueBool(this bool boolean)
        {
            if (boolean)
            {
                return ValueTrue.Instance;
            }
            else
            {
                return ValueFalse.Instance;
            }
        }
    }
}
