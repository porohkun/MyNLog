﻿using Microsoft.Win32;
using MyNLog.Services;
using System;
using System.Windows;

namespace MyNLog.Commands
{
    [PrismResourceInjection]
    public class OpenNlogConfigCommand : InjectableCommand<OpenNlogConfigCommand>
    {
        private readonly LogFileService _logFileService;
        private readonly ICommand _closeLogCommand;

        public OpenNlogConfigCommand(LogFileService logFileService, CloseLogCommand closeLogCommand)
        {
            _logFileService = logFileService;
            _closeLogCommand = closeLogCommand;
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
                    if (_closeLogCommand.CanExecute())
                        _closeLogCommand.Execute();
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
