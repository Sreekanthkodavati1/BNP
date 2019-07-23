namespace BnPBaseFramework.Web
{
    using System;
    using OpenQA.Selenium.Remote;

      public class CapabilitiesSetEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CapabilitiesSetEventArgs"/> class.
        /// </summary>
        /// <param name="capabilities">The existing capabilities</param>
        public CapabilitiesSetEventArgs(DesiredCapabilities capabilities)
        {
            this.Capabilities = capabilities;
        }

        /// <summary>
        /// Gets the current capabilities
        /// </summary>
        public DesiredCapabilities Capabilities { get; }
    }
}