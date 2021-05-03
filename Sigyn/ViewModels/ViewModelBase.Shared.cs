using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Sigyn.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sigyn.ViewModels
{
    public abstract class ViewModelBase:BaseViewModel
    {
        #region Constructors

        public ViewModelBase()
        {
            GoBackCommand = new AsyncCommand(() => this.FocusTask(() => GoBack()));
        }

        #endregion Constructors

        #region Properties

        public ICommand GoBackCommand
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Perfomce an Task and stops other functions from starting while it's running
        ///     If IsBusy=false performce an Task and sets IsBusy=true while the Task is running.
        ///     After the Task completes sets IsBusy=false again.
        ///     <para>If IsBusy=true returns.</para>
        /// </summary>
        /// <example>
        ///     Shows how to use this function in combination with a Command
        ///     <code>
        /// ButtonSelectionCommand = new AsyncCommand(()=>  DoUITask(() => MoveAsync()));
        /// </code>
        /// </example>
        /// <param name="action">Action</param>
        public void DoUITask(Action action = null)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            action?.Invoke();
            IsBusy = false;
        }

        /// <summary>
        ///     Overridable behavior for entering a View
        /// </summary>
        public void Entering()
        {
            Subscribe();
        }

        /// <summary>
        ///     Perform an Task and stops other functions from starting while it's running
        ///     DOES NOT PROVIDE A MAINTHREAD
        ///     <para>
        ///         If IsBusy=false perform an async Task and sets IsBusy=true while the Task is running.
        ///         After the Task completes sets IsBusy=false again.
        ///     </para>
        ///     <para>If IsBusy=true returns.</para>
        /// </summary>
        /// <example>
        ///     Shows how to use this function in combination with an async Command
        ///     <code>
        /// ButtonSelectionCommand = new AsyncCommand(()=>  DoUITask(new Func &lt; Task &gt; (() => MoveAsync())));
        /// </code>
        /// </example>
        /// <param name="action">Async Func</param>
        /// <returns> Returns an async Task</returns>
        public async Task FocusTask(Func<Task> action = null)
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                await action?.Invoke();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Goes back on the NavigationStack
        /// Runs on the MainThread
        /// </summary>
        public  async Task GoBack()
        {
            await PrivateThreading.BeginInvokeOnMainThreadAsync(() => Application.Current.MainPage.Navigation.PopModalAsync(false));
        }

        /// <summary>
        ///     Overridable behavior for entering a View
        /// </summary>
        public  void Leaving() => UnSubscribe();

        /// <summary>
        ///     Overridable behavior for subscribing to events
        /// </summary>
        public  void Subscribe()
        {
        }

        /// <summary>
        ///     Overridable behavior for un-subscribing to events
        /// </summary>
        public  void UnSubscribe()
        {
        }

        #endregion Methods
    }
}
