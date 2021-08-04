using System;
using Meel.DataItems;

namespace Meel.Parsing
{
    public static class DataItemsParser
    {
        public static DataItem Parse(ReadOnlySpan<byte> span)
        {
            DataItem result = null;
            var byte0 = AsciiComparer.ToUpper(span[0]);
            if (byte0 == LexiConstants.B)
            {
                var byte4 = AsciiComparer.ToUpper(span[4]);
                if (byte4 == LexiConstants.Space)
                {
                    result = new BodyDataItem();
                } else if (byte4 == LexiConstants.S)
                {
                    result = new BodyStructureDataItem();
                } else if (byte4 == LexiConstants.SquareOpenBrace)
                {
                    result = new BodySectionDataItem();
                }
            } else if (byte0 == LexiConstants.E)
            {
                result = new EnvelopeDataItem();
            } else if (byte0 == LexiConstants.F)
            {
                result = new FlagsDataItem();
            } else if (byte0 == LexiConstants.I)
            {
                result = new InternalDateDataItem();
            } else if (byte0 == LexiConstants.R)
            {
                var byte6 = AsciiComparer.ToUpper(span[6]);
                var byte7 = AsciiComparer.ToUpper(span[7]);
                if (AsciiComparer.ToUpper(byte6) == LexiConstants.Space)
                {
                    result = new BodyDataItem();
                } else if (byte7 == LexiConstants.H) {
                    result = new Rfc822HeaderDataItem();
                } else if (byte7 == LexiConstants.S) {
                    result = new Rfc822SizeDataItem();
                }
            } else if (byte0 == LexiConstants.U)
            {
                result = new UidDataItem();
            }
            return result;
        }
    }
}
