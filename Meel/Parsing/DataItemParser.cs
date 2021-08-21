using System;
using Meel.DataItems;

namespace Meel.Parsing
{
    public static class DataItemParser
    {
        public static DataItem Parse(ReadOnlySpan<byte> span)
        {
            DataItem result = null;
            var byte0 = AsciiComparer.ToUpper(span[0]);
            if (byte0 == LexiConstants.A)
            {
                // Can be ALL
                result = MacroAll();
            } else if (byte0 == LexiConstants.B)
            {
                // Can be BODY, BODY[...], BODY.PEEK[...] or BODYSTRUCTURE
                var byte4 = AsciiComparer.ToUpper(span[4]);
                if (byte4 == LexiConstants.Space)
                {
                    result = new BodyDataItem();
                }
                else if (byte4 == LexiConstants.S)
                {
                    result = new BodyStructureDataItem();
                }
                else if (byte4 == LexiConstants.SquareOpenBrace)
                {
                    var section = ParseSection(span.Slice(4));
                    result = new BodySectionDataItem(section);
                }
                else if (byte4 == LexiConstants.Period)
                {
                    var section = ParseSection(span.Slice(8));
                    result = new BodyPeekDataItem(section);
                }
            }
            else if (byte0 == LexiConstants.E)
            {
                // Can be ENVELOPE
                result = new EnvelopeDataItem();
            }
            else if (byte0 == LexiConstants.F)
            {
                // Can be FAST, FULL or FLAGS.
                var byte1 = AsciiComparer.ToUpper(span[1]);
                if (byte1 == LexiConstants.A)
                {
                    result = MacroFast();
                } 
                else if (byte1 == LexiConstants.U)
                {
                    result = MacroFull();
                }
                else if (byte1 == LexiConstants.L)
                {
                    result = new FlagsDataItem();
                }
            }
            else if (byte0 == LexiConstants.I)
            {
                // Can be INTERNALDATE
                result = new InternalDateDataItem();
            }
            else if (byte0 == LexiConstants.R)
            {
                // Can be RFC822, RFC822.SIZE, RFC822.HEADER or RFC822.TEXT
                var byte6 = AsciiComparer.ToUpper(span[6]);
                var byte7 = AsciiComparer.ToUpper(span[7]);
                if (byte6 == LexiConstants.Space)
                {
                    result = new BodyDataItem();
                }
                else if (byte7 == LexiConstants.H)
                {
                    result = new Rfc822HeaderDataItem();
                }
                else if (byte7 == LexiConstants.S)
                {
                    result = new Rfc822SizeDataItem();
                }
                else if (byte7 == LexiConstants.T)
                {
                    result = new Rfc822TextDataItem();
                }
            }
            else if (byte0 == LexiConstants.U)
            {
                // Can be UID
                result = new UidDataItem();
            }
            return result;
        }

        private static DataItem MacroAll()
        {
            var left = new AggregatedDataItem(new FlagsDataItem(), new InternalDateDataItem());
            var right = new AggregatedDataItem(new Rfc822SizeDataItem(), new EnvelopeDataItem());
            return new AggregatedDataItem(left, right);
        }

        private static DataItem MacroFast()
        {
            var left = new AggregatedDataItem(new FlagsDataItem(), new InternalDateDataItem());
            var right = new Rfc822SizeDataItem();
            return new AggregatedDataItem(left, right);
        }

        private static DataItem MacroFull()
        {
            var left = new AggregatedDataItem(new FlagsDataItem(), new InternalDateDataItem());
            var middle = new AggregatedDataItem(new Rfc822SizeDataItem(), new EnvelopeDataItem());
            var right = new AggregatedDataItem(middle, new BodyDataItem());
            return new AggregatedDataItem(left, right);
        }

        private static BodySection ParseSection(ReadOnlySpan<byte> span)
        {
            var section = new BodySection();
            var index = span.IndexOf(LexiConstants.Period);
            while(index != -1) {
                var num = span.AsNumber();
                section.AddPart(num);
                span = span.Slice(index);
                index = span.IndexOf(LexiConstants.Period) + 1;
            }
            var byte0 = AsciiComparer.ToUpper(span[0]);
            if (byte0 == LexiConstants.H)
            {
                // Can be HEADER, HEADER.FIELDS or HEADER.FIELDS.NOT
                var byte6 = span[6];
                if (byte6 == LexiConstants.Period)
                {
                    var byte13 = span[13];
                    if (byte13 == LexiConstants.Period)
                    {
                        section.Subset = BodySubset.HeaderFieldsNot;
                    }
                    else
                    {
                        section.Subset = BodySubset.HeaderFields;
                    }
                } 
                else
                {
                    section.Subset = BodySubset.Header;
                }
            }
            else if (byte0 == LexiConstants.M)
            {
                section.Subset = BodySubset.Mime;
            }
            else if (byte0 == LexiConstants.T)
            {
                section.Subset = BodySubset.Text;
            }
            return null;
        }
    }
}
