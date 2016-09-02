using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Reflection;
namespace trx2html
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("trx2html.exe \n  Create HTML reports of VSTS TestRuns. (c)rido'08");
            Console.WriteLine("version:" + Assembly.GetExecutingAssembly().GetName().Version.ToString()+ "\n");
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: trx2html <TestResult>.trx");
                return;
            }

            string fileName = args[0];
            VersionFinder v = new VersionFinder();
            SupportedFormats f = v.GetFileVersion(fileName);
            if (f == SupportedFormats.unknown)
            {
                Console.WriteLine("File {0} is not a recognized trx", fileName);
            }
            {
                Console.WriteLine("Processing {0} trx file", f.ToString());
                Transform(fileName, PrepareXsl(f.ToString()));
                Console.WriteLine("Tranformation Succeed. OutputFile: " + fileName + ".htm\n");
            }
        }

        private static void Transform(string fileName, XmlDocument xsl)
        {
            XslCompiledTransform x = new XslCompiledTransform();
            x.Load(xsl, new XsltSettings(true, true), null);
            x.Transform(fileName, fileName + ".htm");
        }

        private static XmlDocument PrepareXsl(string xslTemplate)
        {
            XmlDocument xslDoc = new XmlDocument();
            xslDoc.Load(ResourceReader.StreamFromResource(xslTemplate + ".xsl"));
            IncludeStyle(xslDoc);
            IncludeScript(xslDoc);
            return xslDoc;
        }

        private static void IncludeScript(XmlDocument xslDoc)
        {
            XmlNode scriptEl = xslDoc.GetElementsByTagName("script")[0];
            XmlAttribute scriptSrc = scriptEl.Attributes["src"];
            string script = ResourceReader.LoadTextFromResource(scriptSrc.Value);
            scriptEl.Attributes.Remove(scriptSrc);
#if !DEBUG
            script = script.Replace(";\r\n", "; ");
            script = script.Replace(",\r\n", ", ");
            script = script.Replace("}\r\n", "} ");
#endif
            scriptEl.InnerText = script;
        }

        private static void IncludeStyle(XmlDocument xslDoc)
        {
            XmlNode headNode = xslDoc.GetElementsByTagName("head")[0];
            XmlNode linkNode = xslDoc.GetElementsByTagName("link")[0];
            XmlElement styleEl = xslDoc.CreateElement("style");
            styleEl.InnerText = ResourceReader.LoadTextFromResource(linkNode.Attributes["href"].Value);
            headNode.ReplaceChild(styleEl, linkNode);
        }


    }
}
