using System.IO;

namespace Rhea
{
    public class SourceReader
    {
        private TextReader mReader;
        
        public string Name { get; private set; }
        public int Line { get; private set; }
        public int Column { get; private set; }
        
        public SourceReader(string name, TextReader reader)
        {
            Name = name;
            Line = 1;
            Column = 1;
            mReader = reader;
        }
        
        public int Peek()
        {
            return mReader.Peek();
        }
        
        public int Read()
        {
            int c = mReader.Read();
            switch (c)
            {
            case '\n':
                Column = 1;
                Line++;
                break;
            case '\t':
                Column += 8;
                break;
            default:
                Column++;
                break;
            }
            return c;
        }
        
        public SourceInfo GetSourceInfo()
        {
            return new SourceInfo(Name, Line, Column);
        }
    }
}
