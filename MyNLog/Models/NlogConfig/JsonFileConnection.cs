using Newtonsoft.Json;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyNLog.Models.NlogConfig
{
    public class JsonFileConnection : INLogConnection
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetLogger(typeof(JsonFileConnection).ToString());

        public int MinIndex { get; private set; } = 0;
        public int MaxIndex { get; private set; } = -1;

        public event Action MaxIndexChanged;

        private JsonSerializer _serializer;
        private string _filename;
        private JsonLayout _layout;
        private FileSystemWatcher _watcher;

        private readonly Dictionary<int, LogItem> _cached = new Dictionary<int, LogItem>();

        public JsonFileConnection(string filename, JsonLayout layout)
        {
            _filename = filename;
            _layout = layout;
        }

        public void CacheAll()
        {
            using (var file = new StreamReader(_filename))
            {
                ReadToTheEnd(file);
            }

            MaxIndexChanged?.Invoke();
        }

        private int ReadToTheEnd(StreamReader file)
        {
            int count = 0;
            var position = file.GetPosition();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                var newPosition = file.GetPosition();
                var length = newPosition - position;
                using (var reader = new JsonTextReader(new StringReader(line)))
                {
                    var item = GetSerializer().Deserialize<LogItem>(reader);
                    item.SetIndex(++MaxIndex);
                    item.SetCoords(position, (int)length);
                    _cached.Add(item.Index, item);
                    count++;
                }
                position = newPosition;
            }
            return count;
        }

        public void CacheFileTail()
        {

        }

        public JsonSerializer GetSerializer()
        {
            if (_serializer == null)
            {
                _serializer = new JsonSerializer()
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented
                };
                _serializer.Converters.Add(new LogItemJsonConverter(_layout));
            }
            return _serializer;
        }

        public LogItem GetRecord(int index)
        {
            if (index < MinIndex || index > MaxIndex)
                return null;
            if (MinIndex > MaxIndex && _cached.Values.Count == 0)
                return null;
            if (_cached.TryGetValue(index, out LogItem item))
                return item;
            else
                throw new ArgumentException($"index '{index}' not found in cache");
        }

        public void BeginWatch()
        {
            _watcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(_filename),
                Filter = Path.GetFileName(_filename),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
            };
            _watcher.Changed += Watcher_Changed;
            _watcher.EnableRaisingEvents = true;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var lastItem = GetRecord(MaxIndex);
                if (lastItem == null) return;

                using (var file = new StreamReader(_filename))
                {
                    file.SetPosition(lastItem.FilePosition + lastItem.StringLength);
                    if (ReadToTheEnd(file) > 0)
                        MaxIndexChanged?.Invoke();
                }
            });
        }
    }
}
