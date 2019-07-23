namespace BnPBaseFramework.Web.Helpers
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Contains useful actions connected with dates
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Gets the tomorrow date.
        /// </summary>
        /// <value>
        /// The tomorrow date.
        /// </value>
        public static string TomorrowDate
        {
            get
            {
                return DateTime.Now.AddDays(1).ToString("ddMMyyyy", CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets the current time stamp.
        /// </summary>
        /// <value>
        /// The current time stamp.
        /// </value>
        public static string CurrentTimeStamp
        {
            get
            {
                return DateTime.Now.ToString("ddMMyyyyHHmmss", CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets the current date.
        /// </summary>
        /// <value>
        /// The current date.
        /// </value>
        public static string CurrentDate
        {
            get
            {
                return DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
            }
        }

        public static int RandomNumberGenerator(int start, int end)
        {
            Random random = new Random();
            int randomNumber = random.Next(start, end);
            return randomNumber;
        }

        /// <summary>
        /// Gets the future date.
        /// </summary>
        /// <param name="numberDaysToAddToNow">The number days to add from current date.</param>
        /// <returns>Date in future depends on parameter: numberDaysToAddToNow</returns>
        public static string GetFutureDate(int numberDaysToAddToNow)
        {
            return DateTime.Now.AddDays(numberDaysToAddToNow).ToString("ddMMyyyy", CultureInfo.CurrentCulture);
        }

        public static string GetDate(string typeOfDate)
        {
            var date = DateTime.Now;

            switch (typeOfDate)
            {
                case "Current":
                    return DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.CurrentCulture);

                case "Future":
                    int randomYear = RandomNumberGenerator(2, 10);
                    int randomDays = RandomNumberGenerator(1, 30);
                    int randomHours = RandomNumberGenerator(1, 24);
                    return DateTime.Now.AddYears(randomYear).AddDays(randomDays).AddHours(randomHours).ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.CurrentCulture);
                default:
                    throw new Exception("Invalid Inputs");
            }
        }

        public static string GeneratePastTimeStampBasedonMin(int minutes)
        {
            var date = DateTime.Now;
            return DateTime.Now.AddMinutes(-minutes).ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.CurrentCulture);
        }

        public static string GeneratePastTimeStampBasedonDay(int days)
        {
            var date = DateTime.Now;
            return DateTime.Now.AddDays(-days).ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.CurrentCulture);
        }
    }
}