using System;
using System.IO;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace MetricMe.Server.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static IObservable<string> ToObservable(this BinaryReader source)
        {
            return Observable.Create<string>(
                obs =>
                    {
                        var subscribed = true;
                        var errored = false;
                        var result = string.Empty;

                        while (subscribed && !errored)
                        {
                            try
                            {
                                result = source.ReadString();
                            }
                            catch (Exception ex)
                            {
                                continue;
                                //errored = true;
                                //obs.OnError(ex);
                            }
                            obs.OnNext(result);
                        }
                        return Disposable.Create(() => subscribed = false);
                    });
        }
    }
}