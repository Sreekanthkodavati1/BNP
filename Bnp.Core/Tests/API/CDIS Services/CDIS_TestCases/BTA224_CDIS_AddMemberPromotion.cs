using System;
using System.Collections.Generic;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA224_CDIS_AddMemberPromotion : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double time = 0;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA224_SOAP_AddMemberPromotion_Positive()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Promotion Definitions from Service";
                PromotionDefinitionStruct[] def = cdis_Service_Method.GetPromotionDefinitionsRecent(0);//the parameter is to provide the start index
                PromotionDefinitionStruct promot = new PromotionDefinitionStruct();
                foreach (PromotionDefinitionStruct pd in def)
                {
                    if (pd.Targeted)
                    {
                        promot = pd;
                        break;
                    }
                }
                testStep.SetOutput("Promotion to be added " + promot.Name);
                Logger.Info("Promotion to be added " + promot.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Promotion to a member from Service";
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, promot.Code);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                String dbresponse = DatabaseUtility.GetMemberPromotionsCodeUsingIdFromDBSOAP(output.IpCode + "");
                testStep.SetOutput("Member Promotion Code from database:" + dbresponse);
                Assert.AreEqual(promotionCode.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Negative")]
        [TestMethod]
        public void BTA1029_ST1086_SOAP_AddMemberPromotion_PromotionCodeNotExists()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Promotion to a member where promotion code does not exists and validate the Error code as: 3362";
                string promotionCode = common.RandomString(3);
                string error = cdis_Service_Method.AddMemberPromotionInvalid(vc[0].LoyaltyIdNumber, promotionCode, null, true, null, "Web", false, string.Empty, out time);
                if (error.Contains("Error code=3362") && error.Contains("Error Message=No content available that matches the specified criteria"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3362. Actual error received is" + error);
                }
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Negative")]
        [TestMethod]
        public void BTA1029_ST1088_SOAP_AddMemberPromotion_NullValues()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Promotion to a member with null fields and validate the error code as: 2003";
                string error = cdis_Service_Method.AddMemberPromotionInvalid(null, null, null, false, null, null, false, null, out time);
                if (error.Contains("Error code=2003") && error.Contains("Error Message=MemberIdentity of AddMemberPromotionIn is a required property"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 2003. Actual error received is" + error);
                }
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
        [TestCategory("AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Negative")]
        [TestMethod]
        public void BTA1029_ST1089_SOAP_AddMemberPromotion_MemberIdNotExists()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion to a member when MemberID does not exist and validate the Error code as 3302";
                //loyalty id should start with 6
                string loyaltyId = "6" + common.RandomNumber(6);
                string error = cdis_Service_Method.AddMemberPromotionInvalid(loyaltyId, cdis_Service_Method.getPromotionCode(), null, true, null, "Web", false, string.Empty, out time);
                testStep.SetOutput("Throws an expection with the error" + error);
                if (error.Contains("Error code=3302") && error.Contains("Error Message=Unable to find member with identity"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error:3302. Actual error received is" + error);
                }
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Negative")]
        [TestMethod]
        public void BTA1029_ST1090_SOAP_AddMemberPromotion_ChannelNotExistsReturnDefAsTrue()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion to a member with channel does not exists, return defination true and validate the Error code as 6003";
                //Sending Channel as Test
                string error = cdis_Service_Method.AddMemberPromotionInvalid(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), null, true, null, "Test", false, string.Empty, out time);
                if (error.Contains("Error code=6003") && error.Contains("Error Message=Specified channel is not defined"))
                {
                    testStep.SetOutput("The ErrorMessage from Service is received as expected. " + error);
                    Logger.Info("The ErrorMessage from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 6003. Actual error received is:\"" + error+"\"");
                }
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA1029_ST1101_SOAP_AddMemberPromotion_WithAllFieldsValidData()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion with the valid data in all fields";
                string certificateNmbr = "CN" + common.RandomNumber(5) + "Q" + common.RandomNumber(2);
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), certificateNmbr, true, "en", "Web", true, string.Empty, out time);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetMemberPromotionsCodeUsingIdFromDBSOAP(output.IpCode + "");
                testStep.SetOutput("Member Promotion Code from database:" + dbresponse);
                Assert.AreEqual(promotionCode.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA1029_ST1278_SOAP_AddMemberPromotion_WithExitingCertificateNumber()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion with the valid data in all fields";
                string certificateNmbr = "CN" + common.RandomNumber(5) + "Q" + common.RandomNumber(2);
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), certificateNmbr, true, "en", "Web", true, string.Empty, out time);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id + " , CertificateNumber:" + promotionCode.CertificateNmbr + " and Member IpCode:" + output.IpCode);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion to same member using Same Certificate Number";
                MemberPromotionStruct promotionCode1 = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), certificateNmbr, true, "en", "Web", true, string.Empty, out time);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode1.Id + " , CertificateNumber:" + promotionCode1.CertificateNmbr + " and Member IpCode:" + output.IpCode);
                Logger.Info("MemberPromotion Code : " + promotionCode1.Id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";

                string dbresponse = DatabaseUtility.GetFromSoapDB("lw_memberpromotion", "ID", promotionCode.Id.ToString(), "ID", string.Empty);
                string dbresponse1 = DatabaseUtility.GetFromSoapDB("lw_memberpromotion", "ID", promotionCode1.Id.ToString(), "ID", string.Empty);
                testStep.SetOutput("Member Promotion Codes from database are :" + dbresponse + " and "+ dbresponse1);
                Assert.AreEqual(promotionCode.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA1029_ST1102_SOAP_AddMemberPromotion_WithReturnDefinitionTrue()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion with Return defination as True";
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), null, true, null, null, null, null, out time);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetMemberPromotionsCodeUsingIdFromDBSOAP(output.IpCode + "");
                testStep.SetOutput("Member Promotion Code from database:" + dbresponse);
                Assert.AreEqual(promotionCode.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA1029_ST1103_SOAP_AddMemberPromotion_WithReturnDefinitionFalse()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion with Return defination as False";
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), null, false, null, null, false, null, out time);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetMemberPromotionsCodeUsingIdFromDBSOAP(output.IpCode + "");
                testStep.SetOutput("Member Promotion Code from database:" + dbresponse);
                Assert.AreEqual(promotionCode.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA1029_ST1104_SOAP_AddMemberPromotion_WithReturnAttributesTrueReturnDefTrue()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion with Return Attributes True and Return defination as True";
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), null, true, null, null, true, null, out time);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetMemberPromotionsCodeUsingIdFromDBSOAP(output.IpCode + "");
                testStep.SetOutput("Member Promotion Code from database:" + dbresponse);
                Assert.AreEqual(promotionCode.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA1029_ST1105_SOAP_AddMemberPromotion_WithReturnAttributesFalseReturnDefTrue()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion with Return Attributes False and Return defination as True";
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), null, true, null, null, false, null, out time);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetMemberPromotionsCodeUsingIdFromDBSOAP(output.IpCode + "");
                testStep.SetOutput("Member Promotion Code from database:" + dbresponse);
                Assert.AreEqual(promotionCode.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Negative")]
        [TestMethod]
        public void BTA1029_ST1100_SOAP_AddMemberPromotion_LanguageChannelNotExistsReturnDefAsTrue()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion to a member  with Language,channel does not exists," +
                    "retun defination true and validate the Error code as 6002";
                //Sending Language as ab and Channel as Test 
                string error = cdis_Service_Method.AddMemberPromotionInvalid(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), null, true, "ab", "Test", false, string.Empty, out time);
                if (error.Contains("Error code=6002") && error.Contains("Error Message=Specified language is not defined"))
                {
                    testStep.SetOutput("The Error Message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error:6002. Actual error received is:\"" + error + "\"");
                }
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA1029_ST1122_SOAP_AddMemberPromotion_ChannelExistsReturnDefAsTrue()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion to a member  with channel exists, retun defination true ";
                string channel = "Web";
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), null, true, null, channel, false, string.Empty, out time);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id + " Channel :" + channel);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id + " Channel :" + channel);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetMemberPromotionsCodeUsingIdFromDBSOAP(output.IpCode + "");
                testStep.SetOutput("Member Promotion Code from database:" + dbresponse);
                Assert.AreEqual(promotionCode.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA1029_ST1123_SOAP_AddMemberPromotion_LanguageExistsReturnDefAsTrue()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion to a member  with Language exists, retun defination true ";
                string language = "en";
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), null, true, language, null, false, string.Empty, out time);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id + " language :" + language);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id + " language :" + language);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetMemberPromotionsCodeUsingIdFromDBSOAP(output.IpCode + "");
                testStep.SetOutput("Member Promotion Code from database:" + dbresponse);
                Assert.AreEqual(promotionCode.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA1029_ST1124_SOAP_AddMemberPromotion_PromotionCodeExists()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                string promoCode = cdis_Service_Method.getPromotionCode();
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Promotion to a member from Service";
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, promoCode);
                testStep.SetOutput("IpCode: " + output.IpCode + " ,MemberPromotion Code Id: " + promotionCode.Id + " and Promotion code :" + promoCode);
                Logger.Info("IpCode:" + output.IpCode + "MemberPromotion Code Id: " + promotionCode.Id + " Promotion code :" + promoCode);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding a new member with CDIS service";
                Member output2 = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("Member IpCode:" + output2.IpCode + ",Name:" + output2.FirstName);
                Logger.Info("Member IpCode:" + output2.IpCode + ",Name:" + output2.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc1 = output2.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Promotion to a new member with Promotion Code exists";
                MemberPromotionStruct promotionCode2 = cdis_Service_Method.AddMemberPromotion(vc1[0].LoyaltyIdNumber, promoCode, null, false, null, null, false, string.Empty, out time);
                testStep.SetOutput("IpCode: " + output2.IpCode + " ,MemberPromotion Code Id: " + promotionCode2.Id + " and Promotion code :" + promoCode);
                Logger.Info("Member IpCode:" + output2.IpCode + "MemberPromotion Code : " + promotionCode2.Id + " Promotion code :" + promoCode);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetUpdatedMemberPromotionsCodeUsingIdFromDBSOAP(output2.IpCode + "");
                testStep.SetOutput("Member Promotion Code from database:" + dbresponse);
                Assert.AreEqual(promotionCode2.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA1029_ST1125_SOAP_AddMemberPromotion_MemberIdentityExists()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                string memberIdentity = vc[0].LoyaltyIdNumber;
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Promotion to a member from Service";
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(memberIdentity, cdis_Service_Method.getPromotionCode());
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id + " memberIdentity :" + memberIdentity);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id + " memberIdentity :" + memberIdentity);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Promotion to a member with Member Identity exists";
                promotionCode = cdis_Service_Method.AddMemberPromotion(memberIdentity, cdis_Service_Method.getPromotionCode(), null, false, null, null, false, string.Empty, out time);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id + " memberIdentity :" + memberIdentity);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id + " memberIdentity :" + memberIdentity);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetUpdatedMemberPromotionsCodeUsingIdFromDBSOAP(output.IpCode + "");
                testStep.SetOutput("Member Promotion Code from database:" + dbresponse);
                Assert.AreEqual(promotionCode.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Negative")]
        [TestMethod]
        public void BTA1029_ST1145_SOAP_AddMemberPromotion_LanguageNotExistsReturnDefAsTrue()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion to a member  with Language does not exists," +
                    "retun defination true and validate the Error code as 6002";
                //Sending Language as ab 
                string error = cdis_Service_Method.AddMemberPromotionInvalid(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), null, true, "ab", null, false, string.Empty, out time);
                if (error.Contains("Error code=6002") && error.Contains("Error Message=Specified language is not defined"))
                {
                    testStep.SetOutput("The Error Message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error:6002. Actual error received is" + error);
                }
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA1029_ST1151_SOAP_AddMemberPromotion_WithReturnDefinitionNull()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion with Return defination as Null";
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), null, null, null, null, false, null, out time);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetMemberPromotionsCodeUsingIdFromDBSOAP(output.IpCode + "");
                testStep.SetOutput("Member Promotion Code from database:" + dbresponse);
                Assert.AreEqual(promotionCode.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_AddMemberPromotion")]
        [TestCategory("AddMemberPromotion_Positive")]
        [TestMethod]
        public void BTA1029_ST1174_SOAP_AddMemberPromotion_verifyElapsedTime()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add member promotion with the valid data in all fields";
                string certificateNmbr = "CN" + common.RandomNumber(5) + "Q" + common.RandomNumber(2);
                double elapsedTime = 0;
                MemberPromotionStruct promotionCode = cdis_Service_Method.AddMemberPromotion(vc[0].LoyaltyIdNumber, cdis_Service_Method.getPromotionCode(), certificateNmbr, true, "en", "Web", true, string.Empty, out elapsedTime);
                testStep.SetOutput("MemberPromotion Code : " + promotionCode.Id);
                Logger.Info("MemberPromotion Code : " + promotionCode.Id);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetMemberPromotionsCodeUsingIdFromDBSOAP(output.IpCode + "");
                testStep.SetOutput("Member Promotion Code from database:" + dbresponse);
                Assert.AreEqual(promotionCode.Id + "", dbresponse, "Expected value is" + promotionCode.Id + "Actual value is" + dbresponse);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the elapsed time is greater than Zero";
                if (elapsedTime > 0)
                {
                    testStep.SetOutput("Elapsed time is greater than zero and the elapsed time is " + elapsedTime);
                }
                else
                {
                    throw new Exception("Elapsed time is not greater than Zero");
                }
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


