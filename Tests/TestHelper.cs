using System;
using System.IO;
using System.Reflection;

namespace Meel.Tests
{
    public static class TestHelper
    {
        public static Stream LoadEmbeddedResource(string path)
        {
            var asm = Assembly.GetExecutingAssembly();
            return asm.GetManifestResourceStream(path);
        }
    }
}
