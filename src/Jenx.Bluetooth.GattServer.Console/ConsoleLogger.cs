using Jenx.Bluetooth.GattServer.Common;
using System.Threading.Tasks;

namespace Jenx.Bluetooth.GattServer.Console
{
    public class ConsoleLogger : ILogger
    {
        Task ILogger.LogMessageAsync(string message)
        {
            System.Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}