using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyNLog.Views
{
    public partial class MainWindow : Window
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetLogger(typeof(MainWindow).ToString());

        public MainWindow()
        {
            InitializeComponent();

            //RegisterAllEvents(typeof(ToggleButton), FilterPopupButton);
        }

        public static void RegisterAllEvents(Type type, FrameworkElement target)
        {
            var events = EventManager.GetRoutedEvents();
            foreach (var routedEvent in events)
            {
                EventManager.RegisterClassHandler(type,
                    routedEvent, new RoutedEventHandler((sender, args) =>
                    {
                        if (sender != target)
                            return;
                        Logger.Debug(args.OriginalSource + "=>" + args.RoutedEvent);
                    }));
            }
        }
    }
}
