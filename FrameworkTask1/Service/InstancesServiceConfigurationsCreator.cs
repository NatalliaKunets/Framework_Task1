using FrameworkTask1.Model;
using FrameworkTask1.Utils;

namespace FrameworkTask1.Service;

public static class InstancesServiceConfigurationsCreator
{
    public static InstancesServiceConfiguration CreateGeneralPurposeConfigurations()
    {
        return TestDataHelper.Get("Data.json");
    }
}
