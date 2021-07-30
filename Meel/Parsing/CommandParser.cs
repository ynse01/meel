using System;
using System.Buffers;
using Meel.Commands;

namespace Meel.Parsing
{
    public static class CommandParser
    {
        public static ImapCommands Parse(SequenceReader<byte> reader, out ReadOnlySpan<byte> options)
        {
            ImapCommands command;
            if (reader.TryReadTo(out ReadOnlySpan<byte> span, LexiConstants.Space, true))
            {
                options = reader.UnreadSpan;
                command = ReadCommand(span);
            }
            else
            {
                options = ReadOnlySpan<byte>.Empty;
                command = ReadCommand(reader.UnreadSpan);
            }
            return command;
        }

        public static ImapCommands Parse(ReadOnlySequence<byte> data, out ReadOnlySpan<byte> options)
        {
            return Parse(new SequenceReader<byte>(data), out options);
        }

        private static ImapCommands ReadCommand(ReadOnlySpan<byte> span)
        {
            ImapCommands command;
            if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Append))
            {
                command = ImapCommands.Append;
            } else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Authenticate))
            {
                command = ImapCommands.Authenticate;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Bad))
            {
                command = ImapCommands.Bad;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Capability))
            {
                command = ImapCommands.Capability;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Check))
            {
                command = ImapCommands.Check;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Close))
            {
                command = ImapCommands.Close;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Copy))
            {
                command = ImapCommands.Copy;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Create))
            {
                command = ImapCommands.Create;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Delete))
            {
                command = ImapCommands.Delete;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Examine))
            {
                command = ImapCommands.Examine;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Expunge))
            {
                command = ImapCommands.Expunge;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Fetch))
            {
                command = ImapCommands.Fetch;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.List))
            {
                command = ImapCommands.List;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Login))
            {
                command = ImapCommands.Login;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Logout))
            {
                command = ImapCommands.Logout ;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.LSub))
            {
                command = ImapCommands.LSub;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Noop))
            {
                command = ImapCommands.Noop;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Rename))
            {
                command = ImapCommands.Rename;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Search))
            {
                command = ImapCommands.Search;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Select))
            {
                command = ImapCommands.Select;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.StartTls))
            {
                command = ImapCommands.StartTls;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Status))
            {
                command = ImapCommands.Status;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Store))
            {
                command = ImapCommands.Store;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Subscribe))
            {
                command = ImapCommands.Subscribe;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Uid))
            {
                command = ImapCommands.Uid;
            }
            else if (AsciiComparer.CompareIgnoreCase(span, LexiConstants.Unsubscribe))
            {
                command = ImapCommands.Unsubscribe;
            }
            else
            {
                command = ImapCommands.Bad;
            }
            return command;
        }
    }
}
