using BnPBaseFramework.Reporting.Utils;
using System;
using System.Collections.Generic;

using System.Text;


namespace BnPBaseFramework.Reporting.Base
{
    public class Report
    {
        public Report()
        {
            this.TestCases = new List<TestCase>();
            this.TestSteps = new List<TestStep>();
            this.TestSuites = new TestSuite();
        }


        public TestSuite TestSuites { get; set; }
        public IList<TestCase> TestCases { get; set; }
        public IList<TestStep> TestSteps { get; set; }
    }
}
