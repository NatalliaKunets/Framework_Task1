﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FrameworkTask1.Extensions;

public static class IWebDriverExtensions
{
    public static void SetDropDownValue(this IWebDriver driver, IWebElement dropdown, string value)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        dropdown.Click();
        wait.Until(_ => { return dropdown.GetAttribute("aria-expanded") == "true"; });

        IWebElement option = driver.FindElement(By.XPath($"//span[text() = '{value}']/ancestor::li[@role = 'option']"));

        driver.JsClick(option);
        wait.Until(_ => { return dropdown.GetAttribute("aria-expanded") == "false"; });
    }

    public static void JsClick(this IWebDriver driver, IWebElement element)
    {
        // The workaround for
        // Element is not clickable at point because another element obscures it
        // Element could not be scrolled into view

        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click()", element);
    }

}
