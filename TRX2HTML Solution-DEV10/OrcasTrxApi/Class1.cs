using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace OrcasTrxApi
{
    public class TrxLoader
    {
        public static void Main()
        {
            string file = @"..\..\Sample.trx";
            //XmlTypeMapping map = new XmlTypeMapping();
            //see http://msdn2.microsoft.com/en-us/library/system.xml.serialization.xmlattributeoverrides.aspx
            XmlAttributeOverrides o = new XmlAttributeOverrides();
            XmlSerializer ser = new XmlSerializer(typeof(TestRunType));

            object tr = ser.Deserialize(File.OpenRead(file));
        }
    }
}
