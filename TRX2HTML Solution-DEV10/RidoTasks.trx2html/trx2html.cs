using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace RidoTasks
{
    public class trx2html : Task
    {
        private string fileName;

        [Required]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public override bool Execute()
        {
            LogHeaderMessage();

            if (!File.Exists(fileName))
            {
                Log.LogError("TRX File not found {0}", fileName);
                return false;
            }

            try
            {
                Log.LogMessage("Creating HTML Report from TRX file: {0}", fileName);
                Transform(fileName, PrepareXsl());
                Log.LogMessage("Report generation Succeed. OutputFile: {0}.htm\n",  fileName);
                return true;
            }
            catch (Exception ex)
            {               
                Log.LogErrorFromException(ex);
                throw;
            }
        }

        private void LogHeaderMessage()
        {
            Log.LogMessage("trx2html  Creates HTML reports of VSTS TRX TestResults . (c)rido'08");
            Log.LogMessage("version: {0} \n", Assembly.GetExecutingAssembly().GetName().Version.ToString());            
        }



        private static void Transform(string fileName, XmlDocument xsl)
        {
            XslCompiledTransform x = new XslCompiledTransform();
            x.Load(xsl, new XsltSettings(true, true), null);
            x.Transform(fileName, fileName + ".htm");
        }
        private XmlDocument PrepareXsl()
        {
            XmlDocument xslDoc = new XmlDocument();
            xslDoc.Load(ResourceReader.StreamFromResource("OrcasTRX.xslt"));
            IncludeStyle(xslDoc);
            IncludeScript(xslDoc);
            return xslDoc;
        }

        private void IncludeScript(XmlDocument xslDoc)
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

        private  void IncludeStyle(XmlDocument xslDoc)
        {
            XmlNode headNode = xslDoc.GetElementsByTagName("head")[0];
            XmlNode linkNode = xslDoc.GetElementsByTagName("link")[0];
            XmlElement styleEl = xslDoc.CreateElement("style");
            styleEl.InnerText = ResourceReader.LoadTextFromResource(linkNode.Attributes["href"].Value);
            headNode.ReplaceChild(styleEl, linkNode);
        }

     
    }
}
