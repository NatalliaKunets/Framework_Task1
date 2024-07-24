namespace FrameworkTask1.Model;

public record InstancesServiceConfiguration
{
    public string NumberOfInstances { get; set; }
    public string Software { get; set; }
    public string MachineFamily { get; set; }
    public string Series { get; set; }
    public string MashineType { get; set; }
    public string GpuType { get; set; }
    public string GpusNumber { get; set; }
    public string LocalSSD { get; set; }
    public string Region { get; set; }
}
