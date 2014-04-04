using System.Collections.Generic;
using System.Linq;

using MetricMe.Core;
using MetricMe.Core.Extensions;
using MetricMe.Server;
using MetricMe.Server.Configuration;
using MetricMe.Server.Listeners;

namespace MetricMe.ServerHost
{
    public class Host
    {
        private Coordinator coordinator;

        public void Start()
        {
            var backEnds = GetRequiredBackends();
            var listeners = GetRequiredListeners();

            this.coordinator = new Coordinator(listeners, backEnds);

            this.coordinator.Start();
        }

        public void Stop()
        {
            this.coordinator.Stop();
        }

        private IEnumerable<IMetricListener> GetRequiredListeners()
        {
            bool requireUdp;
            bool requireTcp = requireUdp = true;

            var specifiedListeners = GlobalConfig.Listeners;

            if (specifiedListeners.Length > 0)
            {
                requireTcp = specifiedListeners.Contains("Tcp");
                requireUdp = specifiedListeners.Contains("Udp");
            }

            if (requireTcp)
                yield return new TcpMetricListener(GlobalConfig.TcpListeningPort);

            if (requireUdp)
                yield return new UdpMetricListener(GlobalConfig.UdpListeningPort);

            yield return new InternalMetricQueue();
        }

        private IEnumerable<IBackend> GetRequiredBackends()
        {
            var backendProviders = TypeFinder.FindAllTypesInheriting<IBackendProvider>();
            var providerInstances = backendProviders.Select(p => p.CreateNew()).Cast<IBackendProvider>().ToList();
            var providersByType =
                providerInstances.SelectMany(p => p.ProvidedTypes().Select(t => new { Type = t, Provider = p }))
                    .GroupBy(p => p.Type)
                    .Select(p => p.First())
                    .ToDictionary(k => k.Type, v => v.Provider);

            var specifiedBackends = GlobalConfig.Backends;

            var allTypes = providersByType.Keys;
            var requiredTypes = specifiedBackends.Length == 0
                                    ? allTypes
                                    : allTypes.Where(
                                        at => specifiedBackends.Contains(at.Name.Replace("Backend", string.Empty)));

            return requiredTypes.Select(t => providersByType[t].CreateBackend(t));
        }
    }
}