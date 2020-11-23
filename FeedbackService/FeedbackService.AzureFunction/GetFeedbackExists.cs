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
using System.Net;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using NewRelic.Api.Agent;
using HelpMyStreet.Utils.Utils;
using System.Threading;

namespace FeedbackService.AzureFunction
{
    public class GetFeedbackExists
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<GetFeedbackExistsRequest> _logger;

        public GetFeedbackExists(IMediator mediator, ILoggerWrapper<GetFeedbackExistsRequest> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Transaction(Web = true)]
        [FunctionName("GetFeedbackExists")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            [RequestBodyType(typeof(GetFeedbackExistsRequest), "Get Feedback Exists")] GetFeedbackExistsRequest req,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetFeedbackExists");
                bool response = await _mediator.Send(req, cancellationToken);
                return new OkObjectResult(ResponseWrapper<bool, FeedbackServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (Exception exc)
            {
                _logger.LogError("Exception occured in GetFeedbackExists", exc);
                return new ObjectResult(ResponseWrapper<bool, FeedbackServiceErrorCode>.CreateUnsuccessfulResponse(FeedbackServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
