using K2Spy.ExtensionMethods;
using SourceCode.SmartObjects.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public static class Trier
    {
        #region Public Methods

        public static void TrySilently(Action action, Action onError = null)
        {
            try
            {
                action();
            }
            catch
            {
                onError?.Invoke();
            }
        }

        public static void Try(this System.Windows.Forms.Control that, Action action)
        {
            Trier.TryInternal(action, null, null, that);
        }

        public static async Task TryAsync(this System.Windows.Forms.Control that, Task action)
        {
            await Trier.TryAsyncInternal(() => action, null, null, that);
        }

        public static async Task TryAsync(this System.Windows.Forms.Control that, Func<Task> action)
        {
            await Trier.TryAsyncInternal(action, null, null, that);
        }

        public static async Task TryAsync(this System.Windows.Forms.Control that, Func<Task> action, Action onError)
        {
            await Trier.TryAsyncInternal(action, () => Task.Run(onError), null, that);
        }

        public static async Task<bool> Try(Task action, Action onError = null, string errorTitle = null, System.Windows.Forms.IWin32Window owner = null)
        {
            return await Trier.TryAsyncInternal(() => action, () => Task.Run(onError), errorTitle, owner);
        }

        public static async Task<bool> Try(Task action, Task onError = null, string errorTitle = null, System.Windows.Forms.IWin32Window owner = null)
        {
            return await Trier.TryAsyncInternal(() => action, () => onError, errorTitle, owner);
        }

        public static async Task<bool> Try(Task action, Func<Task> onError, string errorTitle = null, System.Windows.Forms.IWin32Window owner = null)
        {
            return await Trier.TryAsyncInternal(() => action, onError, errorTitle, owner);
        }

        public static async Task<bool> TryAsync(Func<Task> action, Func<Task> onError = null, string errorTitle = null, System.Windows.Forms.IWin32Window owner = null)
        {
            return await Trier.TryAsyncInternal(action, onError, errorTitle, owner);
        }

        public static async Task<bool> TryAsync(Func<Task> action, Action onError, string errorTitle = null, System.Windows.Forms.IWin32Window owner = null)
        {
            return await Trier.TryAsyncInternal(action, () => Task.Run(onError), errorTitle, owner);
        }


        public static bool Try(Action action, Action error = null, string errorTitle = null, System.Windows.Forms.IWin32Window owner = null)
        {
            return Trier.TryInternal(action, error, errorTitle, owner);
        }

        #endregion

        private static bool TryInternal(Action action, Action error = null, string errorTitle = null, System.Windows.Forms.IWin32Window owner = null)
        {
            try
            {
                try
                {
                    action();
                    return true;
                }
                catch (SmartObjectException ex) when (ex.BrokerData?.Count > 0)
                {
                    Serilog.Log.Warning($"Caught SmartObjectException: {ex}");
                    SmartObjectExceptionData[] data = ex.BrokerData.OfType<SmartObjectExceptionData>().ToArray();
                    throw new Exception(string.Join(@"
", data.Select(key => $"{key.Severity}, {key.ServiceName}: {key.Message}")), ex);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Warning($"Error in TryInternal: {ex}");
                try
                {
                    System.Windows.Forms.MessageBox.Show(owner, ex.BuildDetailedMessage(), errorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                }
                catch (ObjectDisposedException) when (owner != null)
                {
                    System.Windows.Forms.MessageBox.Show(ex.BuildDetailedMessage(), errorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                }
                error?.Invoke();
                return false;
            }
        }

        private static async Task<bool> TryAsyncInternal(Func<Task> action, Func<Task> onError, string errorTitle, System.Windows.Forms.IWin32Window owner)
        {
            try
            {
                try
                {
                    await action();
                    return true;
                }
                catch (SmartObjectException ex) when (ex.BrokerData?.Count > 0)
                {
                    Serilog.Log.Warning($"Caught SmartObjectException: {ex}");
                    SmartObjectExceptionData[] data = ex.BrokerData.OfType<SmartObjectExceptionData>().ToArray();
                    throw new Exception(string.Join(@"
", data.Select(key => $"{key.Severity}, {key.ServiceName}: {key.Message}")), ex);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Warning($"Error in TryAsyncInternal: {ex}");
                try
                {
                    System.Windows.Forms.MessageBox.Show(owner, ex.BuildDetailedMessage(), errorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                }
                catch (ObjectDisposedException) when (owner != null)
                {
                    System.Windows.Forms.MessageBox.Show(ex.BuildDetailedMessage(), errorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                }
                if (onError != null)
                {
                    await Trier.TryAsyncInternal(onError, () => null, errorTitle, owner);
                }
                return false;
            }
        }
    }
}