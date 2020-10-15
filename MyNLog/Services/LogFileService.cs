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

        public event Action MaxIndexChanged;

        private string _configPath;
        private XmlLoggingConfiguration _config;
        private Target _target;

        private INLogConnection _connection;

        public void OpenConfigFile(string fileName)
        {
            Logger.Trace("OpenConfigFile begin");

            _configPath = fileName;
            _config = new XmlLoggingConfiguration(fileName);
            SelectTarget(_config.AllTargets.First(t =>
            {
                if (!(t is FileTarget ft))
                    return false;
                return ft.Layout is JsonLayout;
            }));

            Logger.Trace("OpenConfigFile end");
        }

        public void SelectTarget(Target target)
        {
            Logger.Trace("SelectTarget begin");
            if (!_config.AllTargets.Contains(target))
                throw new ArgumentException("Config doesnt contain selected target");

            if (target is FileTarget layoutTarget && layoutTarget.Layout is JsonLayout layout)
            {
                _connection = new JsonFileConnection(GetFilePath(layoutTarget), layout);
                _connection.MaxIndexChanged += () => MaxIndexChanged?.Invoke();
                (_connection as JsonFileConnection).CacheAllFile();
            }
            Logger.Trace("SelectTarget end");
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
