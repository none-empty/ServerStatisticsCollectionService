using System.Diagnostics;

namespace ServerStatisticsCollectionService.MemoryMonitors;

public class LinuxMemoryMonitor : IMemoryMonitor
{
    public async Task<(double memoryUsage, double availableMemory)> GetMemorUsageAndAvailableMemory()
    {
        string memInfo = await RunCommand("bash", "-c \"free -m\"");

        return ParseMemory(memInfo);
    }
    
    private async Task<string> RunCommand(string file, string args)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = file,
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = psi, EnableRaisingEvents = true };
        process.Start();
        
        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        if (!string.IsNullOrEmpty(error)) ; //TODO

        return output;
    }
    
    private (int used, int available) ParseMemory(string output)
    {
       
        var memLine = output.Split('\n')
            .FirstOrDefault(l => l.TrimStart().StartsWith("Mem:"));
        if (memLine == null)
            return ( 0, 0);

       
        var parts = memLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        
        if (parts.Length < 7)
            return (0, 0);

        
        int used = int.Parse(parts[2]);
        int available = int.Parse(parts[6]);

        return ( used, available);
    }
}