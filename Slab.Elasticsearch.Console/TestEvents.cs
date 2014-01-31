using System.Diagnostics.Tracing;

namespace Slab.Elasticsearch.Console
{
    [EventSource(Name = "TestEvents")]
    public class TestEvents : EventSource
    {
        public static readonly TestEvents Log = new TestEvents();

        [Event(1, Message = "TestEvents Critical: {0}", Level = EventLevel.Critical)]
        public void Critical(string message)
        {
            if (IsEnabled()) WriteEvent(1, message);
        }

        [Event(2, Message = "TestEvents Error {0}", Level = EventLevel.Error)]
        public void Error(string message)
        {
            if (IsEnabled()) WriteEvent(2, message);
        }

        [Event(3, Message = "TestEvents Informational {0}", Level = EventLevel.Informational)]
        public void Informational(string message)
        {
            if (IsEnabled()) WriteEvent(3, message);
        }

        [Event(4, Message = "TestEvents LogAlways {0}", Level = EventLevel.LogAlways)]
        public void LogAlways(string message)
        {
            if (IsEnabled()) WriteEvent(4, message);
        }

        [Event(5, Message = "TestEvents Verbose {0}", Level = EventLevel.Verbose)]
        public void Verbose(string message)
        {
            if (IsEnabled()) WriteEvent(5, message);
        }

        [Event(6, Message = "TestEvents Warning {0}", Level = EventLevel.Warning)]
        public void Warning(string message)
        {
            if (IsEnabled()) WriteEvent(6, message);
        }
    }
}