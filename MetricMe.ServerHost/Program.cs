using MetricMe.Core;

using Topshelf;

namespace MetricMe.ServerHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AssemblyLoader.LoadAllInAppDomainPath();

            HostFactory.Run(
                x =>
                    {
                        x.Service<Host>(
                            s =>
                                {
                                    s.ConstructUsing(n => new Host());
                                    s.WhenStarted(h => h.Start());
                                    s.WhenStopped(h => h.Stop());
                                });
                        x.RunAsLocalSystem();
                        x.SetDescription("MetricMe Server");
                        x.SetDisplayName("MetricMe Server");
                        x.SetServiceName("MetricMeServer");
                    });
        }
    }
}