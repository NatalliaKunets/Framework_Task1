using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FrameworkTask1.PageObgects;

public class CostEstimateSummaryPage
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public CostEstimateSummaryPage(IWebDriver webDriver)
    {
        driver = webDriver;
        wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
    }

    public void Navigate(string url)
    {
        driver.Navigate().GoToUrl(url);
    }

    public string GetItemValue(string costItem)
    {
        return driver.FindElement(By.XPath($"//span[text() = '{costItem}']/following-sibling::span")).Text;
    }

    public void WaitForPageLoaded()
    {
        wait.Until(driver => driver.FindElement(By.XPath("//h4[text() = 'Cost Estimate Summary']")).Displayed);
    }
}
