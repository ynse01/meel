using MimeKit;
using System;

namespace Meel.Parsing
{
    public static class Rfc822Formatter
    {
        public static bool TryFormat(DateTimeOffset dateTimeOffset, Span<byte> destination, out int bytesWritten)
        {
            var day = dateTimeOffset.Day.AsSpan();
            var month = GetMonthAsSpan(dateTimeOffset.Month);
            var year = dateTimeOffset.Year.AsSpan();
            var hour = dateTimeOffset.Hour.AsSpan();
            var min = dateTimeOffset.Minute.AsSpan();
            var sec = dateTimeOffset.Second.AsSpan();
            var tzHour = dateTimeOffset.Offset.Hours.AsSpan();
            var tzMin = dateTimeOffset.Offset.Minutes.AsSpan();
            // Date format " 1-May-2012"
            // Time format "hh:mm:ss +zzzz"
            if (day.Length == 1)
            {
                destination[0] = LexiConstants.Space;
                destination[1] = day[0];
            } else
            {
                destination[0] = day[0];
                destination[1] = day[1];
            }
            destination[2] = LexiConstants.Minus;
            destination[3] = month[0];
            destination[4] = month[1];
            destination[5] = month[2];
            destination[6] = LexiConstants.Minus;
            destination[7] = year[0];
            destination[8] = year[1];
            destination[9] = year[2];
            destination[10] = year[3];
            destination[11] = LexiConstants.Space;
            if (hour.Length == 1)
            {
                destination[12] = LexiConstants.Number0;
                destination[13] = hour[0];
            } else
            {
                destination[12] = hour[0];
                destination[13] = hour[1];
            }
            destination[14] = LexiConstants.Colon;
            if (min.Length == 1)
            {
                destination[15] = LexiConstants.Number0;
                destination[16] = min[0];
            }
            else
            {
                destination[15] = min[0];
                destination[16] = min[1];
            }
            destination[17] = LexiConstants.Colon;
            if (sec.Length == 1)
            {
                destination[18] = LexiConstants.Number0;
                destination[19] = sec[0];
            }
            else
            {
                destination[18] = sec[0];
                destination[19] = sec[1];
            }
            destination[20] = LexiConstants.Space;
            if (dateTimeOffset.Offset.Hours > 0)
            {
                destination[21] = LexiConstants.Plus;
            } else
            {
                destination[21] = LexiConstants.Minus;
                tzHour = tzHour.Slice(1);
            }
            if (tzHour.Length == 1)
            {
                destination[22] = LexiConstants.Number0;
                destination[23] = tzHour[0];
            }
            else
            {
                destination[22] = tzHour[0];
                destination[23] = tzHour[1];
            }
            if (tzMin.Length == 1)
            {
                destination[24] = LexiConstants.Number0;
                destination[25] = tzMin[0];
            }
            else
            {
                destination[24] = tzMin[0];
                destination[25] = tzMin[1];
            }
            bytesWritten = 26;
            return true;
        }

        public static bool TryFormat(InternetAddressList addresses, Span<byte> destination, out int bytesWritten)
        {
            destination[0] = LexiConstants.OpenParenthesis;
            var span = destination.Slice(1);
            int addressBytesWritten;
            bytesWritten = 1;
            for(var i = 0; i < addresses.Count; i++)
            {
                var address = addresses[i];
                if (address is MailboxAddress)
                {
                    TryFormat((MailboxAddress)address, span, out addressBytesWritten);
                }
                else
                {
                    var innerAddresses = ((GroupAddress)address).Members;
                    TryFormat(innerAddresses, span, out addressBytesWritten);
                }
                if ((i + 1) == addresses.Count)
                {
                    span[addressBytesWritten] = LexiConstants.CloseParenthesis;
                }
                else
                {
                    span[addressBytesWritten] = LexiConstants.Space;
                }
                span = span.Slice(addressBytesWritten + 1);
                bytesWritten += addressBytesWritten + 1;
            }
            return true;
        }

        public static bool TryFormat(MailboxAddress address, Span<byte> destination, out int bytesWritten)
        {
            var name = address.Name.AsAsciiSpan();
            var parts = address.Address.Split('@');
            var user = parts[0].AsAsciiSpan();
            var host = parts[1].AsAsciiSpan();
            var span = destination;
            span[0] = LexiConstants.OpenParenthesis;
            span[1] = LexiConstants.DoubleQuote;
            span = span.Slice(2);
            name.CopyTo(span);
            span[name.Length] = LexiConstants.DoubleQuote;
            span[name.Length + 1] = LexiConstants.Space;
            span = span.Slice(name.Length + 2);
            LexiConstants.Nil.CopyTo(span);
            span[3] = LexiConstants.Space;
            span[4] = LexiConstants.DoubleQuote;
            span = span.Slice(5);
            user.CopyTo(span);
            span[user.Length] = LexiConstants.DoubleQuote;
            span[user.Length + 1] = LexiConstants.Space;
            span[user.Length + 2] = LexiConstants.DoubleQuote;
            span = span.Slice(user.Length + 3);
            host.CopyTo(span);
            span[host.Length] = LexiConstants.DoubleQuote;
            span[host.Length + 1] = LexiConstants.CloseParenthesis;
            bytesWritten = 14 + name.Length + user.Length + host.Length;
            return true;
        }

        private static ReadOnlySpan<byte> GetMonthAsSpan(int month)
        {
            ReadOnlySpan<byte> result;
            switch(month)
            {
                case 1:
                    result = LexiConstants.January;
                    break;
                case 2:
                    result = LexiConstants.February;
                    break;
                case 3:
                    result = LexiConstants.March;
                    break;
                case 4:
                    result = LexiConstants.April;
                    break;
                case 5:
                    result = LexiConstants.May;
                    break;
                case 6:
                    result = LexiConstants.June;
                    break;
                case 7:
                    result = LexiConstants.July;
                    break;
                case 8:
                    result = LexiConstants.August;
                    break;
                case 9:
                    result = LexiConstants.September;
                    break;
                case 10:
                    result = LexiConstants.October;
                    break;
                case 11:
                    result = LexiConstants.November;
                    break;
                case 12:
                    result = LexiConstants.December;
                    break;
                default:
                    result = ReadOnlySpan<byte>.Empty;
                    break;
            }
            return result;
        }
    }
}
