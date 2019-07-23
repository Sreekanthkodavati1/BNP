namespace BnPBaseFramework.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Enum extension helper class
    /// </summary>
    public class EnumUtils
    {
        /// <summary>
        /// Method to get enum description which contains spaces
        /// </summary>
        /// <param name="value">enum value</param>
        /// <returns>
        /// enum value for which description to be returned
        /// </returns>
        public static string GetDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description;
        }
    }
}
