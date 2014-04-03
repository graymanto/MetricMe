using System;
using System.Collections.Generic;

namespace MetricMe.Server
{
    public interface IBackendProvider
    {
        IEnumerable<Type> ProvidedTypes();

        IBackend CreateBackend(Type backendType);
    }
}