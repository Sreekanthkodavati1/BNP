using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace Bnp.Core
{
    [TestClass]
    public class OrderExecutionTests : ProjectTestBase
    {

        public static string ordTestSuiteName = string.Empty;

        [TestMethod]
        public void OrderExecution_Navigator_Smoke()

        {
            JArray TotalTestcases = ReadTestcases("Web_Navigator_Smoke_TestCases.json");
            ordTestSuiteName = (string)TotalTestcases.Root.FirstOrDefault()["Execution"];
            RunTest(TotalTestcases);
         }

        public void OrderExecution_CSPortal_Smoke()

        {
            JArray TotalTestcases = ReadTestcases("Web_Navigator_Smoke_TestCases.json");
            ordTestSuiteName = (string)TotalTestcases.Root.FirstOrDefault()["Execution"];
            RunTest(TotalTestcases);
        }
        public void RunTest(JArray TestCases)
        {
            foreach (string testcase in TestCases)
            {
                string[] splitClassAndTestNames = testcase.Split('.');
                List<string> list = new List<string>(splitClassAndTestNames);
                list.RemoveAt(splitClassAndTestNames.Length - 1);
                list.RemoveAt(splitClassAndTestNames.Length - 2);
                string testNamespace = string.Join(".", list.ToArray());
                string testClass = splitClassAndTestNames[splitClassAndTestNames.Length - 2];
                string testCaseName = splitClassAndTestNames[splitClassAndTestNames.Length - 1];

                Type testType = Type.GetType(testNamespace + "." + testClass);
                var ctor = Activator.CreateInstance(testType, "true");
                if (ctor != null)
                {
                    MethodInfo methodInfo = testType.GetMethod(testCaseName);
                    // methodInfo.Invoke(ctor, new object[] { ProjectTestBase.dr });
                    methodInfo.Invoke(ctor, null);
                }
            }
        }

        public JArray ReadTestcases(string file)
        {
            string TestDatPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            JArray testcaseJObject = JsonConvert.DeserializeObject<JArray>(File.ReadAllText(@TestDatPath + "//" + file));
            JArray testcases = testcaseJObject[0].Value<JArray>("TestCases");
            return testcases;

        }

        public void WriteTestCases(JArray json, string file)
        {
            string TestDatPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(@TestDatPath + "//" + file, output);
        }
    }
}
