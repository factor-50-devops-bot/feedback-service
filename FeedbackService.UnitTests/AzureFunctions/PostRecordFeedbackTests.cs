using FeedbackService.AzureFunction;
using FeedbackService.Core.Exceptions;
using HelpMyStreet.Contracts.FeedbackService.Request;
using HelpMyStreet.Contracts.FeedbackService.Response;
using HelpMyStreet.Contracts.Shared;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FeedbackService.UnitTests.AzureFunctions
{
    public class PostRecordFeedbackTests
    {
        private Mock<IMediator> _mediator;
        private Mock<ILoggerWrapper<PostRecordFeedbackRequest>> _logger;
        private PostRecordFeedback _classUnderTest;
        private PostRecordFeedbackResponse _response;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILoggerWrapper<PostRecordFeedbackRequest>>();
            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<PostRecordFeedbackRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => _response);
            _classUnderTest = new PostRecordFeedback(_mediator.Object, _logger.Object);

        }

        [Test]
        public async Task HappyPath_ReturnsTrue()
        {
            _response = new PostRecordFeedbackResponse()
            {
                Success = true
            };
            IActionResult result = await _classUnderTest.Run(new PostRecordFeedbackRequest()
            {
                JobId = 1,
                UserId = 1,
                RequestRoleType = new RequestRoleType() { RequestRole = RequestRoles.GroupAdmin },
                FeedbackRatingType = new FeedbackRatingType() { FeedbackRating = FeedbackRating.HappyFace}
            }, CancellationToken.None);

            OkObjectResult objectResult = result as OkObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);

            ResponseWrapper<PostRecordFeedbackResponse, FeedbackServiceErrorCode> deserialisedResponse = objectResult.Value as ResponseWrapper<PostRecordFeedbackResponse, FeedbackServiceErrorCode>;
            Assert.IsNotNull(deserialisedResponse);

            Assert.IsTrue(deserialisedResponse.HasContent);
            Assert.IsTrue(deserialisedResponse.IsSuccessful);
            Assert.AreEqual(0, deserialisedResponse.Errors.Count());
            Assert.AreEqual(true, deserialisedResponse.Content.Success);

            _mediator.Verify(x => x.Send(It.IsAny<PostRecordFeedbackRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExistingFeedback_ThrowsFeedbackExistsError()
        {
            PostRecordFeedbackRequest req = new PostRecordFeedbackRequest()
            {
                JobId = 1,
                UserId = 1,
                RequestRoleType = new RequestRoleType() { RequestRole = RequestRoles.GroupAdmin },
                FeedbackRatingType = new FeedbackRatingType() { FeedbackRating = FeedbackRating.HappyFace }
            };

            _mediator.Setup(x => x.Send(It.IsAny<PostRecordFeedbackRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new FeedbackExistsException());


            IActionResult result = await _classUnderTest.Run(req, CancellationToken.None);

            ObjectResult objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(422, objectResult.StatusCode);

            ResponseWrapper<PostRecordFeedbackResponse, FeedbackServiceErrorCode> deserialisedResponse = objectResult.Value as ResponseWrapper<PostRecordFeedbackResponse, FeedbackServiceErrorCode>;
            Assert.IsNotNull(deserialisedResponse);

            Assert.IsFalse(deserialisedResponse.HasContent);
            Assert.IsFalse(deserialisedResponse.IsSuccessful);
            Assert.AreEqual(1, deserialisedResponse.Errors.Count());
            Assert.AreEqual(FeedbackServiceErrorCode.FeedbackAlreadyExists, deserialisedResponse.Errors[0].ErrorCode);

            _mediator.Verify(x => x.Send(It.IsAny<PostRecordFeedbackRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
