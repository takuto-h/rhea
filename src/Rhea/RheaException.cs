using System;

namespace Rhea
{
    public class RheaException : Exception
    {
        public SourceInfo Info { get; private set; }
        
        public RheaException(SourceInfo info) : base()
        {
            Info = info;
        }
        
        public RheaException(string message, SourceInfo info) : base(message)
        {
            Info = info;
        }
        
        public RheaException(string message, Exception innerException, SourceInfo info)
          : base(message, innerException)
        {
            Info = info;
        }
    }
}
