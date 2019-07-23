using BnPBaseFramework.Reporting.Base;
using System;
using System.Configuration;
using System.IO;

namespace BnPBaseFramework.Reporting.Reports
{
    public class HtmlReportGenerator
    {        
        public String GenerateHTMLReport(TestSuite testSuite)
        {
            try
            {
                String currentDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
                String testSuiteStartTemplateBuffer = TemplatesForHtmlReport.GetTestSuiteStartTemplate();
                String testSuiteEndTemplateBuffer = TemplatesForHtmlReport.GetTestSuiteEndTemplate();                                
                String allTestSuiteHtml = this.BuildTestSuite(testSuiteStartTemplateBuffer.ToString(), testSuite);
                String allTestCasesHtml = this.BuildTestCasesAndTestSteps(currentDirectoryPath, testSuite);
                String testSuiteEnd = testSuiteEndTemplateBuffer.ToString();
                testSuiteEnd = testSuiteEnd.Replace("<%=PassedTestCases%>", testSuite.GetPassed().ToString());
                testSuiteEnd = testSuiteEnd.Replace("<%=FailedTestCases%>", testSuite.GetFailed().ToString());
                allTestSuiteHtml = allTestSuiteHtml + allTestCasesHtml;               
                allTestSuiteHtml = allTestSuiteHtml + testSuiteEnd;
                return allTestSuiteHtml;
            }
            catch (IOException e)
            {
                throw new Exception(("Not able to generate HTML report due to " + e.Message));
            }

        }       

        private String BuildTestSuite(String allTestSuiteHtml, TestSuite testSuite)
        {
            allTestSuiteHtml = allTestSuiteHtml.Replace("<%=PassedTestCases%>", testSuite.GetPassed().ToString());
            allTestSuiteHtml = allTestSuiteHtml.Replace("<%=FailedTestCases%>", testSuite.GetFailed().ToString());
            allTestSuiteHtml = allTestSuiteHtml.Replace("<%=Suite_Name%>", testSuite.GetSuiteName());
            allTestSuiteHtml = allTestSuiteHtml.Replace("<%=Suite_StartTime%>", testSuite.GetSuiteStartTime());
            allTestSuiteHtml = allTestSuiteHtml.Replace("<%=Suite_EndTime%>", testSuite.GetSuiteEndTime());
            //allTestSuiteHtml = allTestSuiteHtml.Replace("<%=Application_UnderTest%>", ConfigurationManager.AppSettings["ApplicationName"]);
            allTestSuiteHtml = allTestSuiteHtml.Replace("<%=Application_UnderTest%>", testSuite.ApplicationName);
            allTestSuiteHtml = allTestSuiteHtml.Replace("<%=Env_Name%>", testSuite.environmentName);
            allTestSuiteHtml = allTestSuiteHtml.Replace("<%=Build_Number%>", testSuite.buildVersion);
         //   allTestSuiteHtml = allTestSuiteHtml.Replace("<%=TestSuiteName%>", testSuite.SuiteName);
            if (testSuite.GetSuiteName().Trim().StartsWith("API") || testSuite.GetSuiteName().Trim().StartsWith("IO"))
            {
                allTestSuiteHtml = allTestSuiteHtml.Replace("<%=Browser_Name%>", " ");
            }
            else{
                allTestSuiteHtml = allTestSuiteHtml.Replace("<%=Browser_Name%>", ConfigurationManager.AppSettings["browser"]);
            }
            return allTestSuiteHtml;
        }

        private String BuildTestCasesAndTestSteps(String currentDirectoryPath, TestSuite testSuite)
        {
            String testCaseStartTemplateBuffer = TemplatesForHtmlReport.GetTestCaseStartTemplate();
            String testCaseEndTemplateBuffer = TemplatesForHtmlReport.GetTestCaseEndTemplate();
            String testStepTemplateBuffer = TemplatesForHtmlReport.GetTestStepTemplate();
            String screenShotsTemplateStart = TemplatesForHtmlReport.GetScreenShotsTemplate();
            String allTestCasesHtml = "";
            for (int testCaseIndex = 0; (testCaseIndex < testSuite.GetListOfTestCases().Count); testCaseIndex++)
            {
                TestCase testCase = testSuite.GetListOfTestCases()[testCaseIndex];
                String testCaseUniqueString = Guid.NewGuid().ToString();
                String testCaseStartHtml = testCaseStartTemplateBuffer.ToString();
                testCaseStartHtml = testCaseStartHtml.Replace("<%=Test_Case_Number%>", testCaseUniqueString);
                if ((testCase.GetTestCaseName().Length >= 35))
                {
                    testCaseStartHtml = testCaseStartHtml.Replace("<%=Test_Case_Name%>", testCase.GetTestCaseName().Substring(0, 35) + " .....");
                }
                else
                {
                    testCaseStartHtml = testCaseStartHtml.Replace("<%=Test_Case_Name%>", testCase.GetTestCaseName());
                }

                String[] testCases = testCase.GetTestCaseName().Split(' ');               
                testCaseStartHtml = testCaseStartHtml.Replace("<%=Test_Case_Name_Afer_Expansion%>", testCase.GetTestCaseName());               

                if (testCase.IsStatus())
                {
                    testCaseStartHtml = testCaseStartHtml.Replace(" <span class=\"label label-danger\" title=\"Failed\">Failed</span>", "");                   
                }
                else
                {
                    testCaseStartHtml = testCaseStartHtml.Replace("<span class=\"label label-success\" title=\"Passed\">Passed</span>", "");
                }

                if (testCase.GetErrorMessage() != null)
                {
                    testCaseStartHtml = testCaseStartHtml.Replace("<%=ErrorMessage%>", "<b>Error Message : </b>"
                                    + testCase.GetErrorMessage() + "");
                }
                else
                {
                    testCaseStartHtml = testCaseStartHtml.Replace("<%=ErrorMessage%>", "");
                }

                if (testCase.GetImageContent() == null || testCase.GetImageContent() =="")
                {
                    testCaseStartHtml = testCaseStartHtml.Replace("<%=ScreenShot%>", "");
                }
                else
                {
                    

                    testCaseStartHtml = testCaseStartHtml.Replace("<%=ScreenShot%>", "<a class=\"screenshot\" href=\"data:image/png;base64,"
                                    + testCase.GetImageContent() + "\">\n" + "                                        <img class=\"screenshot\" style=\"height:100%;width:100%\" id" +
                                    "=\"my_images\" src=\"data:image/png;base64,"
                                    + testCase.GetImageContent() + "\">\n" + "                                    </a>");
                }
                int passCount = 0;
                int failCount = 0;
                if ((testCase.GetTestCaseSteps() != null))
                {
                    for (int totalTestSteps = 0; totalTestSteps < testCase.GetTestCaseSteps().Count; totalTestSteps++)
                    {
                        if (testCase.GetTestCaseSteps()[totalTestSteps].IsStatus())
                        {
                            passCount++;
                        }
                        else
                        {
                            failCount++;
                        }
                    }
                }
                testCaseStartHtml = testCaseStartHtml.Replace("<%=Total_Test_Steps_Successful%>", passCount.ToString());
                testCaseStartHtml = testCaseStartHtml.Replace("<%=Total_Test_Steps_Fail%>", failCount.ToString());
                testCaseStartHtml = testCaseStartHtml.Replace("<%=Test_Case_Start%>", testCase.GetStartTime());
                testCaseStartHtml = testCaseStartHtml.Replace("<%=Test_Case_End%>", testCase.GetEndTime());
                testCaseStartHtml = testCaseStartHtml.Replace("<%=Test_Case_Status%>", testCase.IsStatus().ToString());
                String allTestStepsHtml = "";
                if ((testCase.GetTestCaseSteps() != null))
                {
                    for (int testStepIndex = 0; (testStepIndex < testCase.GetTestCaseSteps().Count); testStepIndex++)
                    {
                        String testStepUniqueString = Guid.NewGuid().ToString();
                        String testStepHtml = testStepTemplateBuffer.ToString();
                        TestStep testStep = testCase.GetTestCaseSteps()[testStepIndex];
                        testStepHtml = testStepHtml.Replace("<%=StepNumber%>", testStepUniqueString);

                        if ((testStep.GetTestStep().Length >= 35))
                        {
                            testStepHtml = testStepHtml.Replace("<%=Test_Step_Name%>", testStep.GetTestStep().Substring(0, 35) + " .....");
                            testStepHtml = testStepHtml.Replace("<%=Test_Step_Name_If_Wrapped%>", testStep.GetTestStep());
                        }
                        else
                        {
                            testStepHtml = testStepHtml.Replace("<%=Test_Step_Name%>", testStep.GetTestStep());
                            testStepHtml = testStepHtml.Replace("<%=Test_Step_Name_If_Wrapped%>", testStep.GetTestStep());
                        }
                        //testStepHtml = testStepHtml.Replace("<%=Test_Step_Name%>", testStep.GetTestStep().Replace(";", "</br>"));
                        if (testStep.IsStatus())
                        {
                            testStepHtml = testStepHtml.Replace(" <span class=\"label label-danger\" title=\"Failed\">Failed</span>", "");
                        }
                        else
                        {
                            testStepHtml = testStepHtml.Replace("<span class=\"label label-success\" title=\"Passed\">Passed</span>", "");
                        }

                        testStepHtml = testStepHtml.Replace("<%=Step_Start_Time%>", testStep.GetStepStartTime());
                        testStepHtml = testStepHtml.Replace("<%=Step_End_Time%>",testStep.GetStepEndTime());
                        if ((testStep.GetInput() != null))
                        {
                            testStepHtml = testStepHtml.Replace("<%=Step_Input>", testStep.GetInput().Replace(";", "</br>"));
                        }
                        else
                        {
                            testStepHtml = testStepHtml.Replace("<%=Step_Input>", "");
                        }

                        if ((testStep.GetOutput() != null))
                        {
                            testStepHtml = testStepHtml.Replace("<%=Output_Status%>", "<b>Output : </b>" + testStep.GetOutput().Replace(";", "</br>"));
                        }
                        else
                        {
                            testStepHtml = testStepHtml.Replace("<%=Output_Status%>", "");
                        }

                        if ((testStep.GetExpectedResult() != null))
                        {
                            testStepHtml = testStepHtml.Replace("<%=Step_Expected_Result%>", testStep.GetExpectedResult());
                        }
                        else
                        {
                            testStepHtml = testStepHtml.Replace("<%=Step_Expected_Result%>", "");
                        }

                        testStepHtml = testStepHtml.Replace("<%=Step_Status%>", testStep.IsStatus().ToString());
                        if (((testStep.GetImageContent() != null)
                                    && (testStep.GetImageContent().Length > 20)))
                        {
                            testStepHtml = testStepHtml.Replace("<%=ScreenShot%>",
                            "<a class=\"screenshot\" href=\"data:image/png;base64,"
                        + testStep.GetImageContent()
                        + "\">\n"
                        + "                                        <img class=\"screenshot\" style=\"height:100%;width:100%\" id=\"my_images\" src=\"data:image/png;base64,"
                        + testStep.GetImageContent()
                        + "\">\n"
                        + "                                    </a>");
                        }
                        else
                        {
                            testStepHtml = testStepHtml.Replace("<%=ScreenShot%>", "");
                        }

                        allTestStepsHtml = (allTestStepsHtml + testStepHtml);
                        if ((testStep.GetImages() != null)
                                    && (testStep.GetImages().Count > 0))
                        {
                            String screenShotUniqueString = Guid.NewGuid().ToString();
                            String screenShotString = screenShotsTemplateStart.ToString();
                            screenShotString = screenShotString.Replace("<%=ScreenShotNumber%>", screenShotUniqueString);
                            int imageCountIndex = 1;
                            foreach (String imageContent in testStep.GetImages())
                            {
                                screenShotString = screenShotString + "<b>Screenshot-"
                                            + imageCountIndex + "</b>";
                                screenShotString = screenShotString + "<a class=\"screenshot\" href=\"data:image/png;base64,"
                                            +imageContent + "\">\n" + "                                        <img class=\"screenshot\" style=\"height:100%;width:100%\" id" +
                                            "=\"my_images\" src=\"data:image/png;base64,"
                                            + imageContent + "\">\n" + "                                    </a>";
                                imageCountIndex++;
                            }

                            allTestStepsHtml = allTestStepsHtml + screenShotString;
                            allTestStepsHtml += "</div></div></div></div>";
                        }
                    }
                }
                if (((testCase.GetImages() != null)
                            && (testCase.GetImages().Count > 0)))
                {
                    String screenShotUniqueString = Guid.NewGuid().ToString();
                    String screenShotString = screenShotsTemplateStart.ToString();
                    screenShotString = screenShotString.Replace("<%=ScreenShotNumber%>", screenShotUniqueString);
                    int imageCountIndex = 1;
                    foreach (String imageContent in testCase.GetImages())
                    {
                        screenShotString = screenShotString + "<b>Screenshot-"
                                    + imageCountIndex + "</b>";
                        screenShotString = screenShotString
                              + "<a class=\"screenshot\" href=\"data:image/png;base64,"
                              + imageContent
                              + "\">\n"
                              + "                                        <img class=\"screenshot\" style=\"height:100%;width:100%\" id=\"my_images\" src=\"data:image/png;base64,"
                              + imageContent
                              + "\">\n"
                              + "                                    </a>";
                       imageCountIndex++;
                    }
                    allTestStepsHtml = allTestStepsHtml + screenShotString;
                    allTestStepsHtml += "</div></div></div></div>";
                }
                testCaseStartHtml = testCaseStartHtml + allTestStepsHtml;
                testCaseStartHtml = testCaseStartHtml + testCaseEndTemplateBuffer.ToString();
                allTestCasesHtml = allTestCasesHtml + testCaseStartHtml;
            }
            return allTestCasesHtml;
        }
    }
}
