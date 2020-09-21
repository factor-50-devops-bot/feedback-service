using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MediatR;
using System;
using HelpMyStreet.Contracts.FeedbackService.Request;
using HelpMyStreet.Contracts.FeedbackService.Response;
using HelpMyStreet.Contracts.Shared;
using Microsoft.AspNetCore.Http;

namespace FeedbackService.AzureFunction
{
    public class GetFeedbackExists
    {
        private readonly IMediator _mediator;

        public GetFeedbackExists(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("GetFeedbackExists")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] GetFeedbackExistsRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("GetFeedbackExists");
                bool response = await _mediator.Send(req);
                return new OkObjectResult(ResponseWrapper<bool, FeedbackServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                log.LogError("Exception occured in GetFeedbackExists", exc);
                return new ObjectResult(ResponseWrapper<bool, FeedbackServiceErrorCode>.CreateUnsuccessfulResponse(FeedbackServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
