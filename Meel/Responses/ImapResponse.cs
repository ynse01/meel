using Meel.Parsing;
using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Text;

namespace Meel.Responses
{
    public struct ImapResponse
    {
        public static readonly byte[] Ok = Encoding.ASCII.GetBytes("OK");
        public static readonly byte[] No = Encoding.ASCII.GetBytes("NO");
        public static readonly byte[] Bad = Encoding.ASCII.GetBytes("BAD");
        public static readonly byte[] Bye = Encoding.ASCII.GetBytes("BYE");
        public static readonly byte[] Untagged = Encoding.ASCII.GetBytes("*");

        private Stream stream;
        private byte[] buffer;
        private int offset;

        public ImapResponse(Stream stream)
        {
            this.stream = new KeepOpenStream(stream);
            buffer = null;
            offset = 0;
        }

        public Span<byte> Span => buffer;

        public int Length => offset;

        public void Allocate(int size)
        {
            buffer = new byte[size];
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

        public void AppendLine(byte value)
        {
            Append(value);
            AppendLine();
        }

        public void AppendLine(ReadOnlySpan<byte> span)
        {
            Append(span);
            AppendLine();
        }

        public void AppendLine(ReadOnlySpan<byte> span0, ReadOnlySpan<byte> span1)
        {
            Append(span0);
            AppendSpace();
            Append(span1);
            AppendLine();
        }

        public void AppendLine(ReadOnlySpan<byte> span0, ReadOnlySpan<byte> span1, ReadOnlySpan<byte> span2)
        {
            Append(span0);
            AppendSpace();
            Append(span1);
            AppendSpace();
            Append(span2);
            AppendLine();
        }

        public void AppendLine(ReadOnlySequence<byte> span0, ReadOnlySequence<byte> span1)
        {
            Append(span0);
            AppendSpace();
            Append(span1);
            AppendLine();
        }

        public void AppendLine(ReadOnlySpan<byte> span0, ReadOnlySequence<byte> span1)
        {
            Append(span0);
            AppendSpace();
            Append(span1);
            AppendLine();
        }


        public void AppendLine(ReadOnlySequence<byte> span0, ReadOnlySpan<byte> span1)
        {
            Append(span0);
            AppendSpace();
            Append(span1);
            AppendLine();
        }

        public void AppendLine(ReadOnlySequence<byte> span0, ReadOnlySpan<byte> span1, ReadOnlySpan<byte> span2)
        {
            Append(span0);
            AppendSpace();
            Append(span1);
            AppendSpace();
            Append(span2);
            AppendLine();
        }

        public void AppendLine(ReadOnlySequence<byte> span0, ReadOnlySpan<byte> span1, ReadOnlySpan<byte> span2, ReadOnlySpan<byte> span3)
        {
            Append(span0);
            AppendSpace();
            Append(span1);
            AppendSpace();
            Append(span2);
            AppendSpace();
            Append(span3);
            AppendLine();
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
            span.CopyTo(Span.Slice(offset));
            offset += span.Length;
        }

        public void Append(ReadOnlySequence<byte> sequence)
        {
            sequence.CopyTo(Span.Slice(offset));
            offset += (int)sequence.Length;
        }

        /// <summary>
        /// Move the specified number of bytes backwards.
        /// </summary>
        public void Rewind(int offset)
        {
            this.offset -= offset;
        }

        public Stream GetStream()
        {
            return stream;
        }

        public void SendToPipe()
        {
            stream.Write(buffer, 0, offset);
            stream.Flush();
        }

        /// <summary>
        /// Return the entire contant of this response.
        /// </summary>
        public override string ToString()
        {
            var span = Span.Slice(0, offset);
            return Encoding.ASCII.GetString(span);
        }
    }
}
