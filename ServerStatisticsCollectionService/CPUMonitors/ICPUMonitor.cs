namespace ServerStatisticsCollectionService.CPUMonitors;

public interface ICPUMonitor
{
    public  Task<double>  GetCpuUsage();
}