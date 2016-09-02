using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;

namespace OrcasTrxApi
{
    public class TrxLoader
    {
        static string file = @"..\..\SampleVS90.trx.xml";
        XNamespace ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2006";
        public static void Main()
        {
//            DemoXmlSerializer();
            new TrxLoader().PlaywithXmlLinq();
            Console.ReadLine();
        }

        void PlaywithXmlLinq()
        {
            XElement doc = XElement.Load(file);
            var unitTests = doc.Descendants(ns + "UnitTest").ToList<XElement>();
            Console.WriteLine(unitTests.Count);
            var unitTestResults = doc.Descendants(ns + "UnitTestResult").ToList<XElement>();
            Console.WriteLine(unitTestResults.Count);

            

            var result = from u in unitTests
                         let id = u.Element(ns + "Execution").Attribute("id").Value
                         let desc = u.Element(ns + "Description").Value
                         let testClass = u.Element(ns + "TestMethod").Attribute("className").Value
                         join r in unitTestResults                         
                         on  id equals r.Attribute("executionId").Value  
                         orderby  testClass                            
                         select new {   Clase = testClass, 
                                        Test = u.Attribute("name").Value,
                                        Description = desc,
                                        Status = r.Attribute("outcome").Value };


            


            foreach (var item in result)
            {
                Console.WriteLine("{0}\t{1}\t{2}",item.Clase,item.Test,item.Status);
            }

            var gresult = result.GroupBy(c => c.Clase);

            foreach (var i1 in gresult)
            {
                Console.WriteLine(i1.Key);
                foreach (var i2 in i1)
                {
                    Console.WriteLine("\t " + i2.Test + " " + i2.Description);

                }
            }


            /*
            var tests = from u in doc.Descendants(xns + "TestMethod")
                        orderby u.Attribute("className").Value
                        select new { TestName = u.Attribute("name"), Class = u.Attribute("className") };
            foreach (var t in tests)
            {
                Console.WriteLine(t.Class.Value + " " + t.TestName.Value);
            }
            */

        }

        private static void DemoXmlSerializer()
        {
            
            //XmlTypeMapping map = new XmlTypeMapping();
            //see http://msdn2.microsoft.com/en-us/library/system.xml.serialization.xmlattributeoverrides.aspx
            XmlAttributeOverrides o = new XmlAttributeOverrides();
            XmlSerializer ser = new XmlSerializer(typeof(TestRunType));

            object tr = ser.Deserialize(File.OpenRead(file));
        }
    }
}
