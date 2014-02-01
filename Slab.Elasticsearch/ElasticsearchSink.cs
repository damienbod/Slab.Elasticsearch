using System;
using System.Dynamic;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Nest;

namespace Slab.Elasticsearch
{
    public sealed class ElasticsearchSink : IObserver<EventEntry>
    {
        private readonly IEventTextFormatter _formatter;
        private ElasticClient _client;
        private readonly string _searchIndex = "slab";
        private readonly string _searchType = "SLABEvent";
        private readonly string _connectionString = "Server=localhost;Index=log;Port=9200";

        public ElasticsearchSink(string connectionString, string searchIndex, string searchType, IEventTextFormatter formatter)
        {
            if (!string.IsNullOrEmpty(connectionString)) _connectionString = connectionString;
            if (!string.IsNullOrEmpty(searchIndex)) _searchIndex = searchIndex.ToLower();
            if (!string.IsNullOrEmpty(searchType)) _searchType = searchType.ToLower();
            _formatter = formatter ?? new EventTextFormatter();
            
        }

        public void OnNext(EventEntry entry)
        {
            if (entry != null)
            {
                using (var writer = new StringWriter())
                {
                    _formatter.WriteEvent(entry, writer);
                    Post(entry, writer.ToString());
                }
            }
        }

        private void Post(EventEntry loggingEvent, string body)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                return;
            }
            var settings = ConnectionBuilder.BuildElsticSearchConnection(_connectionString);
            _client = new ElasticClient(settings);
            var logEvent = CreateLogEvent(loggingEvent, body);
            try
            {
                _client.IndexAsync(logEvent, _searchIndex, _searchType);
            }
            catch (InvalidOperationException)
            {
            }
        }

        private static dynamic CreateLogEvent(EventEntry loggingEvent, string formattedBody)
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
            logEvent.Payload = loggingEvent.Payload;
            logEvent.ProviderId = loggingEvent.Schema.ProviderId;
            logEvent.ProviderName = loggingEvent.Schema.ProviderName;
            logEvent.Task = loggingEvent.Schema.Task;
            logEvent.TaskName = loggingEvent.Schema.TaskName;
            logEvent.Version = loggingEvent.Schema.Version;
            logEvent.EventId = loggingEvent.EventId;
            logEvent.FormattedMessage = formattedBody;
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