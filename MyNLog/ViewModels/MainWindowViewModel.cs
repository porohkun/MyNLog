using MyNLog.Models;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace MyNLog.ViewModels
{
    public class MainWindowViewModelDummy : MainWindowViewModel
    {
        public MainWindowViewModelDummy() : base()
        {
            LogItems.Add(new LogItem(DateTime.Now, LogLevel.Trace, "MyNLog.App", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", ""));
            LogItems.Add(new LogItem(DateTime.Now, LogLevel.Debug, "MyNLog.App", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", ""));
            LogItems.Add(new LogItem(DateTime.Now, LogLevel.Info, "MyNLog.App", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", ""));
            LogItems.Add(new LogItem(DateTime.Now, LogLevel.Warn, "MyNLog.App", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", "System.NotImplementedException: Метод или операция не реализована.\n   в MyNLog.Services.LogFileService.OpenFile(String fileName) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Services\\LogFileService.cs:строка 14\n   в MyNLog.Commands.OpenLogFileCommand.ExecuteInternal(Object parameter) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Commands\\OpenLogFileCommand.cs:строка 32"));
            LogItems.Add(new LogItem(DateTime.Now, LogLevel.Error, "MyNLog.App", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", "System.NotImplementedException: Метод или операция не реализована.\n   в MyNLog.Services.LogFileService.OpenFile(String fileName) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Services\\LogFileService.cs:строка 14\n   в MyNLog.Commands.OpenLogFileCommand.ExecuteInternal(Object parameter) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Commands\\OpenLogFileCommand.cs:строка 32"));
            LogItems.Add(new LogItem(DateTime.Now, LogLevel.Fatal, "MyNLog.App", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", "System.NotImplementedException: Метод или операция не реализована.\n   в MyNLog.Services.LogFileService.OpenFile(String fileName) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Services\\LogFileService.cs:строка 14\n   в MyNLog.Commands.OpenLogFileCommand.ExecuteInternal(Object parameter) в C:\\Projects\\Desktop\\MyNLog\\MyNLog\\Commands\\OpenLogFileCommand.cs:строка 32"));
            LogItems.Add(new LogItem(DateTime.Now, LogLevel.Trace, "MyNLog.App", "Trace example", "ExecutionContext.Run => ExecutionContext.Run => ExecutionContext.RunInternal => CulturePreservingExecutionContext.CallbackWrapper => DispatcherOperation.InvokeInSecurityContext => DispatcherOperation.InvokeImpl => ExceptionWrapper.TryCatchWhen => ExceptionWrapper.InternalRealCall => Application.<.ctor>b__1_0 => App.OnStartup", ""));
        }
    }

    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<LogItem> LogItems { get; } = new ObservableCollection<LogItem>();

        public MainWindowViewModel()
        {
        }
    }
}
