using FrameworkTask1.Model;
using FrameworkTask1.Utils;

namespace FrameworkTask1.Service;

public static class InstancesServiceConfigurationCreator
{
    public static InstancesServiceConfiguration CreateGeneralPurposeConfiguration()
    {
        return TestDataHelper.Get("Data.json");
    }
}
