using System.Buffers;
using Meel.Commands;

namespace Meel.Parsing
{
    public static class CommandParser
    {
        public static ImapCommands Parse(ReadOnlySequence<byte> data, out ReadOnlySequence<byte> options)
        {
            options = ReadOnlySequence<byte>.Empty;
            return ImapCommands.Noop;
        }
    }
}
