﻿using Meel.Parsing;
using Meel.Responses;
using System;

namespace Meel.DataItems
{
    public class BodySectionDataItem : DataItem
    {
        private BodySection section;

        public BodySectionDataItem(BodySection section)
        {
            this.section = section;
        }

        public override ReadOnlySpan<byte> Name => LexiConstants.Body;

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            response.Append(Name);
            response.AppendSpace();

            BodySectionFormatter.FormatBodySection(message, ref response, section);
        }
    }
}
