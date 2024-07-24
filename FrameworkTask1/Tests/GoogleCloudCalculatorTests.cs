using FrameworkTask1.Driver;
using FrameworkTask1.Model;
using FrameworkTask1.PageObgects;
using FrameworkTask1.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using OpenQA.Selenium;

namespace FrameworkTask1.Tests;

public sealed class GoogleCloudCalculatorTests : IDisposable
{
    private readonly IWebDriver webDriver;

    public GoogleCloudCalculatorTests()
    {
        var settings = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();

        webDriver = DriverSingleton.GetDriver();

        webDriver.Url = settings["baseURL"];
    }

    public void Dispose()
    {
        DriverSingleton.CloseDriver();
    }


    private void ConfigureProducts()
    {
        var pricingCalculatorPage = new PricingCalculatorPage(webDriver);
        //pricingCalculatorPage.Navigate();

        // predefined values
        //const string NumberOfInstances = "4";
        //const string Software = "Free: Debian, CentOS, CoreOS, Ubuntu or BYOL (Bring Your Own License)";
        //const string MachineFamily = "General Purpose";
        //const string Series = "N1";
        //const string MashineType = "n1-standard-8";
        //const string GpuType = "NVIDIA V100";
        //const string GpusNumber = "1";
        //const string LocalSSD = "2x375 GB";
        //const string Region = "Netherlands (europe-west4)";

        InstancesServiceConfiguration generalPurposeConfigurations = InstancesServiceConfigurationsCreator.CreateGeneralPurposeConfigurations();

        pricingCalculatorPage.ClickAdd();
        pricingCalculatorPage.WaitForAddToEstimateFrame();
        pricingCalculatorPage.ClickComputeEngine();

        var computeEnginePage = new ComputeEnginePage(webDriver);
        computeEnginePage.WaitForPageLoaded();

        computeEnginePage.EnterNumberOfInstances(generalPurposeConfigurations.NumberOfInstances);
        computeEnginePage.SetSoftware(generalPurposeConfigurations.Software);

        computeEnginePage.SetProvisioningModel();
        computeEnginePage.SetMachineFamily(generalPurposeConfigurations.MachineFamily);
        computeEnginePage.SetSeries(generalPurposeConfigurations.Series);
        computeEnginePage.SetMachineType(generalPurposeConfigurations.MashineType);

        computeEnginePage.AddGPU();
        computeEnginePage.WaitForGpuConfigsDisplayed();
        computeEnginePage.SetGpuModel(generalPurposeConfigurations.GpuType);
        computeEnginePage.SetGpuNumber(generalPurposeConfigurations.GpusNumber);

        computeEnginePage.SetLocalSSD(generalPurposeConfigurations.LocalSSD);

        computeEnginePage.SetRegion(generalPurposeConfigurations.Region);
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
        
        //webDriver.Navigate().Refresh();

        Assert.Equal("4", summaryPage.GetItemValue("Number of Instances"));
        Assert.Equal("Free: Debian, CentOS, CoreOS, Ubuntu or BYOL (Bring Your Own License)", summaryPage.GetItemValue("Operating System / Software"));
        Assert.Equal("Regular", summaryPage.GetItemValue("Provisioning Model"));
        Assert.Equal("n1-standard-8, vCPUs: 8, RAM: 30 GB", summaryPage.GetItemValue("Machine type"));
        Assert.Equal("NVIDIA V100", summaryPage.GetItemValue("GPU Model"));
        Assert.Equal("1", summaryPage.GetItemValue("Number of GPUs"));
        Assert.Equal("2x375 GB", summaryPage.GetItemValue("Local SSD"));
        Assert.Equal("Netherlands (europe-west4)", summaryPage.GetItemValue("Region"));
    }
}