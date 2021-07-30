using System;

namespace Meel.Parsing
{
    public static class LexiConstants
    {
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
    }
}
