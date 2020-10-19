using MyNLog.Models;
using MyNLog.Models.NlogConfig;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Linq;

namespace MyNLog.Services
{
    [PrismSingleton]
    public class LogFileService
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetLogger(typeof(LogFileService).ToString());

        public int MinIndex => _connection.MinIndex;
        public int MaxIndex => _connection.MaxIndex;

        public bool IsLogOpened => _connection != null;

        public event Action MaxIndexChanged;
        public event Action SourceConnected;
        public event Action SourceDisconnected;

        private XmlLoggingConfiguration _config;

        private INLogConnection _connection;

        public void OpenConfigFile(string fileName)
        {
            _config = new XmlLoggingConfiguration(fileName);
            SelectTarget(_config.AllTargets.First(t =>
            {
                if (!(t is FileTarget ft))
                    return false;
                return ft.Layout is JsonLayout;
            }));
        }

        internal void CloseLog()
        {
            _connection.Close();
            _connection = null;
            SourceDisconnected?.Invoke();
        }

        public void SelectTarget(Target target)
        {
            if (!_config.AllTargets.Contains(target))
                throw new ArgumentException("Config doesnt contain selected target");

            if (target is FileTarget layoutTarget && layoutTarget.Layout is JsonLayout layout)
            {
                _connection = new JsonFileConnection(GetFilePath(layoutTarget), layout);
                _connection.MaxIndexChanged += () => MaxIndexChanged?.Invoke();
                _connection.CacheAll();
                _connection.BeginWatch();
                SourceConnected?.Invoke();
            }
        }

        private string GetFilePath(FileTarget target)
        {
            var fullFileNameField = typeof(FileTarget).GetField("_fullFileName", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var fullFileName = fullFileNameField.GetValue(target);
            var filePathLayoutType = typeof(FileTarget).Assembly.GetType("NLog.Internal.FilePathLayout");
            var getCleanFileName = filePathLayoutType.GetMethod("GetCleanFileName", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var cleanedFixedResult = filePathLayoutType.GetField("_cleanedFixedResult", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return getCleanFileName.Invoke(fullFileName, new object[] { cleanedFixedResult.GetValue(fullFileName) }) as string;
        }

        public LogItem GetRecord(int index)
        {
            return _connection.GetRecord(index);
        }

    }
}
