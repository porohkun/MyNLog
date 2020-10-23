using MyNLog.Models;
using MyNLog.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MyNLog.ViewModels
{
    public class MainWindowViewModelDummy : MainWindowViewModel
    {
        public MainWindowViewModelDummy() : base()
        {
            LogItems.Add(new LogItemViewModel(new LogItem(0, DateTime.Now, LogLevel.Trace, "MyNLog.App", "Trace example", "in TestLogCommand.ExecuteInternal\nin InjectableCommand`1.Execute\nin CommandHelpers.CriticalExecuteCommandSource\nin MenuItem.InvokeClickAfterRender\nin ExceptionWrapper.InternalRealCall\nin ExceptionWrapper.TryCatchWhen\nin DispatcherOperation.InvokeImpl\nin DispatcherOperation.InvokeInSecurityContext\nin CulturePreservingExecutionContext.CallbackWrapper\nin ExecutionContext.RunInternal", "")));
            LogItems.Add(new LogItemViewModel(new LogItem(1, DateTime.Now, LogLevel.Debug, "MyNLog.Appp", "Trace example", "in TestLogCommand.ExecuteInternal\nin InjectableCommand`1.Execute\nin CommandHelpers.CriticalExecuteCommandSource\nin MenuItem.InvokeClickAfterRender\nin ExceptionWrapper.InternalRealCall\nin ExceptionWrapper.TryCatchWhen\nin DispatcherOperation.InvokeImpl\nin DispatcherOperation.InvokeInSecurityContext\nin CulturePreservingExecutionContext.CallbackWrapper\nin ExecutionContext.RunInternal", "")));
            LogItems.Add(new LogItemViewModel(new LogItem(2, DateTime.Now, LogLevel.Info, "MyNLog.Apppp", "Trace example", "in TestLogCommand.ExecuteInternal\nin InjectableCommand`1.Execute\nin CommandHelpers.CriticalExecuteCommandSource\nin MenuItem.InvokeClickAfterRender\nin ExceptionWrapper.InternalRealCall\nin ExceptionWrapper.TryCatchWhen\nin DispatcherOperation.InvokeImpl\nin DispatcherOperation.InvokeInSecurityContext\nin CulturePreservingExecutionContext.CallbackWrapper\nin ExecutionContext.RunInternal", "")));
            LogItems.Add(new LogItemViewModel(new LogItem(3, DateTime.Now, LogLevel.Warn, "MyNLog.Appppp", "Trace example", "in TestLogCommand.ExecuteInternal\nin InjectableCommand`1.Execute\nin CommandHelpers.CriticalExecuteCommandSource\nin MenuItem.InvokeClickAfterRender\nin ExceptionWrapper.InternalRealCall\nin ExceptionWrapper.TryCatchWhen\nin DispatcherOperation.InvokeImpl\nin DispatcherOperation.InvokeInSecurityContext\nin CulturePreservingExecutionContext.CallbackWrapper\nin ExecutionContext.RunInternal", "System.NotImplementedException: Метод или операция не реализована.\n   в MyNLog.Services.LogFileService.OpenFile(String fileName) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Services\\LogFileService.cs:строка 14\n   в MyNLog.Commands.OpenLogFileCommand.ExecuteInternal(Object parameter) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Commands\\OpenLogFileCommand.cs:строка 32")));
            LogItems.Add(new LogItemViewModel(new LogItem(4, DateTime.Now, LogLevel.Error, "MyNLog.Apppppp", "Trace example", "in TestLogCommand.ExecuteInternal\nin InjectableCommand`1.Execute\nin CommandHelpers.CriticalExecuteCommandSource\nin MenuItem.InvokeClickAfterRender\nin ExceptionWrapper.InternalRealCall\nin ExceptionWrapper.TryCatchWhen\nin DispatcherOperation.InvokeImpl\nin DispatcherOperation.InvokeInSecurityContext\nin CulturePreservingExecutionContext.CallbackWrapper\nin ExecutionContext.RunInternal", "System.NotImplementedException: Метод или операция не реализована.\n   в MyNLog.Services.LogFileService.<string, string>OpenFile(String fileName) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Services\\LogFileService.cs:строка 14\n   в MyNLog.Commands.OpenLogFileCommand.ExecuteInternal(Object parameter) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Commands\\OpenLogFileCommand.cs:строка 32")));
            LogItems.Add(new LogItemViewModel(new LogItem(5, DateTime.Now, LogLevel.Fatal, "MyNLog.App", "Trace example", "in TestLogCommand.ExecuteInternal\nin InjectableCommand`1.Execute\nin CommandHelpers.CriticalExecuteCommandSource\nin MenuItem.InvokeClickAfterRender\nin ExceptionWrapper.InternalRealCall\nin ExceptionWrapper.TryCatchWhen\nin DispatcherOperation.InvokeImpl\nin DispatcherOperation.InvokeInSecurityContext\nin CulturePreservingExecutionContext.CallbackWrapper\nin ExecutionContext.RunInternal", "System.NotImplementedException: Метод или операция не реализована.\n   в MyNLog.Services.LogFileService.OpenFile(String fileName) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Services\\LogFileService.cs:строка 14\n   в MyNLog.Commands.OpenLogFileCommand.ExecuteInternal(Object parameter) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Commands\\OpenLogFileCommand.cs:строка 32")));
            LogItems.Add(new LogItemViewModel(new LogItem(6, DateTime.Now, LogLevel.Trace, "MyNLog.App", "Trace example", "in TestLogCommand.ExecuteInternal\nin InjectableCommand`1.Execute\nin CommandHelpers.CriticalExecuteCommandSource\nin MenuItem.InvokeClickAfterRender\nin ExceptionWrapper.InternalRealCall\nin ExceptionWrapper.TryCatchWhen\nin DispatcherOperation.InvokeImpl\nin DispatcherOperation.InvokeInSecurityContext\nin CulturePreservingExecutionContext.CallbackWrapper\nin ExecutionContext.RunInternal", "")));
        }
    }

    public class MainWindowViewModel : BindableBase
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetLogger(typeof(LogFileService).ToString());

        public ObservableCollection<LogItemViewModel> LogItems { get; } = new ObservableCollection<LogItemViewModel>();
        private CollectionView _itemsView;

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                if (SetProperty(ref _filterText, value))
                {
                    _escapedFilterText = Regex.Escape(FilterText);
                    _itemsView.Refresh();
                }
            }
        }

        private string _escapedFilterText;

        public DelegateCommand ClearLogCommand { get; }
        public DelegateCommand CollapseAllCommand { get; }
        public DelegateCommand<IEnumerable> CopySelectedCommand { get; }

        private LogFileService _logFileService;
        private int _minIndex = -1;

        protected MainWindowViewModel() { }

        public MainWindowViewModel(LogFileService logFileService)
        {
            _itemsView = (CollectionView)CollectionViewSource.GetDefaultView(LogItems);
            _itemsView.Filter = ItemsFilter;

            ClearLogCommand = new DelegateCommand(ClearLog);
            CollapseAllCommand = new DelegateCommand(CollapseAll);
            CopySelectedCommand = new DelegateCommand<IEnumerable>(CopySelected);

            _logFileService = logFileService;
            _logFileService.MaxIndexChanged += LogFileService_MaxIndexChanged;
            _logFileService.SourceDisconnected += LogFileService_SourceDisconnected;
        }

        private bool ItemsFilter(object entry)
        {
            if (!(entry is LogItemViewModel item)) return false;

            if (string.IsNullOrWhiteSpace(_escapedFilterText))
                return true;
            var regex = new Regex(_escapedFilterText);

            if (regex.IsMatch(item.Level.ToString()))
                return true;
            if (regex.IsMatch(item.Logger))
                return true;
            if (regex.IsMatch(item.Message))
                return true;
            if (regex.IsMatch(item.StackTrace))
                return true;
            if (regex.IsMatch(item.Exception))
                return true;
            return false;
        }

        private void LogFileService_MaxIndexChanged()
        {
            _minIndex = Math.Max(_minIndex, _logFileService.MinIndex);

            for (int i = _minIndex; i <= _logFileService.MaxIndex; i++)
                LogItems.Add(new LogItemViewModel(_logFileService.GetRecord(i)));

            _minIndex = LogItems.Any() ? (LogItems.Last().Index + 1) : _logFileService.MinIndex;
        }

        private void LogFileService_SourceDisconnected()
        {
            _minIndex = -1;
            ClearLog();
        }

        private void ClearLog()
        {
            LogItems.Clear();
        }

        private void CollapseAll()
        {
            foreach (var item in LogItems)
                item.Expanded = false;
        }

        private void CopySelected(IEnumerable selectedItems)
        {
            var sb = new StringBuilder();
            foreach (LogItemViewModel item in selectedItems)
            {
                sb.AppendLine(item.ToString());
                sb.AppendLine();
            }
            Clipboard.SetText(sb.ToString());
        }
    }
}
