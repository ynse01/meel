﻿using System;

namespace Meel.Search
{
    public class NewSearchKey : ISearchKey
    {
        public bool Matches(ImapMessage message, int sequence)
        {
            return !message.Seen && message.Recent;
        }
    }
}
