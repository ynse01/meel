using System;
using System.Buffers;
using System.Text;


namespace Meel.Parsing
{
    public static class Conversions
    {
        public static string AsString(this ReadOnlySequence<byte> sequence)
        {
            return AsString(AsSpan(sequence));
        }

        public static string AsString(this ReadOnlySpan<byte> span)
        {
            return Encoding.ASCII.GetString(span);
        }

        public static Span<byte> AsSpan(this string txt)
        {
            return Encoding.ASCII.GetBytes(txt);
        }

        public static Span<byte> AsSpan(this int number)
        {
            return AsSpan(number.ToString());
        }

        public static ReadOnlySpan<byte> AsSpan(this ReadOnlySequence<byte> sequence)
        {
            ReadOnlySpan<byte> result;
            if (sequence.IsSingleSegment)
            {
                result = sequence.FirstSpan;
            } else
            {
                result = sequence.ToArray();
            }
            return result;
        }
    }
}
