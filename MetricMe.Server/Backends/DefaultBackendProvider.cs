using System;
using System.Collections.Generic;

using MetricMe.Server.Configuration;
using MetricMe.Server.Graphite;

namespace MetricMe.Server.Backends
{
    public class DefaultBackendProvider : IBackendProvider
    {
        public IEnumerable<Type> ProvidedTypes()
        {
            return new[] { typeof(ConsoleBackend), typeof(GraphiteBackend) };
        }

        public IBackend CreateBackend(Type backendType)
        {
            if (backendType == typeof(GraphiteBackend))
            {
                var client = new GraphiteUdpClient(GlobalConfig.Graphite.Host, GlobalConfig.Graphite.Port);
                return new GraphiteBackend(client);
            }

            return new ConsoleBackend();
        }
    }
}