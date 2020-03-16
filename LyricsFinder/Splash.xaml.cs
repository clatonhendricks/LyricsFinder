using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Reflection;
using System.Deployment;

namespace LyricsFinder
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public Splash()
        {
            InitializeComponent();

            
            
            this.Loaded += new RoutedEventHandler(Splash_Loaded);
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
                
            }
        }

        //public string version
        //{
        //    get
        //    {

        //        System.Reflection.Assembly _assemblyInfo =
        //          System.Reflection.Assembly.GetExecutingAssembly();

        //        string ourVersion = string.Empty;

        //        //if running the deployed application, you can get the version
        //        //  from the ApplicationDeployment information. If you try
        //        //  to access this when you are running in Visual Studio, it will not work.
        //        if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
        //        {
        //            ourVersion =

        //              ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
        //        }
        //        else
        //        {
        //            if (_assemblyInfo != null)
        //            {
        //                ourVersion = _assemblyInfo.GetName().Version.ToString();
        //            }
        //        }
        //        return ourVersion;
        //    }
        //}
        
        

        void Splash_Loaded(object sender, RoutedEventArgs e)
        {
		    //Version 
            

            //txtVer.Text = String.Format("Version {0} ", AssemblyVersion);
            
            

            IAsyncResult result = null;

            // This is an anonymous delegate that will be called when the initialization has COMPLETED
            AsyncCallback initCompleted = delegate(IAsyncResult ar)
            {
                App.Current.ApplicationInitialize.EndInvoke(result);

                // Ensure we call close on the UI Thread.
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate { Close(); });
            };

            // This starts the initialization process on the Application
            result = App.Current.ApplicationInitialize.BeginInvoke(this, initCompleted, null);
        }

        public void SetProgress(double progress)
        {
            // Ensure we update on the UI Thread.
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate { progBar.Value = progress; });
        }
    }

}