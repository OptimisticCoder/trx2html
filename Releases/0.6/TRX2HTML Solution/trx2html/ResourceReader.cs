using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace trx2html
{
    internal class ResourceReader
    {
        internal static string LoadTextFromResource(string name)
        {
            string result = string.Empty;
            using (StreamReader sr = new StreamReader(
                   StreamFromResource(name)))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }

        internal static Stream StreamFromResource(string name)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("trx2html." + name);
        }
    }
}
