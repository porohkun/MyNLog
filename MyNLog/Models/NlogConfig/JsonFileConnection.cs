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

        private readonly Dictionary<int, LogItem> _cached = new Dictionary<int, LogItem>();

        public JsonFileConnection(string filename, JsonLayout layout)
        {
            _filename = filename;
            _layout = layout;
        }

        public void CacheAllFile()
        {
            Logger.Trace("CacheAllFile begin");
            foreach (var line in File.ReadAllLines(_filename))
            {
                using (var reader = new JsonTextReader(new StringReader(line)))
                {
                    var item = GetSerializer().Deserialize<LogItem>(reader);
                    item.SetIndex(++MaxIndex);
                    _cached.Add(item.Index, item);
                }
            }
            Logger.Trace("MaxIndexChanged calling");
            MaxIndexChanged?.Invoke();
            Logger.Trace("CacheAllFile end");
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
    }
}
