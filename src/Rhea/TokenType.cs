namespace Rhea
{
    public enum TokenType
    {
        EOF        = -1,
        
        NewLine    = -2,
        
        Int        = -3,
        Identifier = -4,
        
        Def = -5,
        
        ExclamationMark = 33,
        Dollar          = 36,
        Percent         = 37,
        Ampersand       = 38,
        LeftParen       = 40,
        RightParen      = 41,
        Asterisk        = 42,
        Plus            = 43,
        Comma           = 44,
        Minus           = 45,
        Dot             = 46,
        Slash           = 47,
        Colon           = 58,
        Semicolon       = 59,
        LessThan        = 60,
        Equal           = 61,
        GreaterThan     = 62,
        QuestionMark    = 63,
        AtMark          = 64,
        LeftBracket     = 91,
        RightBracket    = 93,
        Hat             = 94,
        LeftBrace       = 123,
        Bar             = 124,
        RightBrace      = 125,
        Tilda           = 126,
    }
}
