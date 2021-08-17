using System;
using System.IO;

namespace Meel.Responses
{
    public class KeepOpenStream: MemoryStream 
    {
        public override void Close()
        {
            // Intentionally left empty
        }
    }
}
