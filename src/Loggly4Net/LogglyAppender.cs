using System;
using log4net.Appender;
using log4net.Core;
using RestSharp;

namespace Loggly4Net
{
    public class LogglyAppender : AppenderSkeleton
    {
        private readonly string azureRoleName;

        private readonly IRestClient client;

        public LogglyAppender(string logglyUrl, string azureRoleName = "")
        {
            this.azureRoleName = azureRoleName;
            client = new RestClient(logglyUrl);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var log = new Log
            {
                Domain = loggingEvent.Domain,
                ExceptionData = RenderFullException(loggingEvent.ExceptionObject),
                Identity = loggingEvent.Identity,
                Level = loggingEvent.Level.DisplayName,
                LocationInformation = new Location
                {
                    ClassName = loggingEvent.LocationInformation.ClassName,
                    FileName = loggingEvent.LocationInformation.FileName,
                    FullInfo = loggingEvent.LocationInformation.FullInfo,
                    LineNumber = loggingEvent.LocationInformation.LineNumber,
                    MethodName = loggingEvent.LocationInformation.MethodName,
                },
                LoggerName = loggingEvent.LoggerName,
                MessageObject = loggingEvent.MessageObject,
                RenderedMessage = loggingEvent.RenderedMessage,
                ThreadName = loggingEvent.ThreadName,
                TimeStampUtf = loggingEvent.TimeStamp.ToUniversalTime(),
                TimeStampLocal = DateTime.SpecifyKind(loggingEvent.TimeStamp, DateTimeKind.Utc).ToLocalTime(),
                UserName = loggingEvent.UserName,
                RoleName = azureRoleName,
            };

            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
            request.AddBody(log);
            client.ExecuteAsync(request, response => { });
        }

        private string RenderFullException(Exception loggingException)
        {
            if (loggingException == null) return string.Empty;
            var exception = string.Format("{0}\r\n{1}", loggingException.Message, loggingException.StackTrace);
            if (loggingException.InnerException != null) exception += "\r\n\r\n=========Inner Exception\r\n" + RenderFullException(loggingException.InnerException);
            return exception;
        }
    }
}
