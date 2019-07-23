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
	public class BTA218_CDIS_GetMembersTiers : ProjectTestBase
	{
		TestCase testCase;
		List<TestStep> listOfTestSteps = new List<TestStep>();
		TestStep testStep;

		[TestMethod]
		public void BTA218_CDIS_GetMembersTiers_Positive()
		{
			testCase = new TestCase(TestContext.TestName);
			listOfTestSteps = new List<TestStep>();
			testStep = new TestStep();
			String stepName = "";
			Common common = new Common(this.DriverContext);
			CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
           
            try
			{
                string output = null;
                MemberTierStruct[] memberTier;
                Logger.Info("Test Method Started");

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get standard member LoyaltyId from Database";
                output = DatabaseUtility.GetFirstLoyaltyIDFromDBUSingSOAP(Tiers_EntryPoints.Standard);
				testStep.SetOutput("Standard members LoyaltyId from the sql query result is " + output);
				Logger.Info("Standard members LoyaltyId from the sql query result is : " + output);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "verify Standard memberTier for the above loyaltymember";
				memberTier = cdis_Service_Method.GetMemberTier(output);
				testStep.SetOutput("The TierName from the response of GetMemberTier method for the requested member with LoyaltyID \""+ output +"\" is \"" + memberTier[0].TierDef.Name+"\"");
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validate the standard Tier entrypoints using Enum";
				//var a = memberTier[0].TierDef.EntryPoints;
				testStep.SetOutput("Tier Name: " + memberTier[0].TierDef.Name + " and its EntryPoints is: " + memberTier[0].TierDef.EntryPoints + " and Exit Points is: " + memberTier[0].TierDef.ExitPoints);
				Assert.AreEqual(memberTier[0].TierDef.Name, (Tiers_EntryPoints)Int32.Parse(memberTier[0].TierDef.EntryPoints + "") + "", "Expected tier name" + (Tiers_EntryPoints)Int32.Parse(memberTier[0].TierDef.EntryPoints + "") + "Actual Tier Name is: " + memberTier[0].TierDef.Name);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get silver member LoyaltyId from Database";
                output = DatabaseUtility.GetFirstLoyaltyIDFromDBUSingSOAP(Tiers_EntryPoints.Silver);
                testStep.SetOutput("Standard members LoyaltyId from the sql query result is " + output);
                Logger.Info("Silver members LoyaltyId from the sql query result is : " + output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "verify silver memberTier for the above loyaltymember";
                memberTier = cdis_Service_Method.GetMemberTier(output);
                testStep.SetOutput("The TierName from the response of GetMemberTier method for the requested member with LoyaltyID \"" + output + "\" is \"" + memberTier[0].TierDef.Name + "\"");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the silver Tier entrypoints using Enum";
                //var a = memberTier[0].TierDef.EntryPoints;
                testStep.SetOutput("Tier Name: " + memberTier[0].TierDef.Name + " and its EntryPoints is: " + memberTier[0].TierDef.EntryPoints + " and Exit Points is: " + memberTier[0].TierDef.ExitPoints);
                Assert.AreEqual(memberTier[0].TierDef.Name, (Tiers_EntryPoints)Int32.Parse(memberTier[0].TierDef.EntryPoints + "") + "", "Expected tier name" + (Tiers_EntryPoints)Int32.Parse(memberTier[0].TierDef.EntryPoints + "") + "Actual Tier Name is: " + memberTier[0].TierDef.Name);
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