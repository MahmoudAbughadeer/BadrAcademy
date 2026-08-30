using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Helpers
{
    static internal class clsAsyncMethodExecutor
    {
        /// <summary>
        /// Executes an async method with a wait control and handles exceptions.
        /// </summary>
        /// <param name="waitControl">The control to show/hide while running.</param>
        /// <param name="action">The async method to run.</param>
        public static async Task RunWithWaitAsync(Control waitControl, Func<Task> action)
        {
            try
            {
                waitControl.Visible = true;
                await action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                waitControl.Visible = false;
            }
        }


        /// <summary>
        /// Executes an async method that returns a value with a wait control and handles exceptions.
        /// </summary>
        /// <typeparam name="T">The return type of the async method.</typeparam>
        /// <param name="waitControl">The control to show/hide while running.</param>
        /// <param name="action">The async method to run.</param>
        /// <param name="defaultValue">Value to return if an exception occurs (optional).</param>
        /// <returns>The result of the async method or defaultValue if failed.</returns>
        public static async Task<T> RunWithWaitAsync<T>(Control waitControl, Func<Task<T>> action, T defaultValue = default)
        {
            try
            {
                waitControl.Visible = true;
                return await action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return defaultValue;
            }
            finally
            {
                waitControl.Visible = false;
            }
        }

        /// <summary>
        /// Executes an async method without a wait control and handles exceptions.
        /// </summary>
        /// <param name="action">The async method to run.</param>
        public static async Task RunAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static async Task<T> RunAsync<T>(Func<Task<T>> action, T defaultValue = default)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return default;
            }
        }

    }
}
