using BnPBaseFramework.Reporting.Base;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Web;
using BnpBaseFramework.API.Utils;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace BnPBaseFramework.Reporting.DAL
{
    public class DataAccessLayer
    {
        public void Create(TestSuite ts)
        {

            try
            {
                using (var connection = new NpgsqlConnection("Server=10.4.10.31;Port=5432;Database=qa_automation;User Id=bta_dev;Password=Aut0*13Q;"))
                {
                    connection.Open();



                    string commandText = "INSERT INTO automation." + '"' + "ReportTestSuite" + '"' +
                                         "(TestSuit_id" + "," + '"' + "TestSuit_Name" + '"' + "," + '"' + "TestSuit_StartTime" + '"' + "," + '"' + "TestSuit_EndTime" +
                                         '"' + "," + '"' + "TestSuit_Environment" + '"' + "," + '"' + "TestSuit_Host" + '"' + "," +
                                         '"' + "TestSuit_TotalTestCases" + '"' + "," + '"' + "TestSuit_TotalPassed" + '"' + "," + '"' + "TestSuit_TotalFail" + '"' + "," + '"' + "listOfTestCases" +
                                         '"' + "," + '"' + "ClientID" + '"' + "," + '"' + "Build_Number" + '"' + "," + '"' + "Test_AppName" + '"' + "," + '"' + "jenkinsBuildURL" + '"' + ") " +
                                         "VALUES('" + ts.GetId() + "','" + ts.GetSuiteName() + "','" + ts.GetSuiteStartTime() + "','"
                                         + ts.GetSuiteEndTime() +
                                         "','" + ts.environmentName + "','" + ts.GetSUT_Host() +
                                         "','" + ts.GetTotalTestCasesExecuted() + "','" + ts.GetPassed() + "','"
                                         + ts.GetFailed() + "','" + ts.GetListOfTestCases().Count + "','"
                                        + ts.ClientID + "','" + ts.buildVersion + "','" + ts.ApplicationName + "','" + ts.GetJenkinsBuildURL() + "')";

                    NpgsqlCommand command = new NpgsqlCommand(commandText, connection);
                    int result = command.ExecuteNonQuery();
                    
                }
            }
            catch (Exception ex)
            {
            }

        }
        //private void WriteToDatabase(PostgreSQLCopyHelper<TestSuite> copyHelper, IEnumerable<TestSuite> entities)
        //{
        //    using (var connection = new NpgsqlConnection("Server=10.4.10.31;Port=5432;Database=QA_Automation;User Id=bta_dev;Password=Aut0*13Q;"))
        //    {
        //        connection.Open();

        //        copyHelper.SaveAll(connection, entities);

        //    }
        //}
        public void CreateTestStep(TestStep tstep)
        {

            try
            {

                using (var connection = new NpgsqlConnection("Server=10.4.10.31;Port=5432;Database=qa_automation;User Id=bta_dev;Password=Aut0*13Q;"))
                {
                    connection.Open();


                    string commandText = "INSERT INTO automation." + '"' + "ReportTestStep" + '"' +
                                         "(teststep_id" + "," + '"' + "testcase_id" + '"' + "," + '"' + "TestStep_Name" +
                                         '"' + "," + '"' + "TestStep_StartTime" + '"' + "," + '"' + "TestStep_EndTime" + '"' + "," +
                                         '"' + "TestStep_Input" + '"' + "," + '"' + "TestStep_Output" + '"' + "," + '"' + "TestStep_expectedResults" +
                                         '"' + "," + '"' + "TestStep_Status" + '"' + "," + '"' + "TestStep_errorMessage" + '"' + "," + '"' + "TestStep_imageContent" + '"' + "," + '"' + "images" + '"' + ") " +
                                         "VALUES('" + tstep.GetId() + "','" + tstep.GetTestCaseid() + "','"
                                         + tstep.GetTestStep() +
                                         "','" + tstep.GetStepStartTime() + "','" + tstep.GetStepEndTime() +
                                         "','" + tstep.GetInput() + "','" + tstep.GetOutput() + "','"
                                         + tstep.GetExpectedResult() + "','" + tstep.GetTestStepdbstatus() + "','"
                                        + tstep.GetErrorMessage() + "','" + tstep.GetImageContent() + "','" + tstep.GetImages() + "')";



                    NpgsqlCommand command = new NpgsqlCommand(commandText, connection);
                    int result = command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
            }

        }

        public void CreateTestCase(TestCase tcase)
        {
            try
            {
                using (var connection = new NpgsqlConnection("Server=10.4.10.31;Port=5432;Database=qa_automation;User Id=bta_dev;Password=Aut0*13Q;"))
                {
                    connection.Open();

                    string commandText = "INSERT INTO automation." + '"' + "ReportTestCase" + '"' +
                                         "(" + '"' + "TestCaseid" + '"' + "," + '"' + "testsuit_id" + '"' + "," + '"' + "TestCaseStartTime" + '"' + "," + '"' + "TestcaseEndTime" +
                                         '"' + "," + '"' + "TestcaseName" + '"' + "," + '"' + "TestCaseStatus" + '"' + ","
                                         + '"' + "TestCaseSteps" + '"' + "," + '"' + "TestCaseErrorMessage" + '"' + "," + '"' + "imageContent" + '"' + "," + '"' + "image" + '"' + ") " +
                                         "VALUES('" + tcase.GetId() + "','" + tcase.GetTestSuiteId() + "','"
                                         + tcase.GetStartTime() +
                                         "','" + tcase.GetEndTime() + "','" + tcase.GetTestCaseName() + "','" + tcase.Getdbstatus() +
                                         "','" + tcase.GetTestCaseSteps().Count() + "','" + tcase.GetErrorMessage() + "','"
                                         + tcase.GetImageContent() + "','" + tcase.GetImages() + "')";


                    NpgsqlCommand command = new NpgsqlCommand(commandText, connection);
                    int result = command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
            }

        }

        public TestSuite GetTestSuit(TestSuite ts)
        {
            TestSuite Testsuite = null;

            using (var connection = new NpgsqlConnection("Server=10.4.10.31;Port=5432;Database=qa_automation;User Id=bta_dev;Password=Aut0*13Q;"))
            {
                connection.Open();
                try
                {

                    NpgsqlTransaction tran = connection.BeginTransaction();
                    var str = ts.GetId();
                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM automation.GetTestsSuites_CasesSteps_new('" + str + "');", connection);
                    NpgsqlDataAdapter dataadapter = new NpgsqlDataAdapter(command);
                    DataSet dataset = new DataSet();
                    DataTable dt = new DataTable();
                    List<TestCase> TestCaseList = null;
                    List<TestStep> TestStepList = null;

                    dataadapter.Fill(dataset);

                    if (dataset != null && dataset.Tables.Count > 0)
                    {
                        Testsuite = new TestSuite();
                        var _result = from row in dataset.Tables[0].AsEnumerable()
                                      group row by new
                                      {
                                          ID = row.Field<string>("TestCaseid"),
                                      } into TestSuiteCaseStep
                                      select new
                                      {
                                          ID = TestSuiteCaseStep.Key.ID,
                                          TestSuiteCaseStep = TestSuiteCaseStep.AsEnumerable()
                                      };
                        TestCaseList = new List<TestCase>();
                        foreach (var datarow in _result)
                        {
                            var data = datarow.TestSuiteCaseStep.FirstOrDefault();

                            TestCase TestCase = new TestCase();
                            Testsuite.SetId(data["TestSuit_id"].ToString());
                            Testsuite.SetSuiteName(data["TestSuit_Name"].ToString());
                            Testsuite.SetSuiteStartTime(data["TestSuit_StartTime"].ToString());
                            Testsuite.SetSuiteEndTime(data["TestSuit_EndTime"].ToString());
                            Testsuite.SetPassed(Convert.ToInt32(data["TestSuit_TotalPassed"]));
                            Testsuite.SetFailed(Convert.ToInt32(data["TestSuit_TotalFail"]));
                            Testsuite.environmentName = data["TestSuit_Environment"].ToString();
                            Testsuite.buildVersion = data["Build_Number"].ToString();
                            Testsuite.ClientID = data["ClientID"].ToString();
                            Testsuite.ApplicationName = data["Test_AppName"].ToString();

                            TestCase.SetId(data["TestCaseid"].ToString());
                            TestCase.SetTestCaseName(data["TestcaseName"].ToString());
                            TestCase.SetStartTime(data["TestCaseStartTime"].ToString());
                            TestCase.SetEndTime(data["TestcaseEndTime"].ToString());
                            TestCase.SetErrorMessage(data["TestCaseErrorMessage"].ToString());
                            TestCase.SetImageContent(data["imageContent"].ToString());
                            TestCase.SetStatus(Convert.ToBoolean(data["TestCaseStatus"]));

                            TestStepList = new List<TestStep>();

                            foreach (var item in datarow.TestSuiteCaseStep)
                            {
                                TestStep TestStep = new TestStep();
                                TestStep.SetTestStep(item[("TestStep_Name")].ToString());
                                TestStep.SetStepStartTime(item[("TestStep_StartTime")].ToString());
                                TestStep.SetStepEndTime(item[("TestStep_EndTime")].ToString());
                                TestStep.SetOutput(item[("TestStep_Output")].ToString());
                                TestStep.SetErrorMessage(item[("TestStep_errorMessage")].ToString());
                                TestStep.SetImageContent(item[("TestStep_imageContent")].ToString());
                                TestStep.SetStatus(Convert.ToBoolean(item["TestStep_Status"]));
                                TestStepList.Add(TestStep);
                            }
                            TestCase.SetTestCaseSteps(TestStepList);
                            TestCaseList.Add(TestCase);
                            TestStepList = null;                            
                        }
                        Testsuite.SetListOfTestCases(TestCaseList);
                        
                    }
                    tran.Commit();

                }


                catch (Exception ex)
                {

                    Console.WriteLine("Message: " + ex.Message);
                   // Log_DataType_Conversion_Error(new StringBuilder( ex.Message));
                }
                finally
                {
                    connection.Close();
                }
                return Testsuite;
            }
            
        }


        //public static void Log_DataType_Conversion_Error(StringBuilder Error)
        //{
        //    string path = @"D:\BnpProject\BnPAutomation\Bnp.Core\Errorlog";
        //    try
        //    {
        //        Error.AppendLine();
        //        Error.AppendLine("ErrorDate:" + DateTime.Now.ToString());
        //        Error.AppendLine("===========================");
        //        Error.AppendLine();
        //        try
        //        {

        //            //System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("~/data/ErrorLog/AppCodeLog_" + DateTime.Now.ToShortDateString().Replace("/", "").Replace(".", "").Replace("-", "") + ".txt"), Error.ToString());

        //            System.IO.File.AppendAllText((path + DateTime.Now.ToShortDateString().Replace("/", "").Replace(".", "").Replace("-", "") + ".txt"), Error.ToString());

        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        //public TestSuite GetTestSuit_Old(TestSuite ts)
        //{
        //    TestSuite Ts = null;
        //    try
        //    {
        //        using (var connection = new NpgsqlConnection("Server=10.4.10.31;Port=5432;Database=qa_automation;User Id=bta_dev;Password=Aut0*13Q;"))
        //        {
        //            connection.Open();

        //            // char[] str = ts.GetId().ToCharArray();
        //            string str = ts.GetId();
        //            NpgsqlCommand command = new NpgsqlCommand("automation.GetTestsSuites_CasesSteps_new", connection);
        //            command.Parameters.AddWithValue("@Suiteid", str);
        //            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
        //            DataSet ds = new DataSet();
        //            DataTable dt = new DataTable();
        //            List<TestCase> TestCaseList = null;
        //            List<TestStep> TestStepList = null;
        //            command.CommandType = CommandType.StoredProcedure;
        //            da.Fill(ds);
        //            NpgsqlDataReader dRead = command.ExecuteReader();
        //            Console.WriteLine("Contents of table in database: \n");
        //            if (ds != null && ds.Tables.Count > 0)
        //            {
        //                Ts = new TestSuite();

        //                foreach (DataRow dr in ds.Tables[0].Rows)
        //                {
        //                    Ts.SetId(dr["TestSuit_id"].ToString());
        //                    Ts.SetSuiteName(dr["TestSuit_Name"].ToString());
        //                    Ts.SetSuiteStartTime(dr["TestSuit_StartTime"].ToString());
        //                    Ts.SetSuiteEndTime(dr["TestSuit_EndTime"].ToString());
        //                    Ts.SetPassed(Convert.ToInt32(dRead["TestSuit_TotalPassed"]));
        //                    Ts.SetFailed(Convert.ToInt32(dRead["TestSuit_TotalFail"]));
        //                    //var TestCase = new List<TestCase>();
        //                    if (ds.Tables[1].Rows.Count > 0)
        //                    {
        //                        TestCaseList = new List<TestCase>();
        //                        var TestCaseids = ds.Tables[1].Select("testsuit_id = '" + Ts.GetId() + "'");
        //                        foreach (DataRow drr in TestCaseids)
        //                        {
        //                            TestCase TC = new TestCase();
        //                            TC.SetId(drr["TestCaseid"].ToString());
        //                            TC.SetTestCaseName(drr["TestcaseName"].ToString());
        //                            TC.SetStartTime(drr["TestCaseStartTime"].ToString());
        //                            TC.SetEndTime(drr["TestcaseEndTime"].ToString());
        //                            TC.SetErrorMessage(dr["TestCaseErrorMessage"].ToString());
        //                            TC.SetImageContent(dr["imageContent"].ToString());
        //                            if (ds.Tables[2].Rows.Count > 0)
        //                            {
        //                                TestStepList = new List<TestStep>();
        //                                var TestStepids = ds.Tables[2].Select("testcase_id = '" + TC.GetId() + "'");
        //                                foreach (DataRow drrow in TestStepids)
        //                                {
        //                                    TestStep TStep = new TestStep();
        //                                    TStep.SetTestStep(drrow[("TestStep_Name")].ToString());
        //                                    TStep.SetStepStartTime(drrow[("TestStep_StartTime")].ToString());
        //                                    TStep.SetStepEndTime(drrow[("TestStep_EndTime")].ToString());
        //                                    TStep.SetOutput(drrow[("TestStep_Output")].ToString());
        //                                    TStep.SetErrorMessage(drrow[("TestStep_errorMessage")].ToString());
        //                                    TStep.SetImageContent(drrow[("imageContent")].ToString());
        //                                    //TStep.SetImages(drrow[("images")]);
        //                                    TestStepList.Add(TStep);
        //                                }
        //                            }
        //                            TC.SetTestCaseSteps(TestStepList);
        //                            TestStepList = null;
        //                            TestCaseList.Add(TC);
        //                        }
        //                    }
        //                    Ts.SetListOfTestCases(TestCaseList);
        //                    TestCaseList = null;
        //                }
        //            }

        //            connection.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return Ts;
        //}
    }

    
}









