using Core.Interfaces;

namespace Logging
{
    public class Logger : ILogger
    {
        public static void Log(string message) => Logger.Log(message);
    }
}
