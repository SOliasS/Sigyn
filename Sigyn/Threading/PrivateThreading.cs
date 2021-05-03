using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sigyn.Threading
{
    public class PrivateThreading
    {
        #region Fields

        private static readonly object balanceLock = new object();

        #endregion Fields

        #region Methods

        /// <summary>
        ///     source: FedericoCalvagna https://forums.xamarin.com/discussion/51720/wait-for-device-begininvokeonmainthread
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Task<T> BeginInvokeOnMainThreadAsync<T>(Func<Task<T>> a)
        {
            lock (balanceLock)
            {
                var tcs = new TaskCompletionSource<T>();

                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        var result = await a();
                        tcs.SetResult(result);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                });
                return tcs.Task;
            }
        }

        public static Task<bool> BeginInvokeOnMainThreadAsync(Func<Task> a)
        {
            lock (balanceLock)
            {
                var tcs = new TaskCompletionSource<bool>();

                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        await a();
                        tcs.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                });
                return tcs.Task;
            }
        }

        #endregion Methods
    }
}
