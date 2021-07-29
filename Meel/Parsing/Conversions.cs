using System;
using System.Buffers;
using System.Text;


namespace Meel.Parsing
{
    public static class Conversions
    {
        public static Span<byte> AsSpan(this string txt)
        {
            return Encoding.ASCII.GetBytes(txt);
        }

        public static string AsString(this ReadOnlySequence<byte> sequence)
        {
            string result;
            if (sequence.IsSingleSegment)
            {
                result = AsString(sequence.FirstSpan);
            }
            else
            {
                result = AsString(sequence.ToArray());
            }
            return result;
        }

        public static string AsString(this ReadOnlySpan<byte> span)
        {
            return Encoding.ASCII.GetString(span);
        }

        public static Span<byte> AsSpan(this int number)
        {
            return AsSpan(number.ToString());
        }

    }
}
