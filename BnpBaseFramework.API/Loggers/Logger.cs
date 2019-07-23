using System;
using System.Collections;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;

namespace BnpBaseFramework.API.Loggers
{
    public static class Logger
    {
        private static ILog Log { get; set; }

        static Logger()
        {
            var patternLayout = new PatternLayout
            {
                //ConversionPattern = "%date{yyyy-MM-dd hh:mm:ss tt} [%class] [%method] [%level]  - %message%newline"
                ConversionPattern = "[ %date{ddd MMM dd yyyy, HH:mm:ss} ] [%class] [%-5level] [%method]   - %message%newline"
            };
            patternLayout.ActivateOptions();

            var consoleAppender = new ConsoleAppender()
            {
                Name = "ConsoleAppender",
                Layout = patternLayout,
                Threshold = Level.All
            };
            consoleAppender.ActivateOptions();

            var rollingAppender = new RollingFileAppender()
            {
                Name = "RollingFileAppender",
                Layout = patternLayout,
                Threshold = Level.All,
                AppendToFile = true,
                File = "RollingFileLogger.log",
                MaximumFileSize = "1MB",
                MaxSizeRollBackups = 15
            };
            rollingAppender.ActivateOptions();

            BasicConfigurator.Configure(consoleAppender, rollingAppender);
            Log = LogManager.GetLogger(typeof(Logger));
        }


        public static void Debug(object message)
        {
            Log.Debug(message);
        }

        public static void Info(object message)
        {
            Log.Info(message);
        }

        public static void Warning(object message)
        {
            Log.Warn(message);
        }

        public static void Error(string message)
        {
            Log.Error(message);
        }
        public static void Error(Exception ex)
        {
            Log.Error(GeDetailFromExceptipon(ex));
        }

        public static void Error(object message, Exception exception)
        {
            Log.Error(message, exception);
        }

        public static void Fatal(string message)
        {
            Log.Fatal(message);
        }

        public static void Fatal(Exception ex)
        {
            Log.Fatal(GeDetailFromExceptipon(ex));
        }
        public static void Fatal(object message, Exception exception)
        {
            Log.Fatal(message, exception);
        }


        static public string GeDetailFromExceptipon(Exception exception)
        {
            string exceptionString = "";
            try
            {
                int i = 0;
                while (exception != null)
                {
                    exceptionString += $"*** Exception Level {i} ***\n";
                    exceptionString += $"Message: {exception.Message} \n";
                    exceptionString += $"Source: {exception.Source} \n";
                    exceptionString += $"Target Site: {exception.TargetSite} \n";
                    exceptionString += $"Stack Trace: {exception.StackTrace} \n";
                    exceptionString += $"Data: ";
                    foreach (DictionaryEntry keyValuePair in exception.Data)
                        exceptionString += $"{keyValuePair.Key.ToString()} : {keyValuePair.Value.ToString()}";
                    exceptionString += "\n***\n\n";
                    exception = exception.InnerException;
                    i++;
                }
            }
            catch { }
            return exceptionString;
        }

    }
}
