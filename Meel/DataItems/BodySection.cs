
using System;

namespace Meel.DataItems
{
    public class BodySection
    {
        private int[] parts = new int[5];
        private int partIndex = 0;

        public void AddPart(uint num)
        {
            parts[partIndex] = (int)num;
            partIndex++;
        }

        public BodySubset Subset { get; set; }

        public ReadOnlySpan<int> Parts => parts;

        public int PartsDepth => partIndex;
    }
}
