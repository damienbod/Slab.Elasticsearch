using System.Diagnostics.Tracing;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;


namespace Slab.Elasticsearch.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //InProcessLogging();
            OutOfProcessLogging();
            System.Console.ReadLine();
        }

        private static void InProcessLogging()
        {
            ObservableEventListener listener = new ObservableEventListener();
            listener.EnableEvents(TestEvents.Log, EventLevel.LogAlways, Keywords.All);

            listener.LogToConsole();
            listener.LogToElasticsearchSink("Server=localhost;Index=log;Port=9200", "slab", "SLABEvent");

            TestEvents.Log.Critical("Hello world In-Process Critical");
            TestEvents.Log.Error("Hello world In-Process Error");
            TestEvents.Log.Informational("Hello world In-Process Informational");
        }

        private static void OutOfProcessLogging()
        {
            TestEvents.Log.Critical("Hello world Out-Of-Process Critical");
            TestEvents.Log.Error("Hello world Out-Of-Process Error");
            TestEvents.Log.Informational("Hello world Out-Of-Process Informational");
        }
    }
}
