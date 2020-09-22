using Microsoft.Extensions.Logging;
using System;

namespace FeedbackService.AzureFunction
{
    public static class LogError
    {
        public static void Log(ILogger log, Exception exc, Object request)
        {
            NewRelic.Api.Agent.NewRelic.NoticeError(exc);
            log.LogError(exc.ToString());
        }
    }
}
