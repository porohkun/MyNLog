using MyNLog.Services;
using MyNLog.Views;
using Prism.Ioc;
using Prism.Unity;
using System.Diagnostics;
using System.Windows;

namespace MyNLog
{
    public partial class App : PrismApplication
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetLogger(typeof(App).ToString());

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Logger.Info("OnStartup");
            base.OnStartup(e);

            PresentationTraceSources.DataBindingSource.Listeners.Add(Container.Resolve<BindingErrorTraceListener>());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
        }
    }
}
