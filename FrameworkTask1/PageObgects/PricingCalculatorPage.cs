using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FrameworkTask1.PageObgects;

public class PricingCalculatorPage
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public PricingCalculatorPage(IWebDriver webDriver)
    {
        driver = webDriver;
        wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
    }

    private IWebElement AddToEstimateButton => driver.FindElement(By.XPath("//span[text()='Add to estimate']/ancestor::button"));
    private IWebElement ComputeEngineButton => driver.FindElement(By.XPath("//*[text()='Compute Engine']/parent::div"));


    public void ClickAdd() => AddToEstimateButton.Click();
    
    public void ClickComputeEngine() => ComputeEngineButton.Click();

    public void WaitForAddToEstimateFrame() => wait.Until(_ => ComputeEngineButton.Displayed);
    
}
