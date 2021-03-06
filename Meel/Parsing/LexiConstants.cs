using System;

namespace Meel.Parsing
{
    public static class LexiConstants
    {
        // Special characters
        public const byte Space = 0x20;
        public const byte CarrageReturn = 0x0d;
        public const byte NewLine = 0x0a;
        public const byte OpenParenthesis = 0x28;
        public const byte CloseParenthesis = 0x29;
        public const byte SquareOpenBrace = 0x5b;
        public const byte SquareCloseBrace = 0x5d;
        public const byte CurlyOpenBrace = 0x7b;
        public const byte CurlyCloseBrace = 0x7d;
        public const byte DoubleQuote = 0x22;
        public const byte Comma = 0x2c;
        public const byte Colon = 0x3a;
        public const byte Asterisk = 0x2a;
        public const byte Plus = 0x2b;
        public const byte Minus = 0x2d;
        public const byte Period = 0x2e;

        // Numbers
        public const byte Number0 = 0x30;
        public const byte Number1 = 0x31;
        public const byte Number2 = 0x32;
        public const byte Number3 = 0x33;
        public const byte Number4 = 0x34;
        public const byte Number5 = 0x35;
        public const byte Number6 = 0x36;
        public const byte Number7 = 0x37;
        public const byte Number8 = 0x38;
        public const byte Number9 = 0x39;
        
        // Letters
        public const byte A = 0x41;
        public const byte B = 0x42;
        public const byte C = 0x43;
        public const byte D = 0x44;
        public const byte E = 0x45;
        public const byte F = 0x46;
        public const byte G = 0x47;
        public const byte H = 0x48;
        public const byte I = 0x49;
        public const byte J = 0x4a;
        public const byte K = 0x4b;
        public const byte L = 0x4c;
        public const byte M = 0x4d;
        public const byte N = 0x4e;
        public const byte O = 0x4f;
        public const byte P = 0x50;
        public const byte Q = 0x51;
        public const byte R = 0x52;
        public const byte S = 0x53;
        public const byte T = 0x54;
        public const byte U = 0x55;
        public const byte V = 0x56;
        public const byte W = 0x57;
        public const byte X = 0x58;
        public const byte Y = 0x59;
        public const byte Z = 0x5a;

        public static readonly byte[] Nil = new byte[] { N, I, L };

        // Commands
        public static readonly byte[] Uid = new byte[] { U, I, D };
        public static readonly byte[] Capability = new byte[] { C, A, P, A, B, I, L, I, T, Y };
        public static readonly byte[] Login = new byte[] { L, O, G, I, N };
        public static readonly byte[] Logout = new byte[] { L, O, G, O, U, T };
        public static readonly byte[] Authenticate = new byte[] { A, U, T, H, E, N, T, I, C, A, T, E };
        public static readonly byte[] StartTls = new byte[] { S, T, A, R, T, T, L, S };
        public static readonly byte[] Noop = new byte[] { N, O, O, P };
        public static readonly byte[] Bad = new byte[] { B, A, D };
        public static readonly byte[] Check = new byte[] { C, H, E, C, K };
        public static readonly byte[] Close = new byte[] { C, L, O, S, E };
        public static readonly byte[] Expunge = new byte[] { E, X, P, U, N, G, E };
        public static readonly byte[] Copy = new byte[] { C, O, P, Y };
        public static readonly byte[] Fetch = new byte[] { F, E, T, C, H };
        public static readonly byte[] Store = new byte[] { S, T, O, R, E };
        public static readonly byte[] Search = new byte[] { S, E, A, R, C, H };
        public static readonly byte[] Append = new byte[] { A, P, P, E, N, D };
        public static readonly byte[] Create = new byte[] { C, R, E, A, T, E };
        public static readonly byte[] Delete = new byte[] { D, E, L, E, T, E };
        public static readonly byte[] Examine = new byte[] { E, X, A, M, I, N, E };
        public static readonly byte[] List = new byte[] { L, I, S, T };
        public static readonly byte[] LSub = new byte[] { L, S, U, B };
        public static readonly byte[] Rename = new byte[] { R, E, N, A, M, E };
        public static readonly byte[] Select = new byte[] { S, E, L, E, C, T };
        public static readonly byte[] Status = new byte[] { S, T, A, T, U, S };
        public static readonly byte[] Subscribe = new byte[] { S, U, B, S, C, R, I, B, E };
        public static readonly byte[] Unsubscribe = new byte[] { U, N, S, U, B, S, C, R, I, B, E };

        // Query paramerers
        public static readonly byte[] All = new byte[] { A, L, L };
        public static readonly byte[] Answered = new byte[] { A, N, S, W, E, R, E, D };
        public static readonly byte[] Bcc = new byte[] { B, C, C };
        public static readonly byte[] Before = new byte[] { B, E, F, O, R, E };
        public static readonly byte[] Body = new byte[] { B, O, D, Y };
        public static readonly byte[] Cc = new byte[] { C, C };
        public static readonly byte[] Deleted = new byte[] { D, E, L, E, T, E, D };
        public static readonly byte[] Flagged = new byte[] { F, L, A, G, G, E, D };
        public static readonly byte[] From = new byte[] { F, R, O, M };
        public static readonly byte[] Keyword = new byte[] { K, E, Y, W, O, R, D };
        public static readonly byte[] New = new byte[] { N, E, W };
        public static readonly byte[] Old = new byte[] { O, L, D };
        public static readonly byte[] On = new byte[] { O, N };
        public static readonly byte[] Recent = new byte[] { R, E, C, E, N, T };
        public static readonly byte[] Seen = new byte[] { S, E, E, N };
        public static readonly byte[] Since = new byte[] { S, I, N, C, E };
        public static readonly byte[] Subject = new byte[] { S, U, B, J, E, C, T };
        public static readonly byte[] Text = new byte[] { T, E, X, T };
        public static readonly byte[] To = new byte[] { T, O };
        public static readonly byte[] UnAnswered = new byte[] { U, N, A, N, S, W, E, R, E, D };
        public static readonly byte[] UnDeleted = new byte[] { U, N, D, E, L, E, T, E };
        public static readonly byte[] UnFlagged = new byte[] { U, N, F, L, A, G, G, E, D };
        public static readonly byte[] UnKeyword = new byte[] { U, N, K, E, W, O, R, D };
        public static readonly byte[] UnSeen = new byte[] { U, N, S,  E, E, N };
        public static readonly byte[] Draft = new byte[] { D, R, A, F, T };
        public static readonly byte[] Header = new byte[] { H, E, A, D, E, R };
        public static readonly byte[] Larger = new byte[] { L, A, R, G, E, R };
        public static readonly byte[] Not = new byte[] { N, O, T };
        public static readonly byte[] Or = new byte[] { O, R };
        public static readonly byte[] SentBefore = new byte[] { S, E, N, T, B, E, F, O, R, E };
        public static readonly byte[] SentOn = new byte[] { S, E, N, T, O, N };
        public static readonly byte[] SentSince = new byte[] { S, E, N, T, B, E, F, O, R, E };
        public static readonly byte[] Smaller = new byte[] { S, M, A, L, L, E, R };
        public static readonly byte[] UnDraft = new byte[] { U, N, D, R, A, F, T };

        // Data items
        public static readonly byte[] BodyStructure = new byte[] { B, O, D, Y, S, T, R, U, C, T, U, R, E };
        public static readonly byte[] BodyPeek = new byte[] { B, O, D, Y, Period, P, E, E, K };
        public static readonly byte[] Mime = new byte[] { M, I, M, E };
        public static readonly byte[] HeaderFields = new byte[] { H, E, A, D, E, R, Period, F, I, E, L, D, S };
        public static readonly byte[] HeaderFieldsNot = 
            new byte[] { H, E, A, D, E, R, Period, F, I, E, L, D, S, Period, N, O, T };

        public static bool IsDigit(byte input)
        {
            return input >= Number0 && input <= Number9;
        }
    }
}
