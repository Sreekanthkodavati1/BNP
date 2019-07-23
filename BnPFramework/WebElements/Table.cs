namespace BnPBaseFramework.Web.WebElements
{
    using NLog;
    using BnPBaseFramework.Web.Extensions;
    using BnPBaseFramework.Web.Types;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;

    /// <summary>
    /// The table class contains actions on tables.
    /// </summary>
    public class Table : RemoteWebElement
    {
        private static readonly NLog.Logger Logger = LogManager.GetLogger("DRIVER");

        /// <summary>
        /// The web element
        /// </summary>
        private readonly IWebElement webElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="webElement">The _webElement.</param>
        public Table(IWebElement webElement)
            : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            this.webElement = webElement;
        }

        /// <summary>
        /// Returns a text representation of the grid or table html like element
        /// </summary>
        /// <param name="rowLocator">The row locator.</param>
        /// <param name="columnLocator">The column locator</param>
        /// <returns>
        /// Text representation of the grid or table html like element
        /// </returns>
        public string[][] GetTable(ElementLocator rowLocator, ElementLocator columnLocator)
        {
            var table = this.webElement;
            var rows = table.GetElements(rowLocator);

            var result = new string[rows.Count][];
            var i = 0;

            foreach (var row in rows)
            {
                var cells = row.GetElements(columnLocator);
                result[i] = new string[cells.Count];

                var j = 0;
                foreach (var cell in cells)
                {
                    var cellValue = cell.Text;
                    Logger.Debug("Table cell Row {0}, column {1}, Value: {2}", i, j, cellValue);
                    result[i][j++] = cellValue;
                }

                i++;
            }

            return result;
        }
    }
}
