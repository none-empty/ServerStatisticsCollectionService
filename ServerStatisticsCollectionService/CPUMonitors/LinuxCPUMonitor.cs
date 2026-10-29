using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ServerStatisticsCollectionService.CPUMonitors;

public class LinuxCPUMonitor : ICPUMonitor
{
    public async Task<double> GetCpuUsage()
    {
        var cpuInfo = await RunCommand("bash", "-c \"top -bn1 | grep 'Cpu(s)'\"");
        var cpuUsage = ParseCpuUsage(cpuInfo);
        return cpuUsage;
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
    
    private double ParseCpuUsage(string output)
    {
       
        var match = Regex.Match(output, @"(\d+(\.\d+)?)\s*id");
        if (!match.Success) return 0;

        double idle = double.Parse(match.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
        return 100.0 - idle; 
    }
}