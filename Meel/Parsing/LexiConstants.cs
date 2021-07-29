﻿using System;
using System.Buffers;
using System.Text;

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
        public const byte DoubleQuote = 0x22;

        public static readonly byte[] UID = new byte[] { 0x55, 0x49, 0x44 };

    }
}
