using System;
using System.Collections;
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
    ///     <MyNamespace:MassCefBrowserControl/>
    ///
    /// </summary>
    public class MassCefBrowserControl : Control
    {
        private StackPanel _root;

        static MassCefBrowserControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MassCefBrowserControl), new FrameworkPropertyMetadata(typeof(MassCefBrowserControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _root = GetTemplateChild("_root") as StackPanel;
            if(_root == null) throw new Exception("There should be a StackPanel with name: _root in the Template!");

            RefreshRoot();
        }

        #region BrowserUrls

        public static readonly DependencyProperty BrowserUrlsProperty = DependencyProperty.Register(
            "BrowserUrls", typeof (IEnumerable<string>), typeof (MassCefBrowserControl), new PropertyMetadata(default(IEnumerable<string>), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = dependencyObject as MassCefBrowserControl;
            control?.RefreshRoot();
        }

        public IEnumerable<string> BrowserUrls
        {
            get { return (IEnumerable<string>) GetValue(BrowserUrlsProperty); }
            set { SetValue(BrowserUrlsProperty, value); }
        }

        #endregion

        private void RefreshRoot()
        {
            if (BrowserUrls == null || !BrowserUrls.Any() || _root == null) return;

            foreach (var browserUrl in BrowserUrls)
            {
                var browser = new CefBrowser("SDXHost", new SampleNetObject());
                browser.LoadUrl(browserUrl);

                _root.Children.Add(browser);
            }
        }

        public sealed class SampleNetObject
        {
            public CefBrowser Owner { get; set; }

            public void postMessage(string akarmi)
            {
                Owner?.OnWebObjectCall();
            }

            public void addResponseReceiver(string valami)
            {
                Owner?.OnWebObjectCall();
            }
        }
    }
}
