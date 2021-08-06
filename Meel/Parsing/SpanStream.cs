using System;
using System.IO;

namespace Meel.Parsing
{
    public unsafe class SpanStream : Stream
    {
        public static void StreamTo(ref Span<byte> span, Action<Stream> callback)
        {
            int length;
            fixed (byte* ptr = span)
            {
                using (var stream = new SpanStream(ptr, span.Length))
                {
                    callback(stream);
                    length = (int)stream.Position;
                }
            }
            span = span.Slice(length);
        }

        private readonly byte* buffer;
        private readonly int length;
        private int position;

        public SpanStream(byte* pointer, int length)
        {
            buffer = pointer;
            this.length = length;
        }

        public override bool CanRead => false;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public override long Length => length;

        public override long Position
        {
            get { return position; }
            set { position = (int)value; }
        }

        public override void Flush()
        {
            // Nothing to do here.
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    position = (int)offset;
                    break;
                case SeekOrigin.Current:
                    position += (int)offset;
                    break;
                case SeekOrigin.End:
                    position = length - (int)offset;
                    break;
            }
            return position;
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException("Length cannot be set.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var end = offset + count;
            if (end < length)
            {
                for (var i = offset; i < end; i++)
                {
                    buffer[position++] = buffer[i];
                }
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }
    }
}
