namespace BnPBaseFramework.Web.WebElements
{
    using BnPBaseFramework.Web.Extensions;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;

    /// <summary>
    /// Contains methods for Radio button.
    /// </summary>
    public class RadioButton : RemoteWebElement
    {
        /// <summary>
        /// The web element
        /// </summary>
        private readonly IWebElement webElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButton"/> class.
        /// </summary>
        /// <param name="webElement">The webElement.</param>
        public RadioButton(IWebElement webElement)
            : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            this.webElement = webElement;
        }

        /// <summary>
        /// Select Radio button
        /// </summary>
        public void SelectRadioButton()
        {
            if (!this.webElement.Selected)
            {
                this.webElement.Click();
            }
        }
    }
}
