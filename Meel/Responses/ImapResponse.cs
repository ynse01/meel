using Meel.Parsing;
using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Text;

namespace Meel.Responses
{
    public ref struct ImapResponse
    {
        public static readonly byte[] Ok = Encoding.ASCII.GetBytes("OK");
        public static readonly byte[] No = Encoding.ASCII.GetBytes("NO");
        public static readonly byte[] Bad = Encoding.ASCII.GetBytes("BAD");
        public static readonly byte[] Bye = Encoding.ASCII.GetBytes("BYE");
        public static readonly byte[] Untagged = Encoding.ASCII.GetBytes("*");

        private PipeWriter writer;
        private Span<byte> buffer;
        private int offset;

        public ImapResponse(PipeWriter writer)
        {
            this.writer = writer;
            buffer = Span<byte>.Empty;
            offset = 0;
        }

        public void Allocate(int size)
        {
            if (writer != null)
            {
                buffer = writer.GetSpan(size);
            } else
            {
                buffer = new byte[size];
            }
        }

        public void Allocate(long size)
        {
            Allocate((int)size);
        }

        public void AppendLine()
        {
            buffer[offset++] = LexiConstants.CarrageReturn;
            buffer[offset++] = LexiConstants.NewLine;
        }

        public void AppendLine(ReadOnlySpan<byte> span)
        {
            span.CopyTo(buffer.Slice(offset));
            offset += span.Length;
            buffer[offset++] = LexiConstants.CarrageReturn;
            buffer[offset++] = LexiConstants.NewLine;
        }

        public void AppendLine(ReadOnlySpan<byte> span0, ReadOnlySpan<byte> span1)
        {
            span0.CopyTo(buffer.Slice(offset));
            offset += span0.Length;
            buffer[offset++] = LexiConstants.Space;
            span1.CopyTo(buffer.Slice(offset));
            offset += span1.Length;
            buffer[offset++] = LexiConstants.CarrageReturn;
            buffer[offset++] = LexiConstants.NewLine;
        }

        public void AppendLine(ReadOnlySpan<byte> span0, ReadOnlySpan<byte> span1, ReadOnlySpan<byte> span2)
        {
            span0.CopyTo(buffer.Slice(offset));
            offset += span0.Length;
            buffer[offset++] = LexiConstants.Space;
            span1.CopyTo(buffer.Slice(offset));
            offset += span1.Length;
            buffer[offset++] = LexiConstants.Space;
            span2.CopyTo(buffer.Slice(offset));
            offset += span2.Length;
            buffer[offset++] = LexiConstants.CarrageReturn;
            buffer[offset++] = LexiConstants.NewLine;
        }

        public void AppendLine(ReadOnlySequence<byte> span0, ReadOnlySequence<byte> span1)
        {
            span0.CopyTo(buffer.Slice(offset));
            offset += (int)span0.Length;
            buffer[offset++] = LexiConstants.Space;
            span1.CopyTo(buffer.Slice(offset));
            offset += (int)span1.Length;
            buffer[offset++] = LexiConstants.CarrageReturn;
            buffer[offset++] = LexiConstants.NewLine;
        }

        public void AppendLine(ReadOnlySpan<byte> span0, ReadOnlySequence<byte> span1)
        {
            span0.CopyTo(buffer.Slice(offset));
            offset += span0.Length;
            buffer[offset++] = LexiConstants.Space;
            span1.CopyTo(buffer.Slice(offset));
            offset += (int)span1.Length;
            buffer[offset++] = LexiConstants.CarrageReturn;
            buffer[offset++] = LexiConstants.NewLine;
        }


        public void AppendLine(ReadOnlySequence<byte> span0, ReadOnlySpan<byte> span1)
        {
            span0.CopyTo(buffer.Slice(offset));
            offset += (int)span0.Length;
            buffer[offset++] = LexiConstants.Space;
            span1.CopyTo(buffer.Slice(offset));
            offset += span1.Length;
            buffer[offset++] = LexiConstants.CarrageReturn;
            buffer[offset++] = LexiConstants.NewLine;
        }

        public void AppendLine(ReadOnlySequence<byte> span0, ReadOnlySpan<byte> span1, ReadOnlySpan<byte> span2)
        {
            span0.CopyTo(buffer.Slice(offset));
            offset += (int)span0.Length;
            buffer[offset++] = LexiConstants.Space;
            span1.CopyTo(buffer.Slice(offset));
            offset += span1.Length;
            buffer[offset++] = LexiConstants.Space;
            span2.CopyTo(buffer.Slice(offset));
            offset += span2.Length;
            buffer[offset++] = LexiConstants.CarrageReturn;
            buffer[offset++] = LexiConstants.NewLine;
        }

        public void AppendLine(ReadOnlySequence<byte> span0, ReadOnlySpan<byte> span1, ReadOnlySpan<byte> span2, ReadOnlySpan<byte> span3)
        {
            span0.CopyTo(buffer.Slice(offset));
            offset += (int)span0.Length;
            buffer[offset++] = LexiConstants.Space;
            span1.CopyTo(buffer.Slice(offset));
            offset += span1.Length;
            buffer[offset++] = LexiConstants.Space;
            span2.CopyTo(buffer.Slice(offset));
            offset += span2.Length;
            buffer[offset++] = LexiConstants.Space;
            span3.CopyTo(buffer.Slice(offset));
            offset += span3.Length;
            buffer[offset++] = LexiConstants.CarrageReturn;
            buffer[offset++] = LexiConstants.NewLine;
        }

        public void AppendSpace()
        {
            buffer[offset++] = LexiConstants.Space;
        }

        public void Append(byte value)
        {
            buffer[offset++] = value;
        }

        public void Append(string str)
        {
            var temp = Encoding.ASCII.GetBytes(str);
            for(var i = 0; i < temp.Length; i++)
            {
                buffer[offset++] = temp[i];
            }
        }

        public void Append(ReadOnlySpan<byte> span)
        {
            span.CopyTo(buffer.Slice(offset));
            offset += span.Length;
        }

        public void Append(ReadOnlySequence<byte> sequence)
        {
            sequence.CopyTo(buffer.Slice(offset));
            offset += (int)sequence.Length;
        }

        public Stream GetStream()
        {
            return writer?.AsStream(true);
        }

        public void SendToPipe()
        {
            writer?.Advance(offset);
        }

        /// <summary>
        /// Return the entire contant of this response.
        /// </summary>
        public override string ToString()
        {
            var span = buffer.Slice(0, offset);
            return Encoding.ASCII.GetString(span);
        }
    }
}
