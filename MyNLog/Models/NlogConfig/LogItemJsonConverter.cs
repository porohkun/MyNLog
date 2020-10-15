using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MyNLog.Models.NlogConfig
{
    public class LogItemJsonConverter : JsonConverter<LogItem>
    {
        private JsonLayout _layout;
        private readonly Dictionary<string, string> _fieldNames = new Dictionary<string, string>();

        public LogItemJsonConverter(JsonLayout layout)
        {
            _layout = layout;

            var regex = new Regex(@"\${(\w+):?[\w\d=@]*}");
            foreach (var attribute in _layout.Attributes)
            {
                if (attribute.Layout is SimpleLayout simpleLayout)
                {
                    var text = simpleLayout.Text;
                    var type = regex.Match(text).Groups[1].Value;
                    _fieldNames.Add(type, attribute.Name);
                }
            }
        }

        public override LogItem ReadJson(JsonReader reader, Type objectType, LogItem existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            var timeString = jsonObject[_fieldNames["longdate"]]?.Value<string>() ?? string.Empty;
            var levelString = jsonObject[_fieldNames["level"]]?.Value<string>() ?? string.Empty;
            var loggerString = jsonObject[_fieldNames["logger"]]?.Value<string>() ?? string.Empty;
            var messageString = jsonObject[_fieldNames["message"]]?.Value<string>() ?? string.Empty;
            var stacktraceString = jsonObject[_fieldNames["stacktrace"]]?.Value<string>() ?? string.Empty;
            var exceptionString = jsonObject[_fieldNames["exception"]]?.Value<string>() ?? string.Empty;

            DateTime.TryParse(timeString, out var time);

            Enum.TryParse<LogLevel>(levelString, true, out var level);

            var r = new Regex(" => ");
            var stacktrace = string.Join("\n in ", r.Split(stacktraceString).Reverse());

            var exception = string.Empty;
            if (!string.IsNullOrWhiteSpace(exceptionString))
                try
                {
                    var exceptionObject = JObject.Parse(exceptionString);
                    var type = exceptionObject["Type"].Value<string>();
                    var message = exceptionObject["Message"]?.Value<string>() ?? string.Empty;
                    var stack = exceptionObject["StackTrace"]?.Value<string>() ?? string.Empty;

                    var stackRegex = new Regex(@"\s{3}[^\d\s]+\s([\w.(<>\s]+\))\s[^\d\s]\s([\w:\\.]+):[^\d\s]+\s(\d+)");
                    var matches = stackRegex.Matches(stack);

                    exception = string.IsNullOrWhiteSpace(stack) ? $"{type}: {message}" : $"{type}: {message}\n{stack}";
                }
                catch
                {
                    exception = exceptionString;
                }

            return new LogItem(
                time: time,
                level: level,
                logger: loggerString,
                message: messageString,
                stackTrace: stacktrace,
                exception: exception);
        }

        public override void WriteJson(JsonWriter writer, LogItem value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
