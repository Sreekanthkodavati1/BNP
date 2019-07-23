namespace BnPBaseFramework.Web.Types
{
    public class SavedTimes
    {
        /// <summary>
        /// The scenario
        /// </summary>
        private readonly string scenario;

        /// <summary>
        /// The browser name
        /// </summary>
        private readonly string browserName;

        /// <summary>
        /// The duration
        /// </summary>
        private long duration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavedTimes" /> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public SavedTimes(string title)
        {
            this.scenario = title;
            this.browserName = BaseConfiguration.TestBrowser.ToString();
        }

        /// <summary>
        /// Gets the scenario.
        /// </summary>
        /// <value>
        /// The scenario.
        /// </value>
        public string Scenario
        {
            get { return this.scenario; }
        }

        /// <summary>
        /// Gets the name of the Driver.
        /// </summary>
        /// <value>
        /// The name of the Driver.
        /// </value>
        public string BrowserName
        {
            get { return this.browserName; }
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public long Duration
        {
            get { return this.duration; }
        }

        /// <summary>
        /// Sets the duration.
        /// </summary>
        /// <param name="loadTime">The load time.</param>
        public void SetDuration(long loadTime)
        {
            this.duration = loadTime;
        }
    }
}
