using Microsoft.Win32;
using MyNLog.Services;
using System;
using System.Windows;

namespace MyNLog.Commands
{
    [PrismResourceInjection]
    public class CloseLogCommand : InjectableCommand<CloseLogCommand>
    {
        private readonly LogFileService _logFileService;

        public CloseLogCommand(LogFileService logFileService)
        {
            _logFileService = logFileService;
        }

        protected override bool CanExecuteInternal(object parameter)
        {
            return _logFileService.IsLogOpened;
        }

        protected override void ExecuteInternal(object parameter)
        {
            _logFileService.CloseLog();
        }
    }
}
