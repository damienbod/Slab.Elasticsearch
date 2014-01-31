
using System.Diagnostics.Tracing;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;

namespace Slab.Elasticsearch.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ObservableEventListener listener = new ObservableEventListener();
            listener.EnableEvents(TestEvents.Log, EventLevel.LogAlways, Keywords.All);

            listener.LogToConsole();
            listener.LogToElasticsearchSink("Test search");

            TestEvents.Log.Critical("Hello world Critical");
            TestEvents.Log.Error("Hello world Error");
            TestEvents.Log.Informational("Hello world Informational");


            System.Console.ReadLine();

        }
    }
}
