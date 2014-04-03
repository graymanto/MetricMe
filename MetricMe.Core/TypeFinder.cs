using System;
using System.Collections.Generic;
using System.Linq;

namespace MetricMe.Core
{
    public static class TypeFinder
    {
        public static IEnumerable<Type> FindAllTypesInheriting<TAssignableType>()
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsClass && typeof(TAssignableType).IsAssignableFrom(t));
        }

        public static IEnumerable<Type> FindAllTypesInheriting(Type assignableType)
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsClass && assignableType.IsAssignableFrom(t));
        }

        public static Type GetGenericTypeFromInterface(Type fullType)
        {
            var genericInterfaceName = fullType.Name + "`1";
            var jobiFace = fullType.GetInterface(genericInterfaceName);
            return jobiFace.GetGenericArguments().First();
        }
    }
}