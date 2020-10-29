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
using NewRelic.Api.Agent;

namespace FeedbackService.AzureFunction
{
    public class PostRecordFeedback
    {
        private readonly IMediator _mediator;

        public PostRecordFeedback(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Transaction(Web = true)]
        [FunctionName("PostRecordFeedback")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] PostRecordFeedbackRequest req,
            ILogger log)
        {

            try
            {
                log.LogInformation("PostRecordFeedback");
                PostRecordFeedbackResponse response = await _mediator.Send(req);
                return new OkObjectResult(ResponseWrapper<PostRecordFeedbackResponse, FeedbackServiceErrorCode>.CreateSuccessfulResponse(response));
            }
            catch (FeedbackExistsException exc)
            {
                return new ObjectResult(ResponseWrapper<PostRecordFeedbackResponse, FeedbackServiceErrorCode>.CreateUnsuccessfulResponse(FeedbackServiceErrorCode.FeedbackAlreadyExists, "Feedback Already Exists Error")) { StatusCode = StatusCodes.Status422UnprocessableEntity };
            }
            catch (Exception exc)
            {
                log.LogError("Exception occured in PostRecordFeedback", exc);
                return new ObjectResult(ResponseWrapper<PostRecordFeedbackResponse, FeedbackServiceErrorCode>.CreateUnsuccessfulResponse(FeedbackServiceErrorCode.InternalServerError, "Internal Error")) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }            
    }
}
