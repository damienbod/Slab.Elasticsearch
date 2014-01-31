using System;
using System.Collections.Specialized;
using Nest;

namespace Slab.Elasticsearch
{
    public class ConnectionBuilder
    {
        public static ConnectionSettings BuildElsticSearchConnection(string connectionString)
        {
            try
            {
                var builder = new System.Data.Common.DbConnectionStringBuilder();
                builder.ConnectionString = connectionString.Replace("{", "\"").Replace("}", "\"");

                StringDictionary lookup = new StringDictionary();
                foreach (string key in builder.Keys)
                {
                    lookup[key] = Convert.ToString(builder[key]);
                }

                return
                    new ConnectionSettings(new Uri(string.Format("http://{0}:{1}", lookup["Server"],
                        Convert.ToInt32(lookup["Port"])))).SetDefaultIndex(lookup["Index"]);
            }
            catch
            {
                throw new InvalidOperationException("Not a valid connection string");
            }
        }
    }
}
