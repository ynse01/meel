using System;
using System.Collections.Generic;

namespace Meel.Parsing
{
    public static class SequenceSetParser
    {
        public static HashSet<int> ParseBySequenceId(ReadOnlySpan<byte> input, int messageCount)
        {
            // RFC3501 Page 89: sequence-set 
            // Example: a message sequence number set of
            // 2,4:7,9,12:* for a mailbox with 15 messages is
            // equivalent to 2,4,5,6,7,9,12,13,14,15
            // Example: a message sequence number set of *:4,5:7
            // for a mailbox with 10 messages is equivalent to
            // 10,9,8,7,6,5,4,5,6,7 and MAY be reordered and
            // overlap coalesced to be 4,5,6,7,8,9,10.
            var set = new HashSet<int>();
            while(!input.IsEmpty)
            {
                var partIndex = input.IndexOf(LexiConstants.Comma);
                ReadOnlySpan<byte> sequence;
                if (partIndex == -1)
                {
                    sequence = input;
                    partIndex = input.Length;
                    input = ReadOnlySpan<byte>.Empty;
                } else
                {
                    sequence = input.Slice(0, partIndex);
                    input = input.Slice(partIndex + 1);
                }
                var index = sequence.IndexOf(LexiConstants.Colon);
                if (index >= 1)
                {
                    var leftSpan = sequence.Slice(0, index);
                    var rightSpan = sequence.Slice(index + 1);
                    int lower = -1;
                    int upper = -1;
                    if (leftSpan[0] == LexiConstants.Asterisk)
                    {
                        lower = rightSpan.AsNumber();
                        upper = messageCount;
                    }
                    else if (rightSpan[0] == LexiConstants.Asterisk)
                    {
                        lower = leftSpan.AsNumber();
                        upper = messageCount;
                    }
                    else
                    {
                        lower = leftSpan.AsNumber();
                        upper = rightSpan.AsNumber();
                    }
                    // Make sure upper is higher then lower
                    if (upper < lower)
                    {
                        var temp = lower;
                        lower = upper;
                        upper = temp;
                    }
                    if (upper >= 0 && lower >= 0)
                    {
                        for (var i = lower; i <= upper; i++)
                        {
                            set.Add(i);
                        }
                    }
                }
                else
                {
                    // Expecting just a single number
                    set.Add(sequence.AsNumber());
                }
            }
            return set;
        }

        public static HashSet<string> ParseByUid(string sequenceSet, string maxUid)
        {
            // RFC3501 Page 89: sequence-set 
            // Example: a message sequence number set of
            // 2,4:7,9,12:* for a mailbox with 15 messages is
            // equivalent to 2,4,5,6,7,9,12,13,14,15
            // Example: a message sequence number set of *:4,5:7
            // for a mailbox with 10 messages is equivalent to
            // 10,9,8,7,6,5,4,5,6,7 and MAY be reordered and
            // overlap coalesced to be 4,5,6,7,8,9,10.
            var set = new HashSet<string>();
            var setParts = sequenceSet.Split(',');
            foreach (var sequence in setParts)
            {
                var seqParts = sequence.Split(':');
                if (seqParts.Length > 1)
                {
                    string lower = null;
                    string upper = null;
                    if (seqParts[0] == "*")
                    {
                        lower = seqParts[1];
                        upper = maxUid;
                    }
                    else if (seqParts[1] == "*")
                    {
                        lower = seqParts[0];
                        upper = maxUid;
                    }
                    else
                    {
                        lower = seqParts[0];
                        upper = seqParts[1];
                    }
                    if (upper != null && lower != null)
                    {
                        var uidList = InterpolateBetweenUids(lower, upper);
                        foreach(var uid in uidList) { 
                            set.Add(uid);
                        }
                    }
                }
                else
                {
                    set.Add(sequence);
                }
            }
            return null;
        }

        private static IEnumerable<string> InterpolateBetweenUids(string lower, string upper)
        {
            return new[] { lower, upper };
        }
    }
}
