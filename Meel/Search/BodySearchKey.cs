﻿using MimeKit.Text;
using System;
using Meel.Parsing;

namespace Meel.Search
{
    public class BodySearchKey : ISearchKey
    {
        private string needle;

        public BodySearchKey(ReadOnlySpan<byte> needle)
        {
            this.needle = needle.AsString();
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return message.Message.GetTextBody(TextFormat.Text).Contains(needle, StringComparison.OrdinalIgnoreCase);
        }
    }
}
