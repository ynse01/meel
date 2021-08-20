using Meel.Responses;
using MimeKit;
using System;

namespace Meel.Parsing
{
    public static class Rfc822Formatter
    {
        public static bool TryFormat(DateTimeOffset dateTimeOffset, ref ImapResponse response)
        {
            var day = dateTimeOffset.Day.AsSpan();
            var month = GetMonthAsSpan(dateTimeOffset.Month);
            var year = dateTimeOffset.Year.AsSpan();
            var hour = dateTimeOffset.Hour.AsSpan();
            var min = dateTimeOffset.Minute.AsSpan();
            var sec = dateTimeOffset.Second.AsSpan();
            var tzSign = Math.Sign(dateTimeOffset.Offset.Hours);
            var tzHour = Math.Abs(dateTimeOffset.Offset.Hours).AsSpan();
            var tzMin = dateTimeOffset.Offset.Minutes.AsSpan();
            // Date format " 1-May-2012"
            // Time format "hh:mm:ss +zzzz"
            if (day.Length == 1)
            {
                response.AppendSpace();
            }
            response.Append(day);
            response.Append(LexiConstants.Minus);
            response.Append(month);
            response.Append(LexiConstants.Minus);
            response.Append(year);
            response.AppendSpace();
            if (hour.Length == 1)
            {
                response.Append(LexiConstants.Number0);
            }
            response.Append(hour);
            response.Append(LexiConstants.Colon);
            if (min.Length == 1)
            {
                response.Append(LexiConstants.Number0);
            }
            response.Append(min);
            response.Append(LexiConstants.Colon);
            if (sec.Length == 1)
            {
                response.Append(LexiConstants.Number0);
            }
            response.Append(sec);
            response.AppendSpace();
            if (tzSign > 0)
            {
                response.Append(LexiConstants.Plus);
            } else
            {
                response.Append(LexiConstants.Minus);
            }
            if (tzHour.Length == 1)
            {
                response.Append(LexiConstants.Number0);
            }
            response.Append(tzHour);
            if (tzMin.Length == 1)
            {
                response.Append(LexiConstants.Number0);
            }
            response.Append(tzMin);
            return true;
        }

        public static bool TryFormat(InternetAddressList addresses, ref ImapResponse response)
        {
            response.Append(LexiConstants.OpenParenthesis);
            for(var i = 0; i < addresses.Count; i++)
            {
                var address = addresses[i];
                if (address is MailboxAddress)
                {
                    TryFormat((MailboxAddress)address, ref response);
                }
                else
                {
                    var innerAddresses = ((GroupAddress)address).Members;
                    TryFormat(innerAddresses, ref response);
                }
                if ((i + 1) == addresses.Count)
                {
                    response.Append(LexiConstants.CloseParenthesis);
                }
                else
                {
                    response.Append(LexiConstants.Space);
                }
            }
            return true;
        }

        public static bool TryFormat(MailboxAddress address, ref ImapResponse response)
        {
            var name = address.Name.AsAsciiSpan();
            var parts = address.Address.Split('@');
            var user = parts[0].AsAsciiSpan();
            var host = parts[1].AsAsciiSpan();
            response.Append(LexiConstants.OpenParenthesis);
            response.Append(LexiConstants.DoubleQuote);
            response.Append(name);
            response.Append(LexiConstants.DoubleQuote);
            response.AppendSpace();
            response.Append(LexiConstants.Nil);
            response.AppendSpace();
            response.Append(LexiConstants.DoubleQuote);
            response.Append(user);
            response.Append(LexiConstants.DoubleQuote);
            response.AppendSpace();
            response.Append(LexiConstants.DoubleQuote);
            response.Append(host);
            response.Append(LexiConstants.DoubleQuote);
            response.Append(LexiConstants.CloseParenthesis);
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
