
using Meel.Parsing;
using System;

namespace Meel.DataItems
{
    public class BodySection
    {
        private int[] parts;
        private int partIndex;

        public BodySection()
        {
            parts = new int[5];
            partIndex = 0;
        }

        public BodySection(int[] parts, BodySubset subset)
        {
            this.parts = parts;
            partIndex = parts.Length;
            Subset = subset;
        }

        public void AddPart(uint num)
        {
            parts[partIndex] = (int)num;
            partIndex++;
        }

        public BodySubset Subset { get; set; }

        public ReadOnlySpan<int> Parts => parts;

        public int PartsDepth => partIndex;

        public byte[] ToArray()
        {
            byte[] result;
            var subset = SubsetToArray();
            var part = PartsToArray();
            if (subset.Length > 0)
            {
                var partLength = part.Length;
                result = new byte[subset.Length + partLength + 1];
                Array.Copy(part, result, partLength);
                result[partLength] = LexiConstants.Period;
                Array.Copy(subset, 0, result, partLength + 1, subset.Length);
            } else
            {
                result = part;
            }
            return result;
        }

        private byte[] SubsetToArray()
        {
            byte[] subset;
            switch (Subset)
            {
                case BodySubset.Header:
                    subset = LexiConstants.Header;
                    break;
                case BodySubset.HeaderFields:
                    subset = LexiConstants.HeaderFields;
                    break;
                case BodySubset.HeaderFieldsNot:
                    subset = LexiConstants.HeaderFieldsNot;
                    break;
                case BodySubset.Text:
                    subset = LexiConstants.Text;
                    break;
                case BodySubset.Mime:
                    subset = LexiConstants.Mime;
                    break;
                default:
                case BodySubset.None:
                    subset = new byte[0];
                    break;
            }
            return subset;
        }

        private byte[] PartsToArray()
        {
            // TODO: Support more parts
            byte[] arr = new byte[1];
            arr[0] = parts[0].AsSpan()[0];
            return arr;
        }
    }
}
