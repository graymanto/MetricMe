using System;
using System.IO;
using System.Linq;
using System.Reflection;

using MetricMe.Core.Extensions;

namespace MetricMe.Core
{
    public class AssemblyLoader
    {
        public static void LoadAllInAppDomainPath()
        {
            LoadAllFromPath(AppDomain.CurrentDomain.BaseDirectory);
        }

        public static void LoadAllFromPath(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            var files = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);

            files.Select(GetAssemblyName)
                .Where(assemblyName => assemblyName != null && !AssemblyExistsInDomain(assemblyName))
                .ForEach(LoadAssembly);
        }

        private static AssemblyName GetAssemblyName(string name)
        {
            try
            {
                return AssemblyName.GetAssemblyName(name);
            }
            catch (BadImageFormatException)
            {
                return null;
            }
        }

        private static void LoadAssembly(AssemblyName assembly)
        {
            try
            {
                Assembly.Load(assembly);
            }
            catch (BadImageFormatException)
            {
            }
        }

        private static bool AssemblyExistsInDomain(AssemblyName assembly)
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                    .Any(a => AssemblyName.ReferenceMatchesDefinition(assembly, a.GetName()));
        }
    }
}