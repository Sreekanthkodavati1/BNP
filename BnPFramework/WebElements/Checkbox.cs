namespace BnPBaseFramework.Web.WebElements
{
    using BnPBaseFramework.Web.Extensions;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;

    /// <summary>
    /// Contains methods for checkbox.
    /// </summary>
    public class Checkbox : RemoteWebElement
    {
        /// <summary>
        /// The web element
        /// </summary>
        private readonly IWebElement webElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="Checkbox"/> class.
        /// </summary>
        /// <param name="webElement">The webElement.</param>
        public Checkbox(IWebElement webElement)
            : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            this.webElement = webElement;
        }

        /// <summary>
        /// Set check box.
        /// </summary>
        public void TickCheckbox()
        {
            if (!this.webElement.Selected)
            {
                this.webElement.ClickElement();
            }
        }

        public bool IsSelected()
        {
            if (this.webElement.Selected)
            {
                return true;
            }
            else
            { return false; }
        }

        /// <summary>
        /// Clear the check box.
        /// </summary>
        public void UntickCheckbox()
        {
            if (this.webElement.Selected)
            {
                this.webElement.ClickElement();
            }
        }
    }
}
