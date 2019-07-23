
namespace BnPBaseFramework.Web.WebElements
{
    using System;
    using System.Globalization;
    using System.Linq;

    using NLog;

    using BnPBaseFramework.Web.Extensions;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// Select contains implementation for method that can be used on dropdown.
    /// </summary>
    public class Select : RemoteWebElement
    {
        private static readonly Logger Logger = LogManager.GetLogger("DRIVER");

        /// <summary>
        /// The web element
        /// </summary>
        private readonly IWebElement webElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="Select"/> class.
        /// </summary>
        /// <param name="webElement">The _webElement.</param>
        public Select(IWebElement webElement)
            : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            this.webElement = webElement;
        }

        /// <summary>
        /// Selenium SelectElement class.
        /// </summary>
        /// <example>Simple use of SelectElement: <code>
        /// this.Driver.GetElement&lt;Select&gt;(WebElement).SelectElement().SelectedOption;
        /// </code></example>
        /// <returns>Return new SelectElement handle</returns>
        public SelectElement SelectElement()
        {
            return new SelectElement(this.webElement);
        }

        /// <summary>
        /// Select value in dropdown using text.
        /// </summary>
        /// <param name="selectValue">Text to be selected.</param>
        public void SelectByText(string selectValue)
        {
            try
            {
                this.SelectByText(selectValue, BaseConfiguration.MediumTimeout);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Select Value:" + selectValue);
            }
        }

        /// <summary>
        /// Select value in dropdown using text.
        /// </summary>
        /// <param name="selectValue">Text to be selected.</param>
        /// <param name="timeout">The timeout.</param>
        public void SelectByText(string selectValue, double timeout)
        {
            var element = this.WaitUntilDropdownIsPopulated(timeout);

            var selectElement = new SelectElement(element);

            try
            {
                selectElement.SelectByText(selectValue);
            }
            catch (NoSuchElementException e)
            {
                throw new Exception("Failed to Select Value:" + selectValue);
            }
        }

        /// <summary>
        /// Select value in dropdown using index.
        /// </summary>
        /// <param name="index">Index value to be selected.</param>
        public void SelectByIndex(int index)
        {
            this.SelectByIndex(index, BaseConfiguration.MediumTimeout);
        }

        /// <summary>
        /// Select value in dropdown using index.
        /// </summary>
        /// <param name="index">Index value to be selected.</param>
        /// <param name="timeout">The timeout.</param>
        public void SelectByIndex(int index, double timeout)
        {
            timeout = timeout.Equals(0) ? BaseConfiguration.MediumTimeout : timeout;

            var element = this.WaitUntilDropdownIsPopulated(timeout);

            var selectElement = new SelectElement(element);

            try
            {
                selectElement.SelectByIndex(index);
            }
            catch (NoSuchElementException e)
            {
                Logger.Error(CultureInfo.CurrentCulture, "unable to select given index: {0}", index);
                Logger.Error(e.Message);
            }
        }

        /// <summary>
        /// Select value in dropdown using value attribute.
        /// </summary>
        /// <param name="selectValue">Value to be selected.</param>
        public void SelectByValue(string selectValue)
        {
            this.SelectByValue(selectValue, BaseConfiguration.MediumTimeout);
        }

        /// <summary>
        /// Select value in dropdown using value attribute.
        /// </summary>
        /// <param name="selectValue">Value to be selected.</param>
        /// <param name="timeout">The timeout.</param>
        public void SelectByValue(string selectValue, double timeout)
        {
            var element = this.WaitUntilDropdownIsPopulated(timeout);

            var selectElement = new SelectElement(element);

            try
            {
                selectElement.SelectByValue(selectValue);
            }
            catch (NoSuchElementException e)
            {
                Logger.Error(CultureInfo.CurrentCulture, "unable to select given value: {0}", selectValue);
                Logger.Error(e.Message);
            }
        }

        /// <summary>
        /// Determines whether text is available in dropdown.
        /// </summary>
        /// <param name="option">The text.</param>
        /// <returns>
        /// True or False depends if text is available in dropdown
        /// </returns>
        public bool IsSelectOptionAvailable(string option)
        {
            return this.IsSelectOptionAvailable(option, BaseConfiguration.MediumTimeout);
        }

        /// <summary>
        /// Determines whether text is available in dropdown.
        /// </summary>
        /// <param name="option">The text.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>
        /// True or False depends if text is available in dropdown
        /// </returns>
        public bool IsSelectOptionAvailable(string option, double timeout)
        {
            var element = this.WaitUntilDropdownIsPopulated(timeout);
            var selectElement = new SelectElement(element);

            return selectElement.Options.Any(el => el.Text.Equals(option));
        }


        public bool IsTextSelected(string option, double timeout)
        {
            try
            {
                var element = this.WaitUntilDropdownIsPopulated(timeout);

                var selectElement = new SelectElement(element);

                if (selectElement.SelectedOption.Text.Equals(option))
                {
                    return true;
                }
                {
                    return false;
                }
            }
            catch (NoSuchElementException e) { throw new Exception("Select Drop down failed" + e.Message); }
        }

        /// <summary>
        /// Waits the until dropdown is populated.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Web element when dropdown populated</returns>
        private IWebElement WaitUntilDropdownIsPopulated(double timeout)
        {
            var selectElement = new SelectElement(this.webElement);
            var isPopulated = false;
            try
            {
                new WebDriverWait(this.webElement.ToDriver(), TimeSpan.FromSeconds(timeout)).Until(
                    x =>
                    {
                        var size = selectElement.Options.Count;
                        if (size >= 1)
                        {
                            isPopulated = true;
                        }

                        return isPopulated;
                    });
            }
            catch (NoSuchElementException e)
            {
                Logger.Error(CultureInfo.CurrentCulture, "unable to select given index: {0}", selectElement);
                Logger.Error(e.Message);
            }

            return this.webElement;
        }
    }
}
