
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
            if (part.Length > 0)
            {
                var partLength = part.Length;
                result = new byte[subset.Length + partLength + 1];
                Array.Copy(part, result, partLength);
                result[partLength] = LexiConstants.Period;
                Array.Copy(subset, 0, result, partLength + 1, subset.Length);
            } else
            {
                result = subset;
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
            byte[] arr;
            if (partIndex > 0)
            {
                arr = new byte[(partIndex * 2) - 1];
                for (var i = 0; i < partIndex; i++)
                {
                    arr[2 * i] = parts[i].AsSpan()[0];
                    if (((2 * i) + 1) < arr.Length)
                    {
                        arr[(2 * i) + 1] = LexiConstants.Period;
                    }
                }
            } else
            {
                arr = new byte[0];
            }
            return arr;
        }
    }
}
