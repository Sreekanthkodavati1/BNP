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
	public class BTA221_CDIS_AddMemberCoupon : ProjectTestBase
	{
		TestCase testCase;
		List<TestStep> listOfTestSteps = new List<TestStep>();
		TestStep testStep;

		[TestMethod]
		public void BTA221_CDIS_AddMemberCoupon_Positive()
		{
			testCase = new TestCase(TestContext.TestName);
			listOfTestSteps = new List<TestStep>();
			testStep = new TestStep();
			String stepName = "";
            double time = 0;
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
				stepName = "Getting Coupon Definitions from Service";
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions();
                //   var reqCoupon = cdis_Service_Method.GetCouponDefinition(1010, null, null, null, string.Empty, out time);
                CouponDefinitionStruct[] coupondefintions = def.CouponDefinition;
                CouponDefinitionStruct reqCoupon = null;
                foreach (var coupon in coupondefintions)
                {
                    if (coupon.UsesAllowed > 10)
                    {
                        reqCoupon = coupon;
                        break;
                    }
                }

                testStep.SetOutput("Coupon Name CertNumbers: " + reqCoupon.Name);
				Logger.Info("Coupon Name CertNumbers: " + reqCoupon.Name);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Adding Coupon to a member from Service";
				long memberCouponId= cdis_Service_Method.AddMemberCoupon(vc[0].LoyaltyIdNumber, reqCoupon.Id);
				testStep.SetOutput("MemberCoupon Id : " + memberCouponId);
				Logger.Info("MemberCoupon Id : " + memberCouponId);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response from database";
				String dbresponse = DatabaseUtility.GetMemberCouponIdUsingVCKeyFromDBSOAP(output.IpCode+"");
				testStep.SetOutput("Member coupon id from database:" + dbresponse);

				Assert.AreEqual(memberCouponId + "", dbresponse, "Expected value is" + memberCouponId + "Actual value is" + dbresponse);
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