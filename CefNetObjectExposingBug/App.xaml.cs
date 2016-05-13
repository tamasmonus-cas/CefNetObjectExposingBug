using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;

namespace CefNetObjectExposingBug
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var settings = new CefSettings { WindowlessRenderingEnabled = true };
            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
            settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
            settings.CefCommandLineArgs.Add("disable-direct-write", "1");

            settings.RegisterScheme(new CefCustomScheme()
            {
                SchemeName = "sample",
                SchemeHandlerFactory = new SchemeHandlerFactory()
            });

            Cef.Initialize(settings);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Cef.Shutdown();
            base.OnExit(e);
        }
    }
}
