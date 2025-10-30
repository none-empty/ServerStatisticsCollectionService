namespace ServerStatisticsCollectionService;

public record ServerStatistics(double MemoryUsage, double AvailableMemory, double CpuUsage, 
    DateTime Timestamp);