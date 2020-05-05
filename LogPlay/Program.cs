using System;
using System.Serialization;
using Serilog;
using Serilog.Formatting.Json;

namespace LogPlay
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(new JsonObjectAsMessageFormatter())
                .Enrich.WithMachineName()
                .CreateLogger();

            Log.Information(new { Name = "Fred", Type = "Bob"}.Serialize());
            Log.Information("I logged this: {0} and that: {1}", "Bob", "Jim");
            Console.ReadLine();
        }
    }
}
