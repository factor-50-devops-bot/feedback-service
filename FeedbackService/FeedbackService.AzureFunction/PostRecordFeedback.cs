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
using FeedbackService.Core.Exceptions;
using System.Net;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using NewRelic.Api.Agent;
using HelpMyStreet.Utils.Utils;
using System.Threading;

namespace FeedbackService.AzureFunction
{
    public class PostRecordFeedback
    {
        private readonly IMediator _mediator;
        private readonly ILoggerWrapper<PostRecordFeedbackRequest> _logger;

        public PostRecordFeedback(IMediator mediator, ILoggerWrapper<PostRecordFeedbackRequest> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Transaction(Web = true)]
        [FunctionName("PostRecordFeedback")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PostRecordFeedbackResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            [RequestBodyType(typeof(PostRecordFeedbackRequest), "Post Record Feedback")] PostRecordFeedbackRequest req,
            CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("PostRecordFeedback");
                PostRecordFeedbackResponse response = await _mediator.Send(req, cancellationToken);
                return new OkObjectResult(ResponseWrapper<PostRecordFeedbackResponse, FeedbackServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (FeedbackExistsException exc)
            {
                return new ObjectResult(ResponseWrapper<PostRecordFeedbackResponse, FeedbackServiceErrorCode>.CreateUnsuccessfulResponse(FeedbackServiceErrorCode.FeedbackAlreadyExists, "Feedback Already Exists Error")) { StatusCode = StatusCodes.Status422UnprocessableEntity };
            }
            catch (Exception exc)
            {
                _logger.LogError("Exception occured in PostRecordFeedback", exc);
                return new ObjectResult(ResponseWrapper<PostRecordFeedbackResponse, FeedbackServiceErrorCode>.CreateUnsuccessfulResponse(FeedbackServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }            
    }
}
