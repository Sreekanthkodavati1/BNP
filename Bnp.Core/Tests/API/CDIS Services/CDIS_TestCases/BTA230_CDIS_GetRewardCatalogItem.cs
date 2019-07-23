using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA230_CDIS_GetRewardCatalogItem : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA230_CDIS_GetRewardCatalogItem_PositiveCase()

        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Dictionary<string, string> productListVerify = new Dictionary<string, string>();
            try
            {
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Reward_With_PartNumber"];

                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get a valid certificateNumber from LW_MemberCoupon table";
                string validRewardIDWithPartNumber = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "ID", sqlstmt);



                //Logger.Info("Test Method Started");
                //testStep = TestStepHelper.StartTestStep(testStep);
                //stepName = "Get Reward using GetRewardCatalog CDIS service";
                //RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRewardCatalog();
                //testStep.SetOutput("The RewardID is : " + rewardCatalog[0].RewardID + " and the reward name is :" + rewardCatalog[0].RewardName);
                //Logger.Info("RewardID:" + rewardCatalog[0].RewardID);
                //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                //listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Reward Catalog Item for the above reward with CDIS service";
                // RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRewardCatalog();
                RewardCatalogItemStruct rewardCatalogItem = cdis_Service_Method.GetRewardCatalogItem(long.Parse(validRewardIDWithPartNumber));
                testStep.SetOutput("The Product details for the above reward from response are ; ProductName: " + rewardCatalogItem.Product.ProductName +
                                                                         "; Product ID: " + rewardCatalogItem.Product.ProductID);
                                                                         //"; ProductVariantID: " + rewardCatalogItem.Product.ProductVariant[0].ProductVariantID +
                                                                         //"; ProductVariant_Description: " + rewardCatalogItem.Product.ProductVariant[0].Description +
                                                                         //"; ProductVariant_PartNumber: " + rewardCatalogItem.Product.ProductVariant[0].PartNumber +
                                                                         //"; ProductVariant_Quantity: " + rewardCatalogItem.Product.ProductVariant[0].Quantity +
                                                                         //"; ProductVariant_VariantOrder: " + rewardCatalogItem.Product.ProductVariant[0].VariantOrder);

                Logger.Info("ProductName: " + rewardCatalogItem.Product.ProductName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating reward details from DB";
    //            productListVerify = DatabaseUtility.GetRewardCatalogitemfromDBUsingIdSOAP(rewardCatalogItem.Product.ProductID + "");
                string id = DatabaseUtility.GetFromSoapDB("lw_product", "ID", rewardCatalogItem.Product.ProductID.ToString(), "ID", string.Empty);
                string name = DatabaseUtility.GetFromSoapDB("lw_product", "NAME", rewardCatalogItem.Product.ProductName.ToString(), "NAME", string.Empty);
                //               Assert.AreEqual(rewardCatalogItem.Product.ProductVariant[0].Description, productListVerify["ProductvariantDescription"], "Expected Value is " + rewardCatalogItem.Product.ProductVariant[0].Description + " Actual Value is " + productListVerify["ProductvariantDescription"]);
                Assert.AreEqual(rewardCatalogItem.Product.ProductName, name, "Expected Value is " + rewardCatalogItem.Product.ProductName + " Actual Value is " + name);
                Assert.AreEqual(rewardCatalogItem.Product.ProductID.ToString(), id, "Expected Value is " + rewardCatalogItem.Product.ProductID + " Actual Value is " + id);
                //Assert.AreEqual(rewardCatalogItem.Product.ProductVariant[0].ProductVariantID.ToString(), productListVerify["ProductvariantID"], "Expected Value is " + rewardCatalogItem.Product.ProductVariant[0].ProductVariantID + " Actual Value is " + productListVerify["ProductvariantID"]);
                //Assert.AreEqual(rewardCatalogItem.Product.ProductVariant[0].PartNumber, productListVerify["ProductvariantPartNumber"], "Expected Value is " + rewardCatalogItem.Product.ProductVariant[0].PartNumber + " Actual Value is " + productListVerify["ProductvariantPartNumber"]);
                //Assert.AreEqual(rewardCatalogItem.Product.ProductVariant[0].Quantity.ToString(), productListVerify["ProductvariantQuantity"], "Expected Value is " + rewardCatalogItem.Product.ProductVariant[0].Quantity + " Actual Value is " + productListVerify["ProductvariantQuantity"]);
                //Assert.AreEqual(rewardCatalogItem.Product.ProductVariant[0].VariantOrder.ToString(), productListVerify["ProductvariantOrder"], "Expected Value is " + rewardCatalogItem.Product.ProductVariant[0].VariantOrder + " Actual Value is " + productListVerify["ProductvariantOrder"]);

                testStep.SetOutput("The Product details from DB are ; ProductName: " + name +
                                                                         "; Product ID: " + id);
                                                                         //"; ProductVariantID: " + productListVerify["ProductvariantID"] +
                                                                         //"; ProductVariant_Description: " + productListVerify["ProductvariantDescription"] +
                                                                         //"; ProductVariant_PartNumber: " + productListVerify["ProductvariantPartNumber"] +
                                                                         //"; ProductVariant_Quantity: " + productListVerify["ProductvariantQuantity"] +
                                                                         //"; ProductVariant_VariantOrder: " + productListVerify["ProductvariantOrder"]);

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
                Assert.Fail(e.Message.ToString());
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
