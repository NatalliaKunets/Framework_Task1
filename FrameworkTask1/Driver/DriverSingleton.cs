using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace FrameworkTask1.Driver;

public class DriverSingleton
{
    private static IWebDriver? driver;

    private DriverSingleton() { }

    public static IWebDriver GetDriver()
    {
        if (driver == null)
        {
            switch (Environment.GetEnvironmentVariable("browser")?.ToLower())
            {
                case "firefox":
                    {
                        new DriverManager().SetUpDriver(new FirefoxConfig());
                        driver = new FirefoxDriver();
                        break;
                    }
                default:
                    {
                        new DriverManager().SetUpDriver(new ChromeConfig());
                        driver = new ChromeDriver();
                        break;
                    }
            }

            driver.Manage().Window.Maximize();
        }

        return driver;
    }

    public static void CloseDriver()
    {
        //driver?.Quit();
    }
}
