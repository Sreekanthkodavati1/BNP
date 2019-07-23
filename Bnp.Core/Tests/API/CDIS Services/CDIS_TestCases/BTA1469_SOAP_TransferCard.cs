using System;
using System.Collections.Generic;
using System.Linq;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1469_SOAP_TransferCard : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        bool makePrimary = false;
        bool deactivateMember = false;
        string newCardSql, cardCountsql, oldCardMemberStatusSql, tranCardStatus, tranCardisPrimary, cardCount, oldCardMemberStatus = "";

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_TransferCard")]
        [TestCategory("API_SOAP_TransferCard_Positive")]
        [TestMethod]
        public void BTA1469_ST1837_SOAP_TransferCard_ToAnotherMember_CardAsMarkPrimary()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            double elapsedtime = 0;
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
              
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName+ " and LoyatlyID is:" +vc[0].LoyaltyIdNumber);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output1 = cdis_Service_Method.GetCDISMemberGeneral();
                IList<VirtualCard> vc1 = output1.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output1.IpCode + ", Name: " + output1.FirstName + " and LoyatlyID is:" + vc1[0].LoyaltyIdNumber);
                Logger.Info("IpCode: " + output1.IpCode + ", Name: " + output1.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Transferring Card through Transfer method";
                deactivateMember = false;
                makePrimary = true;
                Member response = cdis_Service_Method.TransferCard(vc[0].LoyaltyIdNumber, vc1[0].LoyaltyIdNumber, makePrimary, deactivateMember, elapsedtime);
                testStep.SetOutput("The following card has transferred to the member with identity :" + vc1[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                newCardSql = "Select * from LW_VIRTUALCARD where loyaltyidnumber = '"+ vc[0].LoyaltyIdNumber + "' and IPCODE = '"+ output1.IpCode.ToString()+"'";
                cardCountsql = "Select COUNT(*) from LW_VIRTUALCARD where IPCODE = '" + output1.IpCode.ToString() + "'";
                oldCardMemberStatusSql = "Select * from LW_LoyaltyMember where IPCODE = '" + output.IpCode.ToString() + "'";

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the transferred card and makeprimary status from database";
                tranCardStatus = DatabaseUtility.GetFromSoapDB("","", "", "Status", newCardSql);
                tranCardisPrimary = DatabaseUtility.GetFromSoapDB("", "", "", "Status", newCardSql);
                cardCount = DatabaseUtility.GetFromSoapDB("", "","", "COUNT(*)", cardCountsql);
                oldCardMemberStatus = DatabaseUtility.GetFromSoapDB("", "", "", "MemberSTATUS", oldCardMemberStatusSql);
                Assert.AreEqual("1", tranCardStatus, "Actual Value is :" + tranCardStatus + ", Expected Value is : 1");
                Assert.AreEqual("1", tranCardisPrimary, "Actual Value is :" + tranCardisPrimary + ", Expected Value is : 1");
                Assert.AreEqual("2", cardCount, "Actual Value is :" + cardCount + ", Expected Value is : 2");
                Assert.AreEqual("Active", (LoyaltyCard_Status)Int32.Parse(oldCardMemberStatus) + "", "Actual Value is :" + (LoyaltyCard_Status)Int32.Parse(oldCardMemberStatus)+", Expected Value is : Active");
                //Assert.IsTrue(elapsedtime>0, "Elapsed Time NOT greater than zero");

                testStep.SetOutput(" The Elapsed time is:" + elapsedtime + "ms" +
                                   "; transferred Card Status: "+ tranCardStatus+
                                   "; transferred isPrimaryStatus: "+ tranCardisPrimary+
                                   "; Total Cards for Second member: "+ cardCount+
                                   "; and Old Member Status is: "+ (LoyaltyCard_Status)Int32.Parse(oldCardMemberStatus));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message);
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }


        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_TransferCard")]
        [TestCategory("API_SOAP_TransferCard_Positive")]
        [TestMethod]
        public void BTA1469_ST1838_SOAP_TransferCard_ToAnotherMember()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            double elapsedtime = 0;
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName + " and LoyatlyID is:" + vc[0].LoyaltyIdNumber);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output1 = cdis_Service_Method.GetCDISMemberGeneral();
                IList<VirtualCard> vc1 = output1.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output1.IpCode + ", Name: " + output1.FirstName + " and LoyatlyID is:" + vc1[0].LoyaltyIdNumber);
                Logger.Info("IpCode: " + output1.IpCode + ", Name: " + output1.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Transferring Card through Transfer method";
                deactivateMember = false;
                makePrimary = false;
                Member response = cdis_Service_Method.TransferCard(vc[0].LoyaltyIdNumber, vc1[0].LoyaltyIdNumber, makePrimary, deactivateMember, elapsedtime);
                testStep.SetOutput("The following card has transferred to the member with identity :" + vc1[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                newCardSql = "Select * from LW_VIRTUALCARD where loyaltyidnumber = '" + vc[0].LoyaltyIdNumber + "' and IPCODE = '" + output1.IpCode.ToString() + "'";
                cardCountsql = "Select COUNT(*) from LW_VIRTUALCARD where IPCODE = '" + output1.IpCode.ToString() + "'";
                oldCardMemberStatusSql = "Select * from LW_LoyaltyMember where IPCODE = '" + output.IpCode.ToString() + "'";

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the transferred card and makeprimary status from database";
                tranCardStatus = DatabaseUtility.GetFromSoapDB("", "", "", "Status", newCardSql);
                cardCount = DatabaseUtility.GetFromSoapDB("", "", "", "COUNT(*)", cardCountsql);
                oldCardMemberStatus = DatabaseUtility.GetFromSoapDB("", "", "", "MemberSTATUS", oldCardMemberStatusSql);
                Assert.AreEqual("1", tranCardStatus, "Actual Value is :" + tranCardStatus + ", Expected Value is : 1");
                Assert.AreEqual("2", cardCount, "Actual Value is :" + cardCount + ", Expected Value is : 2");
                Assert.AreEqual("Active", (LoyaltyCard_Status)Int32.Parse(oldCardMemberStatus) + "", "Actual Value is :" + (LoyaltyCard_Status)Int32.Parse(oldCardMemberStatus) + ", Expected Value is : Active");
                //Assert.IsTrue(elapsedtime>0, "Elapsed Time NOT greater than zero");

                testStep.SetOutput(" The Elapsed time is:" + elapsedtime + "ms" +
                                   "; transferred Card Status: " + tranCardStatus +
                                   "; Total Cards for Second member: " + cardCount +
                                   "; and Old Member Status is: " + (LoyaltyCard_Status)Int32.Parse(oldCardMemberStatus));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message);
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_TransferCard")]
        [TestCategory("API_SOAP_TransferCard_Positive")]
        [TestMethod]
        public void BTA1469_ST1839_SOAP_TransferCard_ToAnotherMember_DeactivateOriginalMember()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            double elapsedtime = 0;
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName + " and LoyatlyID is:" + vc[0].LoyaltyIdNumber);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output1 = cdis_Service_Method.GetCDISMemberGeneral();
                IList<VirtualCard> vc1 = output1.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output1.IpCode + ", Name: " + output1.FirstName + " and LoyatlyID is:" + vc1[0].LoyaltyIdNumber);
                Logger.Info("IpCode: " + output1.IpCode + ", Name: " + output1.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Transferring Card through Transfer method";
                deactivateMember = true;
                makePrimary = false;
                Member response = cdis_Service_Method.TransferCard(vc[0].LoyaltyIdNumber, vc1[0].LoyaltyIdNumber, makePrimary, deactivateMember, elapsedtime);
                testStep.SetOutput("The following card has transferred to the member with identity :" + vc1[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                newCardSql = "Select * from LW_VIRTUALCARD where loyaltyidnumber = '" + vc[0].LoyaltyIdNumber + "' and IPCODE = '" + output1.IpCode.ToString() + "'";
                cardCountsql = "Select COUNT(*) from LW_VIRTUALCARD where IPCODE = '" + output1.IpCode.ToString() + "'";
                oldCardMemberStatusSql = "Select * from LW_LoyaltyMember where IPCODE = '" + output.IpCode.ToString() + "'";

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the transferred card and makeprimary status from database";
                tranCardStatus = DatabaseUtility.GetFromSoapDB("", "", "", "Status", newCardSql);
                cardCount = DatabaseUtility.GetFromSoapDB("", "", "", "COUNT(*)", cardCountsql);
                oldCardMemberStatus = DatabaseUtility.GetFromSoapDB("", "", "", "MemberSTATUS", oldCardMemberStatusSql);
                Assert.AreEqual("1", tranCardStatus, "Actual Value is :" + tranCardStatus + ", Expected Value is : 1");
                Assert.AreEqual("2", cardCount, "Actual Value is :" + cardCount + ", Expected Value is : 2");
                Assert.AreEqual("Inactive", (LoyaltyCard_Status)Int32.Parse(oldCardMemberStatus) + "", "Actual Value is :" + (LoyaltyCard_Status)Int32.Parse(oldCardMemberStatus) + ", Expected Value is : Inactive");
                //Assert.IsTrue(elapsedtime>0, "Elapsed Time NOT greater than zero");

                testStep.SetOutput(" The Elapsed time is:" + elapsedtime + "ms" +
                                   "; transferred Card Status: " + tranCardStatus +
                                   "; Total Cards for Second member: " + cardCount +
                                   "; and Old Member Status is: " + (LoyaltyCard_Status)Int32.Parse(oldCardMemberStatus));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message);
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_TransferCard")]
        [TestCategory("API_SOAP_TransferCard_Negative")]
        [TestMethod]
        public void BTA1469_ST1840_SOAP_TransferCard_ToSameMember()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            double elapsedtime = 0;
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output1 = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output1.IpCode + ", Name: " + output1.FirstName);
                Logger.Info("IpCode: " + output1.IpCode + ", Name: " + output1.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                IList<VirtualCard> vc = output.GetLoyaltyCards();
                IList<VirtualCard> vc1 = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Transferring Card through Transfer method";
                makePrimary = true;
                deactivateMember = false;
                string response = (string)cdis_Service_Method.TransferCardNegative(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, makePrimary, deactivateMember, elapsedtime);
                string[] errors = response.Split(';');
                string[] errorssplit = errors[0].Split('=');
                string[] errorsnew = errors[1].Split('=');

                testStep.SetOutput(response);
                Logger.Info(response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the error code and the ErrorMessage for the Error Code";
                Assert.AreEqual("3349", errorssplit[1], "Expected value is" + "3349" + "Actual value is" + errorssplit[1]);
                testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message);
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }
    }
}

    
