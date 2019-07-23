namespace BnPBaseFramework.Web.Types
{
    using System;

    using OpenQA.Selenium;

    public class ErrorDetail
    {
        public ErrorDetail(Screenshot screenshot, DateTime dateTime, Exception exception)
        {
            this.Screenshot = screenshot;
            this.DateTime = dateTime;
            this.Exception = exception;
        }

        /// <summary>
        /// Gets or sets the screenshot.
        /// </summary>
        /// <value>
        /// The screenshot.
        /// </value>
        public Screenshot Screenshot { get; set; }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }
    }
}
