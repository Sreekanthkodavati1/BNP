namespace BnPBaseFramework.Web.Extensions
{
    using System;
    using BnPBaseFramework.Web.WebElements;
    using OpenQA.Selenium;

    /// <summary>
    /// Extension methods for IWebElement
    /// </summary>
    public static class WebElementExtensions
    {
        /// <summary>
        /// Verify if actual element text equals to expected.
        /// </summary>
        /// <param name="webElement">The web element.</param>
        /// <param name="text">The text.</param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public static bool IsElementTextEqualsToExpected(this IWebElement webElement, string text)
        {
            return webElement.Text.Equals(text);
        }

        /// <summary>
        /// Set element attribute using java script.
        /// </summary>
        /// <example>Sample code to check page title: <code>
        /// this.Driver.SetAttribute(this.username, "attr", "10");
        /// </code></example>
        /// <param name="webElement">The web element.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <exception cref="System.ArgumentException">Element must wrap a web driver
        /// or
        /// Element must wrap a web driver that supports java script execution</exception>
        public static void SetAttribute(this IWebElement webElement, string attribute, string attributeValue)
        {
            var javascript = webElement.ToDriver() as IJavaScriptExecutor;
            if (javascript == null)
            {
                throw new ArgumentException("Element must wrap a web driver that supports javascript execution");
            }

            javascript.ExecuteScript(
                "arguments[0].setAttribute(arguments[1], arguments[2])",
                webElement,
                attribute,
                attributeValue);
        }

        /// <summary>
        /// Click on element using java script.
        /// </summary>
        /// <param name="webElement">The web element.</param>
        public static void JavaScriptClick(this IWebElement webElement)
        {
            webElement.ScrollToElement();
            var javascript = webElement.ToDriver() as IJavaScriptExecutor;
            if (javascript == null)
            {
                throw new ArgumentException("Element must wrap a web driver that supports javascript execution");
            }

            javascript.ExecuteScript("arguments[0].click();", webElement);
        }

  
        public static void JavaScriptSendText(this IWebElement webElement,string input)
        {
            webElement.ScrollToElement();
            webElement.Clear();
            var javascript = webElement.ToDriver() as IJavaScriptExecutor;
            if (javascript == null)
            {
                throw new ArgumentException("Element must wrap a web driver that supports javascript execution");
            }

            javascript.ExecuteScript("arguments[0].setAttribute('value', '" + input + "')", webElement);
        }

        public static void SendText(this IWebElement webElement, string input)
        {
            try
            {
                webElement.ScrollToElement();
                webElement.Clear();
                webElement.SendKeys(input);
            }
            catch (Exception e) { throw new Exception("Failed to Send input: " + input + "to InputElement:" + webElement + "Due to:" + e); }
        }

        public static void ClickElement(this IWebElement webElement)
        {
            try
            {
                webElement.ScrollToElement();
                webElement.Click();
            }
            catch (Exception e) { throw new Exception("Clicking on Element:" + webElement + " failed due to:" + e); }
        }

        public static void ScrollToElement(this IWebElement webElement)
        {
            try
            {
                var javascript = webElement.ToDriver() as IJavaScriptExecutor;
                if (javascript == null)
                {
                    throw new ArgumentException("Element must wrap a web driver that supports javascript execution");
                }

                javascript.ExecuteScript("arguments[0].scrollIntoView(true);", webElement);
            }catch(Exception e) { throw new Exception("Failed to Scroll due to:" + e); }
        }

        public static void DragandDrop(this IWebElement webElement)
        {
            var javascript = webElement.ToDriver() as IJavaScriptExecutor;
            if (javascript == null)
            {
                throw new ArgumentException("Element must wrap a web driver that supports javascript execution");
            }

            javascript.ExecuteScript("arguments[0].ClickElement();", webElement);
        }

        public static bool IsElementPresent(this IWebElement webElement)
        {
            try
            {
                if (webElement.Displayed)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary> Returns the textual content of the specified node, and all its descendants regardless element is visible or not.</summary>
        /// <returns>textContent</returns>
        /// <param name="webElement">The web element.</param>
        public static string GetTextContent(this IWebElement webElement)
        {
            var javascript = webElement.ToDriver() as IJavaScriptExecutor;
            if (javascript == null)
            {
                throw new ArgumentException("Element must wrap a web driver that supports javascript execution");
            }

            var textContent = (string)javascript.ExecuteScript("return arguments[0].textContent", webElement);
            return textContent;
        }
    }
}
