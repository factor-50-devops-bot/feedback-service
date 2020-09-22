using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Web.Http;

namespace FeedbackService.AzureFunction
{
    public class HealthCheck
    {
        [FunctionName("HealthCheck")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed health check request.");

                return new OkObjectResult("I'm alive!");
            }
            catch (Exception exc)
            {
                LogError.Log(log, exc, req);
                return new InternalServerErrorResult();
            }
        }
    }
}
