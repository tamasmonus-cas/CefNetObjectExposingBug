using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.Wpf;

namespace CefNetObjectExposingBug
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CefNetObjectExposingBug"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CefNetObjectExposingBug;assembly=CefNetObjectExposingBug"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CefBrowser/>
    ///
    /// </summary>
    public class CefBrowser : Control
    {
        private Grid _contentRoot;
        private Border _root;
        private Border indicator;
        private Button _devConsoleButton;

        private ChromiumWebBrowser _browser;
        private MassCefBrowserControl.SampleNetObject _webObject;

        static CefBrowser()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CefBrowser), new FrameworkPropertyMetadata(typeof(CefBrowser)));
        }

        public CefBrowser(string name, MassCefBrowserControl.SampleNetObject webObject)
        {
            ExposingSuccess = false;

            _browser = new ChromiumWebBrowser
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0),
                BorderBrush = new SolidColorBrush(Colors.Transparent),
                BrowserSettings = new BrowserSettings
                {
                    OffScreenTransparentBackground = false,
                    JavascriptOpenWindows = CefState.Disabled
                }
            };

            if (!string.IsNullOrWhiteSpace(name) && webObject != null)
            {
                _webObject = webObject;
                _webObject.Owner = this;
                _browser.RegisterJsObject(name, webObject);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _contentRoot = GetTemplateChild("_contentRoot") as Grid;
            if (_contentRoot == null) throw new Exception("The Template has to have a Grid with name: _contentRoot !");

            _root = GetTemplateChild("_root") as Border;
            if(_root == null) throw new Exception("The Template has to have a Border with name: _root !");

            indicator = GetTemplateChild("indicator") as Border;

            indicator.Background = ExposingSuccess ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

            _root.Child = _browser;

            _devConsoleButton = new Button
            {
                Content = "Developer console",
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            _devConsoleButton.Click += DevConsoleButtonOnClick;

            _contentRoot.Children.Add(_devConsoleButton);
        }

        private void DevConsoleButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _browser.ShowDevTools();
        }

        public void LoadUrl(string url)
        {
            _browser.Address = url;
        }

        private bool _exposingSuccess;

        public bool ExposingSuccess
        {
            get
            {
                return _exposingSuccess;
                
            }
            set
            {
                _exposingSuccess = value;
                if (indicator != null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        indicator.Background = _exposingSuccess
                            ? new SolidColorBrush(Colors.Green)
                            : new SolidColorBrush(Colors.Red);
                    });
                }
            }
        }

        public void OnWebObjectCall()
        {
            ExposingSuccess = true;
        }
    }
}
