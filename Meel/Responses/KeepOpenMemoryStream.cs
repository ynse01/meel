using System.IO;

namespace Meel.Responses
{
    public class KeepOpenMemoryStream: MemoryStream 
    {
        public override void Close()
        {
            // Intentionally left empty
        }
    }
}
