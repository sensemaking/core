using System.IO;
using System.Serialization;
using Newtonsoft.Json.Linq;
using Serilog.Events;

namespace Serilog.Formatting.Json
{
    public class ObjectsAreJsonMessagesFormatter : ITextFormatter
    {
        private readonly JsonFormatter fallback = new JsonFormatter(renderMessage: true);

        public void Format(LogEvent logEvent, TextWriter output)
        {
            var message = logEvent.RenderMessage();
            if (message.IsJson())
                output.WriteLine(new { Log = message.Deserialize<dynamic>(), logEvent.Level, At = logEvent.Timestamp }.Serialize());
            else
                fallback.Format(logEvent, output);
        }
    }

    internal static class Json
    {
        internal static bool IsJson(this string input)
        {
            input = input.Trim();
            return (input.IsObject() || input.IsArray()) && input.IsWellFormed();
        }

        private static bool IsObject(this string input)
        {
            return input.StartsWith("{") && input.EndsWith("}");
        }

        private static bool IsArray(this string input)
        {
            return input.StartsWith("[") && input.EndsWith("]");
        }

        private static bool IsWellFormed(this string input)
        {
            try { JToken.Parse(input); }
            catch { return false; }

            return true;
        }
    }
}
