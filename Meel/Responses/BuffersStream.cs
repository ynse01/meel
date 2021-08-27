using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Meel.Responses
{
    public class BuffersStream : Stream
    {
        private List<ArraySegment<byte>> buffers = new List<ArraySegment<byte>>();

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => 0L;

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            buffers.Add(new ArraySegment<byte>(buffer, offset, count));
        }

        public override string ToString()
        {
            return string.Concat(buffers.Select(buf => Encoding.ASCII.GetString(buf.Array, buf.Offset, buf.Count)));
        }
    }
}
