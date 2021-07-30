
using System;

namespace Meel.Parsing
{
    public class AsciiComparer
    {
        public static bool CompareIgnoreCase(ReadOnlySpan<byte> input, ReadOnlySpan<byte> expected)
        {
            bool result = false;
            if (input.Length == expected.Length)
            {
                result = true;
                for(var i = 0; i < input.Length; i++)
                {
                    if (!CompareIgnoreCase(input[i], expected[i]))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        public static bool CompareIgnoreCase(byte input, byte expected)
        {
            bool result;
            if (input > 0x60 && input < 0x7b)
            {
                result = expected == input - 0x20;
            }
            else
            {
                result = expected == input;
            }
            return result;
        }
    }
}
