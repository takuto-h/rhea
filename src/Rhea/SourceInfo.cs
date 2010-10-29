namespace Rhea
{
    public class SourceInfo : IShowable
    {
        public string Name { get; private set; }
        public int Line { get; private set; }
        public int Column { get; private set; }
        
        public SourceInfo(string name, int line, int column)
        {
            Name = name;
            Line = line;
            Column = column;
        }
        
        public string Show()
        {
            return string.Format("{0}:{1}:{2}", Name, Line, Column);
        }
        
        public override string ToString()
        {
            return Show();
        }
    }
}
