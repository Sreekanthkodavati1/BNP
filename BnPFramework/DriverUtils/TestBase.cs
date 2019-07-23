
namespace BnPBaseFramework.Web
{
    using System.Collections.Generic;
    using BnPBaseFramework.Web.Helpers;

    /// <summary>
    /// Class contains method for all tests, should be used in project test base
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// Take screenshot if test failed and delete cached page objects.
        /// </summary>
        /// <param name="driverContext">The driver context.</param>
        /// <returns>The saved attachements, null if not found.</returns>
        public string[] SaveTestDetailsIfTestFailed(DriverContext driverContext, string appType)
        {
            if (driverContext.IsTestFailed)
            {
                if (appType.Contains("Web"))
                {
                    var screenshots = driverContext.TakeAndSaveScreenshot();
                    var pageSource = this.SavePageSource(driverContext);
                    var returnList = new List<string>();
                    returnList.AddRange(screenshots);
                    if (pageSource != null)
                    {
                        returnList.Add(pageSource);
                    }

                    return returnList.ToArray();
                }
            }

            return null;
        }

        /// <summary>
        /// Save Page Source
        /// </summary>
        /// <param name="driverContext">
        /// Driver context includes
        /// </param>
        /// <returns>Path to the page source</returns>
        public string SavePageSource(DriverContext driverContext)
        {
            return driverContext.SavePageSource(driverContext.TestTitle);
        }

        /// <summary>
        /// Fail Test If Verify Failed and clear verify messages
        /// </summary>
        /// <param name="driverContext">Driver context includes</param>
        /// <returns>True if test failed</returns>
        public bool IsVerifyFailedAndClearMessages(DriverContext driverContext)
        {
            if (driverContext.VerifyMessages.Count.Equals(0))
            {
                return false;
            }

            driverContext.VerifyMessages.Clear();
            return true;
        }
    }
}