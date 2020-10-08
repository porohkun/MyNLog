using System.Diagnostics;
using System.Text;

namespace MyNLog.Services
{
    public class BindingErrorTraceListener : TraceListener
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetLogger(typeof(BindingErrorTraceListener).ToString());

        private readonly StringBuilder _messageBuilder = new StringBuilder();

        public override void Write(string message)
        {
            _messageBuilder.Append(message);
        }

        public override void WriteLine(string message)
        {
            Write(message);

            var fullMessage = _messageBuilder.ToString();
            Logger.Warn(fullMessage);
            _messageBuilder.Clear();
        }
    }
}
