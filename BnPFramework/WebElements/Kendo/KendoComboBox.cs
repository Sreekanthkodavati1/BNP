namespace BnPBaseFramework.Web.WebElements.Kendo
{
    using System.Globalization;

    using BnPBaseFramework.Web.Extensions;

    using OpenQA.Selenium;

    /// <summary>
    ///     Kendo Combo Box element
    /// </summary>
    public class KendoComboBox : KendoSelect
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="KendoComboBox" /> class.
        /// </summary>
        /// <param name="webElement">The webElement</param>
        public KendoComboBox(IWebElement webElement)
            : base(webElement)
        {
        }

        /// <summary>
        ///     Gets web element of the visible input element, where the user types.
        /// </summary>
        public IWebElement Input
        {
            get
            {
                var element =
                    (IWebElement)this.Driver.JavaScripts()
                        .ExecuteScript(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "return $('{0}').data('{1}').input.toArray()[0];",
                                this.ElementCssSelector,
                                this.SelectType));
                return element;
            }
        }

        /// <summary>Gets the selector.</summary>
        /// <value>The selector.</value>
        protected override string SelectType
        {
            get
            {
                return "kendoComboBox";
            }
        }

        /// <summary>
        ///     Types text into KendoComboBox input
        /// </summary>
        /// <param name="text">Text to type</param>
        public new void SendKeys(string text)
        {
            this.Input.Clear();
            this.Input.SendKeys(text);
        }
    }
}