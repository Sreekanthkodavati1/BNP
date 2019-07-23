
namespace BnPBaseFramework.Web.WebElements.Kendo
{
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;

    using BnPBaseFramework.Web.Extensions;
    using BnPBaseFramework.Web.Types;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;

    /// <summary>
    ///     Kendo Select element
    /// </summary>
    public abstract class KendoSelect : RemoteWebElement
    {
        private readonly ElementLocator kendoSelect = new ElementLocator(
            Locator.XPath,
            "./ancestor-or-self::span[contains(@class, 'k-widget')]//*[@id]");

        private readonly IWebElement webElement;

        protected KendoSelect(IWebElement webElement)
            : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            this.webElement = webElement;
            var id = this.webElement.GetElement(this.kendoSelect, e => e.Displayed == false).GetAttribute("id");
            this.ElementCssSelector = string.Format(CultureInfo.InvariantCulture, "#{0}", id);
        }

        /// <summary>
        ///     Gets the driver.
        /// </summary>
        public IWebDriver Driver
        {
            get
            {
                return this.webElement.ToDriver();
            }
        }

        /// <summary>Gets the unordered list.</summary>
        /// <value>The unordered list.</value>
        public IWebElement UnorderedList
        {
            get
            {
                var element =
                    (IWebElement)this.Driver.JavaScripts()
                        .ExecuteScript(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "return $('{0}').data('{1}').ul.toArray()[0];",
                                this.ElementCssSelector,
                                this.SelectType));

                return element;
            }
        }

        /// <summary>
        ///     Gets the options.
        /// </summary>
        public Collection<string> Options
        {
            get
            {
                var elements =
                    (ReadOnlyCollection<IWebElement>)this.Driver.JavaScripts()
                        .ExecuteScript(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "return $('{0}').data('{1}').ul.children().toArray();",
                                this.ElementCssSelector,
                                this.SelectType));
                var options = elements.Select(element => element.GetTextContent()).ToList();
                return new Collection<string>(options);
            }
        }

        /// <summary>Gets the selected option.</summary>
        /// <value>The selected option.</value>
        public string SelectedOption
        {
            get
            {
                var option =
                    (string)this.Driver.JavaScripts()
                        .ExecuteScript(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "return $('{0}').data('{1}').text();",
                                this.ElementCssSelector,
                                this.SelectType));
                return option;
            }
        }

        /// <summary>
        /// Gets or sets the element selector.
        /// </summary>
        protected string ElementCssSelector { get; set; }

        /// <summary>Gets the selector.</summary>
        /// <value>The selector.</value>
        protected abstract string SelectType { get; }

        /// <summary>Select by text.</summary>
        /// <param name="text">The text.</param>
        public void SelectByText(string text)
        {
            this.Driver.JavaScripts()
                .ExecuteScript(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "$('{0}').data('{1}').select(function(dataItem) {{return dataItem.text === '{2}';}});",
                        this.ElementCssSelector,
                        this.SelectType,
                        text));
        }

        /// <summary>Closes this object.</summary>
        public void Close()
        {
            this.Driver.JavaScripts()
                .ExecuteScript(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "$('{0}').data('{1}').close();",
                        this.ElementCssSelector,
                        this.SelectType));
        }

        /// <summary>Opens this object.</summary>
        public void Open()
        {
            this.Driver.JavaScripts()
                .ExecuteScript(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "$('{0}').data('{1}').open();",
                        this.ElementCssSelector,
                        this.SelectType));
        }
    }
}