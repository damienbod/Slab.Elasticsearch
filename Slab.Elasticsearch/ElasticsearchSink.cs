using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Nest;

namespace Slab.Elasticsearch
{
    public sealed class ElasticsearchSink : IObserver<EventEntry>
    {
        private const string DefaultSubject = "Elasticsearch Sink Extension";
        private IEventTextFormatter formatter;
        private ElasticClient client;
        public string ConnectionString { get; set; }

        public ElasticsearchSink(string log, IEventTextFormatter formatter)
        {
            this.ConnectionString = "Server=localhost;Index=log;Port=9200";
            this.formatter = formatter ?? new EventTextFormatter();
            
        }

        public void OnNext(EventEntry entry)
        {
            if (entry != null)
            {
                using (var writer = new StringWriter())
                {
                    this.formatter.WriteEvent(entry, writer);
                    Post(entry, writer.ToString());
                }
            }
        }

        private async void Post(EventEntry loggingEvent, string body)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                var exception = new InvalidOperationException("Connection string not present.");
               // ErrorHandler.Error("Connection string not included in appender.", exception, ErrorCode.GenericFailure);

                return;
            }
            var settings = ConnectionBuilder.BuildElsticSearchConnection(ConnectionString);
            client = new ElasticClient(settings);
            var logEvent = CreateLogEvent(loggingEvent);
            try
            {
                client.IndexAsync(logEvent, settings.DefaultIndex, "SLABEvent");
                client.IndexAsync(logEvent, "slab", "SLABEvent");
            }
            catch (InvalidOperationException ex)
            {
                //ErrorHandler.Error("Invalid connection to ElasticSearch", ex, ErrorCode.GenericFailure);
            }
        }

        private static dynamic CreateLogEvent(EventEntry loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            dynamic logEvent = new ExpandoObject();
            logEvent.Id = new UniqueIdGenerator().GenerateUniqueId();
            logEvent.EventName = loggingEvent.Schema.EventName;
            logEvent.Keywords = loggingEvent.Schema.Keywords.ToString();
            logEvent.Level = loggingEvent.Schema.Level;
            logEvent.Opcode = loggingEvent.Schema.Opcode;
            logEvent.OpcodeName = loggingEvent.Schema.OpcodeName;
            logEvent.Payload = loggingEvent.Schema.Payload;
            logEvent.ProviderId = loggingEvent.Schema.ProviderId;
            logEvent.ProviderName = loggingEvent.Schema.ProviderName;
            logEvent.Task = loggingEvent.Schema.Task;
            logEvent.TaskName = loggingEvent.Schema.TaskName;
            logEvent.Version = loggingEvent.Schema.Version;
            logEvent.EventId = loggingEvent.EventId;
            logEvent.FormattedMessage = loggingEvent.FormattedMessage;
            logEvent.Timestamp = loggingEvent.Timestamp;

            
            return logEvent;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

     

      
    }
}