using Microsoft.Win32;
using MyNLog.Services;
using System;
using System.Windows;

namespace MyNLog.Commands
{
    [PrismResourceInjection]
    public class OpenNlogConfigCommand : InjectableCommand<OpenNlogConfigCommand>
    {
        private readonly LogFileService _logFileService;

        public OpenNlogConfigCommand(LogFileService logFileService)
        {
            _logFileService = logFileService;
        }

        protected override bool CanExecuteInternal(object parameter)
        {
            return true;
        }

        protected override void ExecuteInternal(object parameter)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Nlog config|Nlog.config|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog().Value)
            {
                try
                {
                    _logFileService.OpenConfigFile(dialog.FileName);
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Cant open file '{dialog.FileName}'");
                    MessageBox.Show($"Cant open file '{dialog.FileName}'");
                }
            }
        }
    }
}
