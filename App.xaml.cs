using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Threading;


namespace LyricsFinder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    internal delegate void Invoker();

    public partial class App : Application
    {
        public App()
        {
            ApplicationInitialize = _applicationInitialize;
        }

        public static new App Current
        {
            get { return Application.Current as App; }
        }

        internal delegate void ApplicationInitializeDelegate(Splash splashWindow);
        internal ApplicationInitializeDelegate ApplicationInitialize;

        private void _applicationInitialize(Splash splashWindow)
        {
            // fake workload, but with progress updates.
            Thread.Sleep(700);
            splashWindow.SetProgress(0.2);

            Thread.Sleep(700);
            splashWindow.SetProgress(0.3);

            Thread.Sleep(700);
            splashWindow.SetProgress(0.5);

            Thread.Sleep(700);
            splashWindow.SetProgress(0.7);

            Thread.Sleep(700);
            splashWindow.SetProgress(0.9);

            Thread.Sleep(700);
            splashWindow.SetProgress(1);

            // Create the main window, but on the UI thread.
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
            {
                MainWindow = new Window1();
                MainWindow.Show();
            });
        }
    }

}