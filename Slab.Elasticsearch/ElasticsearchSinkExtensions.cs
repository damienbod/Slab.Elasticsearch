using System;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;

namespace Slab.Elasticsearch
{
    public static class ElasticsearchSinkExtensions
    {
        public static SinkSubscription<ElasticsearchSink> LogToElasticsearchSink(
            this IObservable<EventEntry> eventStream, string log, IEventTextFormatter formatter = null)
        {
            var sink = new ElasticsearchSink(log, formatter);

            var subscription = eventStream.Subscribe(sink);

            return new SinkSubscription<ElasticsearchSink>(subscription, sink);
        }
    }
}