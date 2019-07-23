using BnPBaseFramework.Reporting.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BnPBaseFramework.Reporting.Utils
{
    public class TestCaseUtils
    {
        public static TestCase validateTestCaseStatus(List<TestStep> listOfTestSteps, TestCase testCase)
        {
            foreach (TestStep testStepToVerifyStatus in listOfTestSteps)
            {
                if (!testStepToVerifyStatus.IsStatus())
                {
                    testCase.SetStatus(false);
                    break;
                }
                else
                {
                    testCase.SetStatus(true);
                }

            }

            return testCase;
        }
    }
}
