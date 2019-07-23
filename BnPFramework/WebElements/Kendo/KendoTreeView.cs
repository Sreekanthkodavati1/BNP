namespace BnPBaseFramework.Web.WebElements.Kendo
{
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;

    using BnPBaseFramework.Web.Extensions;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;

    /// <summary>
    ///     Kendo Tree View element
    /// </summary>
    public class KendoTreeView : RemoteWebElement
    {
        private readonly string kendoTreeView;

        private readonly IWebElement webElement;

        /// <summary>
        ///     Initializes a new instance of the <see cref="KendoTreeView" /> class.
        /// </summary>
        /// <param name="webElement">The webElement</param>
        public KendoTreeView(IWebElement webElement)
            : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            this.webElement = webElement;
            var id = webElement.GetAttribute("id");
            this.kendoTreeView = string.Format(CultureInfo.InvariantCulture, "$('#{0}').data('kendoTreeView')", id);
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

        /// <summary>
        ///     Gets selected webElement or null
        /// </summary>
        public IWebElement SelectedOption
        {
            get
            {
                var element =
                    (IWebElement)this.Driver.JavaScripts()
                        .ExecuteScript(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "var treeView = {0}; return treeView.select().toArray()[0];",
                                this.kendoTreeView));
                return element;
            }
        }

        /// <summary>
        /// Expands collapsed nodes.
        /// </summary>
        public void Expand()
        {
            this.Driver.JavaScripts()
                .ExecuteScript(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "var treeView = {0}.expand('.k-item'); ",
                        this.kendoTreeView));
        }

        /// <summary>
        /// Collapses nodes.
        /// </summary>
        public void Collapse()
        {
            this.Driver.JavaScripts()
                .ExecuteScript(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "var treeView = {0}.collapse('.k-item'); ",
                        this.kendoTreeView));
        }

        /// <summary>
        ///     The select by text.
        /// </summary>
        /// <param name="text">
        ///     The text.
        /// </param>
        public void SelectByText(string text)
        {
            this.Driver.JavaScripts()
                .ExecuteScript(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "var treeView = {0}; var element = treeView.findByText('{1}'); treeView.select(element); treeView.trigger('select',{{node:element}});",
                        this.kendoTreeView,
                        text));
            ////this.Driver.WaitForSourceChanged(1);
        }

        /// <summary>Searches for the first text.</summary>
        /// <param name="text">The text.</param>
        /// <returns>The found text.</returns>
        public Collection<string> FindByText(string text)
        {
            var elements =
                (ReadOnlyCollection<IWebElement>)this.Driver.JavaScripts()
                    .ExecuteScript(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "var treeView = {0}; return treeView.findByText('{1}').toArray();",
                            this.kendoTreeView,
                            text));

            if (elements == null)
            {
                return new Collection<string>();
            }

            var textItems = elements.Select(element => element.GetTextContent()).ToList();
            return new Collection<string>(textItems);
        }
    }
}