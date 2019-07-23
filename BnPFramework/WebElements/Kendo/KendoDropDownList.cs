
namespace BnPBaseFramework.Web.WebElements.Kendo
{
    using OpenQA.Selenium;

    /// <summary>
    /// Kendo Drop Down List element
    /// </summary>
    public class KendoDropDownList : KendoSelect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KendoDropDownList"/> class.
        /// </summary>
        /// <param name="webElement">The webElement</param>
        public KendoDropDownList(IWebElement webElement)
            : base(webElement)
        {
        }

        /// <summary>Gets the selector.</summary>
        /// <value>The selector.</value>
        protected override string SelectType
        {
            get
            {
                return "kendoDropDownList";
            }
        }
    }
}
