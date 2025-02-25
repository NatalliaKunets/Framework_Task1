using FrameworkTask1.Driver;
using FrameworkTask1.Model;
using FrameworkTask1.PageObgects;
using FrameworkTask1.Utils;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace FrameworkTask1.Tests;

public sealed class GoogleCloudCalculatorTests : IDisposable
{
    private readonly IWebDriver webDriver;
    private readonly InstancesServiceConfiguration generalPurposeConfiguration;
    private readonly ScreenshotHelper screenshotHelper;
    private bool testFailed = true;
    private readonly IConfigurationRoot configuration;

    public GoogleCloudCalculatorTests()
    {
        // dotnet test -e TEST_ENVIRONMENT=chrome
        var env = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT");
        configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{env}.json", optional: true)
            .Build();

        webDriver = DriverSingleton.GetDriver(configuration["browser"]!);
        webDriver.Url = configuration["baseURL"];

        generalPurposeConfiguration = TestDataHelper.Get(configuration["testData"] ?? "GeneralPurposeFree.json");

        screenshotHelper = new ScreenshotHelper(webDriver);
    }

    public void Dispose()
    {
        if (testFailed)
        {
            string filePath = configuration["screenshotsPath"];
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            Directory.CreateDirectory(filePath);
            screenshotHelper.TakeScreenshot(Path.Combine(filePath, $"failure_{timestamp}.png"));
        }

        DriverSingleton.CloseDriver();
    }

    private void ConfigureProducts()
    {
        var pricingCalculatorPage = new PricingCalculatorPage(webDriver);

        pricingCalculatorPage.ClickAdd();
        pricingCalculatorPage.WaitForAddToEstimateFrame();
        pricingCalculatorPage.ClickComputeEngine();

        var computeEnginePage = new ComputeEnginePage(webDriver);
        computeEnginePage.WaitForPageLoaded();

        computeEnginePage.EnterNumberOfInstances(generalPurposeConfiguration.NumberOfInstances);
        computeEnginePage.SetSoftware(generalPurposeConfiguration.Software);

        computeEnginePage.SetProvisioningModel();
        computeEnginePage.SetMachineFamily(generalPurposeConfiguration.MachineFamily);
        computeEnginePage.SetSeries(generalPurposeConfiguration.Series);
        computeEnginePage.SetMachineType(generalPurposeConfiguration.MashineType);

        computeEnginePage.AddGPU();
        computeEnginePage.WaitUntilCostUpdated();

        computeEnginePage.SetGpuModel(generalPurposeConfiguration.GpuType);
        computeEnginePage.SetGpuNumber(generalPurposeConfiguration.GpusNumber);

        computeEnginePage.SetLocalSSD(generalPurposeConfiguration.LocalSSD);

        computeEnginePage.SetRegion(generalPurposeConfiguration.Region);
        computeEnginePage.WaitUntilCostUpdated();


        computeEnginePage.ShareClick();
        computeEnginePage.WaitForShareFrame();
        computeEnginePage.OpenEstimatedCost();
    }

    [Fact]
    public void CostEstimateTableTest()
    {
        ConfigureProducts();

        webDriver.SwitchTo().Window(webDriver.WindowHandles[1]);
        var summaryPage = new CostEstimateSummaryPage(webDriver);
        summaryPage.WaitForPageLoaded();

        Assert.Equal(generalPurposeConfiguration.NumberOfInstances, summaryPage.GetItemValue("Number of Instances"));
        Assert.Equal(generalPurposeConfiguration.Software, summaryPage.GetItemValue("Operating System / Software"));
        Assert.Contains(generalPurposeConfiguration.MashineType, summaryPage.GetItemValue("Machine type"));
        Assert.Equal(generalPurposeConfiguration.GpuType, summaryPage.GetItemValue("GPU Model"));
        Assert.Equal(generalPurposeConfiguration.GpusNumber, summaryPage.GetItemValue("Number of GPUs"));
        Assert.Equal(generalPurposeConfiguration.LocalSSD, summaryPage.GetItemValue("Local SSD"));
        Assert.Equal(generalPurposeConfiguration.Region, summaryPage.GetItemValue("Region"));

        testFailed = false;
    }
}