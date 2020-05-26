using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PasswordBoss.Helpers
{
    /// <summary>
    /// relay command for defining class
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }

    /// An async version for delegate command
    /// </summary>
    public class AsyncRelayCommand<T> : ICommand where T : System.Windows.Window, new()
    {
        BackgroundWorker _worker = new BackgroundWorker();
        Func<object, bool> _canExecute;
        Action<object> _action;
        Action<object> _beforeExecute;
        Window _ownerWindow;
        T _loadingWindow = null;
        ILogger logger = Logger.GetLogger(typeof(AsyncRelayCommand<T>));
        Stopwatch _timer;

		public void HideLoadingWindow()
		{
			try
			{
				_loadingWindow.Hide();
			}
			catch(Exception ex)
			{
				logger.Error(ex.ToString());
			}
		}

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="action">The action to be executed</param>
        /// <param name="canExecute">Will be used to determine if the action can be executed</param>
        /// <param name="completed">Will be invoked when the action is completed</param>
        /// <param name="error">Will be invoked if the action throws an error</param>
        public AsyncRelayCommand(Action<object> action,
                                    Func<object, bool> canExecute = null,
                                    Action<object> beforeExecute = null,
                                    Action<object> completed = null,
                                    Action<Exception> error = null,
                                    Func<object, object> executionFunction = null,
                                    Window ownerWindow = null,
                                    bool forceDelay = false
                                    )
        {
            _beforeExecute = beforeExecute;
            _loadingWindow = new T(); //create window that we want to show while loading
			_ownerWindow = ownerWindow;
            _action = action;
            if (_timer == null)
            {
                _timer = new Stopwatch();
            }

            _worker.DoWork += (s, e) =>
                {
                    CommandManager.InvalidateRequerySuggested();
                    if (_timer == null)
                    {
                        _timer = new Stopwatch();
                    }
                    _timer.Reset();
                    _timer.Start();
                    if (action != null)
                    {
                        _action(e.Argument);
                    }
                    else if (executionFunction != null)
                    {
                        e.Result = executionFunction(e.Argument);
                    }

                };

            _worker.RunWorkerCompleted += (s, e) =>
                {
                    //Mouse.OverrideCursor = Cursors.Arrow;
                    _timer.Stop();
                    int _milisecondsToSleep = 1350;
                    // Force full circle of loading window. If operation is fast enough (< 100 ms) skip it.
                    if (forceDelay && _timer.Elapsed.TotalMilliseconds > 100 && _timer.Elapsed.TotalMilliseconds < _milisecondsToSleep)
                    {
                        var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(_milisecondsToSleep - _timer.Elapsed.TotalMilliseconds) };

                        timer.Tick += (sender, args) =>
                        {
                            timer.Stop();
                            if (_loadingWindow != null)
                            {
                                _loadingWindow.Hide();
                            }
                        };
                        timer.Start();
                    }
                    else
                    {
                        if (_loadingWindow != null && _loadingWindow.IsVisible)
                        {
                            _loadingWindow.Hide();
                        }
                    }


                    if (completed != null && e.Error == null)
                    {
                        completed(e.Result);
                    }


                    if (e.Error != null)
                    {
#if DEBUG
                        MessageBox.Show("DBG MSG ONLY: " + e.Error.ToString());
#endif
                        logger.Error("{0}", e.Error);
                        if (error != null)
                        {
                            error(e.Error);
                        }

                    }


                    CommandManager.InvalidateRequerySuggested();
                    if (_loadingWindow.Owner != null)
                        _loadingWindow.Owner.Focus();

                };

            _canExecute = canExecute;
        }

        /// <summary>
        /// To cancel an ongoing execution
        /// </summary>
        public void Cancel()
        {
            if (_worker.IsBusy)
                _worker.CancelAsync();
        }

        /// <summary>
        /// Note that this will return false if the worker is already busy
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return (_canExecute == null) ?
                    !(_worker.IsBusy) : !(_worker.IsBusy)
                        && _canExecute(parameter);
        }

        /// <summary>
        /// Let us use command manager for thread safety
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Here we'll invoke the background worker
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            //Mouse.OverrideCursor = Cursors.Wait;
            if (_beforeExecute != null)
            {
                _beforeExecute(parameter);
            }
            if (_timer == null)
            {
                _timer = new Stopwatch();
            }
            _timer.Reset();
            _timer.Start();
            _worker.RunWorkerAsync(parameter);
            if (_loadingWindow != null)
            {
                _loadingWindow = new T();
                try
                {
                    // Setting owner and position of loading window
                    if (_ownerWindow == null)
                        _loadingWindow.Owner = System.Windows.Application.Current.MainWindow;
                    else
                        _loadingWindow.Owner = _ownerWindow;
                    _loadingWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }

                _loadingWindow.ShowDialog();
            }
        }
    }
}