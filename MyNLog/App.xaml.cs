using MyNLog.Services;
using MyNLog.Views;
using NLog;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MyNLog
{
    public partial class App : PrismApplication
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetLogger(typeof(App).ToString());

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in types.Where(t => t.IsClass && !t.IsAbstract && t.CustomAttributes.Any(a => a.AttributeType == typeof(PrismSingletonAttribute))))
                containerRegistry.RegisterSingleton(type, type);

            foreach (var type in types.Where(t => t.IsClass && !t.IsAbstract && t.CustomAttributes.Any(a => a.AttributeType == typeof(PrismResourceInjectionAttribute))))
            {
                var attribute = Attribute.GetCustomAttribute(type, typeof(PrismResourceInjectionAttribute)) as PrismResourceInjectionAttribute;
                Resources.Add(attribute.ResourceKey ?? type.Name, Container.Resolve(type));
            }

            foreach (var type in types.Where(t => t.IsClass && !t.IsAbstract && t.CustomAttributes.Any(a => a.AttributeType == typeof(PrismGenericResourceInjectionAttribute))))
            {
                var attribute = Attribute.GetCustomAttribute(type, typeof(PrismGenericResourceInjectionAttribute)) as PrismGenericResourceInjectionAttribute;
                var resultType = attribute.GenericType.IsGenericTypeDefinition ? attribute.GenericType.MakeGenericType(type) : attribute.GenericType;
                var key = attribute.ResourceKey;
                if (key == null)
                    if (attribute.GenericType.CustomAttributes.Any(a => a.AttributeType == typeof(PrismResourceKeyFormatAttribute)))
                    {
                        var gAttribute = Attribute.GetCustomAttribute(attribute.GenericType, typeof(PrismResourceKeyFormatAttribute)) as PrismResourceKeyFormatAttribute;
                        key = string.Format(gAttribute.KeyFormat, type.Name);
                    }
                    else
                        key = $"{attribute.GenericType.Name}{type.Name}";
                Resources.Add(key, Container.Resolve(resultType));
            }
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
