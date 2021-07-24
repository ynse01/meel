using System;

namespace Meel.Parsing
{
    public class LexiConstants
    {
        public static byte Space => 0x20;

        public static byte[] UID { get; } = new byte[] { 0x55, 0x49, 0x44 };
    }
}
