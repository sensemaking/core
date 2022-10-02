using Serilog;
using Serilog.Events;

namespace Sensemaking;

public class NullLogger : ILogger
{
    public void Write(LogEvent logEvent) { }
}