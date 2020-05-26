using System;
using System.ComponentModel.Composition;
using System.Threading;

namespace PasswordBoss
{
    [Export(typeof(IPBAppUI))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class AppRunner : IPBAppUI
    {
        private static IResolver moduleLoader = null;
        private static PBApp app;
        private static bool isRunning = false;
        private static readonly object syncLock = new object();

        private static readonly ILogger logger = Logger.GetLogger(typeof(AppRunner));
		private readonly string[] args;

        [ImportingConstructor]
        public AppRunner([Import(typeof(IResolver))] IResolver resolver)
        {
            moduleLoader = resolver;

            AppDomain.CurrentDomain.UnhandledException += onUnhandledException;
        }

        public void Init(string[] args)
		{ 			
            Execute(args);
        }

        private static event Action InitComplete;

        public void OnInitComplete(Action a)
        {
            lock (syncLock)
            {
                InitComplete = a;
            }
        }

        private static void Start(string[] args)
        {
            logger.Debug("Execute");

            Thread t = null;

            lock (syncLock)
            {
                if (isRunning)
                {
                    return;
                }

                t = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        lock (syncLock)
                        {
                            isRunning = true;

                            if (app == null)
                            {
                                app = new PBApp(moduleLoader, args);
                            }

                            if (InitComplete != null)
                            {
                                InitComplete();
                            }
                        }

                        app.Run();
                    }
                    catch (Exception exc)
                    {
                        logger.Error(exc.ToString());
                    }
                }));

                t.IsBackground = true;
                t.SetApartmentState(ApartmentState.STA);
            }

            t.Start();
        }

        public void Execute(string[] args)
        {
            Start(args);
        }

        public void Quit()
        {
            lock (syncLock)
            {
                if (!isRunning) return;
            }

            app.Quit();
        }

        public void Show()
        {
            lock (syncLock)
            {
                if (!isRunning) return;
            }

            app.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    app.ShowUI();
                }
                catch (Exception exc)
                {
                    logger.Error(exc.ToString());
                }
            }), null);
        }

        public bool IsRunning { get { lock (syncLock) { return isRunning; } } }

        private void onUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;

                if (exception != null)
                {
                    logger.Error("AppRunner.onUnhandledException Message: " + exception.Message);
                    logger.Error("AppRunner.onUnhandledException StackTrace: " + exception.StackTrace);
                }
            }
            catch (Exception exc)
            {
                logger.Error(exc.ToString());
            }
        }
    }
}