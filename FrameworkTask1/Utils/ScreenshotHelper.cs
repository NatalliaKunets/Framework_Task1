using OpenQA.Selenium;

namespace FrameworkTask1.Utils;

public class ScreenshotHelper
{
    private readonly IWebDriver driver;

    public ScreenshotHelper(IWebDriver driver)
    {
        this.driver = driver;
    }

    public void TakeScreenshot(string filePath)
    {
        Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        screenshot.SaveAsFile(filePath);
    }
}
