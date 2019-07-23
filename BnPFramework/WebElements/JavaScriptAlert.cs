namespace BnPBaseFramework.Web.WebElements
{
    using OpenQA.Selenium;

    /// <summary>
    /// Implementation for JavaScript Alert interface.
    /// </summary>
    public class JavaScriptAlert
    {
        /// <summary>
        /// The web driver
        /// </summary>
        private readonly IWebDriver webDriver;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptAlert"/> class.
        /// </summary>
        /// <param name="webDriver">The web driver.</param>
        public JavaScriptAlert(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        /// <summary>
        /// Gets java script popup text
        /// </summary>
        public string JavaScriptText
        {
            get { return this.webDriver.SwitchTo().Alert().Text; }
        }

        /// <summary>
        /// Confirms the java script alert popup.
        /// </summary>
        public void ConfirmJavaScriptAlert()
        {
            this.webDriver.SwitchTo().Alert().Accept();
            this.webDriver.SwitchTo().DefaultContent();
        }

        /// <summary>
        /// Dismisses the java script alert popup.
        /// </summary>
        public void DismissJavaScriptAlert()
        {
            this.webDriver.SwitchTo().Alert().Dismiss();
            this.webDriver.SwitchTo().DefaultContent();
        }

        /// <summary>
        /// Method sends text to Java Script Alert
        /// </summary>
        /// <param name="text">Text to be sent</param>
        public void SendTextToJavaScript(string text)
        {
            this.webDriver.SwitchTo().Alert().SendKeys(text);
        }
    }
}
