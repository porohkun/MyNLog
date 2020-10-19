using System;

namespace MyNLog.Models.NlogConfig
{
    public interface INLogConnection
    {
        int MinIndex { get; }
        int MaxIndex { get; }
        event Action MaxIndexChanged;

        LogItem GetRecord(int index);
        void BeginWatch();
        void CacheAll();
        void Close();
    }
}
