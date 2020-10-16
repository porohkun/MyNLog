using System.IO;
using System.Reflection;

namespace MyNLog
{
    public static class StreamReaderExtensions
    {
        readonly static FieldInfo _charPosField = typeof(StreamReader).GetField("charPos", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        readonly static FieldInfo _byteLenField = typeof(StreamReader).GetField("byteLen", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        readonly static FieldInfo _charBufferField = typeof(StreamReader).GetField("charBuffer", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        public static long GetPosition(this StreamReader reader)
        {
            int byteLen = (int)_byteLenField.GetValue(reader);
            var position = reader.BaseStream.Position - byteLen;

            int charPos = (int)_charPosField.GetValue(reader);
            if (charPos > 0)
            {
                var charBuffer = (char[])_charBufferField.GetValue(reader);
                var encoding = reader.CurrentEncoding;
                var bytesConsumed = encoding.GetBytes(charBuffer, 0, charPos).Length;
                position += bytesConsumed;
            }

            return position;
        }

        public static void SetPosition(this StreamReader reader, long position)
        {
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(position, SeekOrigin.Begin);
        }
    }
}
