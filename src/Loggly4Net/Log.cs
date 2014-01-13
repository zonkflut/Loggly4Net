using System;

namespace Loggly4Net
{
    internal class Log
    {
        public string Domain { get; set; }

        public string ExceptionData { get; set; }

        public string Identity { get; set; }

        public string Level { get; set; }

        public Location LocationInformation { get; set; }

        public string LoggerName { get; set; }

        public object MessageObject { get; set; }

        public string RenderedMessage { get; set; }

        public string ThreadName { get; set; }

        public DateTime TimeStampUtf { get; set; }

        public DateTime TimeStampLocal { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }
    }
}