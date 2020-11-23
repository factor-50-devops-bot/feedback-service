using FeedbackService.AzureFunction;
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
    public class GetFeedbackExistsTests
    {
        private Mock<IMediator> _mediator;
        private Mock<ILoggerWrapper<GetFeedbackExistsRequest>> _logger;
        private GetFeedbackExists _classUnderTest;
        private bool _response;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILoggerWrapper<GetFeedbackExistsRequest>>();
            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<GetFeedbackExistsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => _response);
            _classUnderTest = new GetFeedbackExists(_mediator.Object, _logger.Object);

        }

        [Test]
        public async Task HappyPath_ReturnsTrue()
        {
            _response = true;
            IActionResult result = await _classUnderTest.Run(new GetFeedbackExistsRequest()
            {
                JobId = 1,
                UserId = 1,
                RequestRoleType = new RequestRoleType() { RequestRole = RequestRoles.GroupAdmin }
            }, CancellationToken.None);

            OkObjectResult objectResult = result as OkObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);

            ResponseWrapper<bool, FeedbackServiceErrorCode> deserialisedResponse = objectResult.Value as ResponseWrapper<bool, FeedbackServiceErrorCode>;
            Assert.IsNotNull(deserialisedResponse);

            Assert.IsTrue(deserialisedResponse.HasContent);
            Assert.IsTrue(deserialisedResponse.IsSuccessful);
            Assert.AreEqual(0, deserialisedResponse.Errors.Count());
            Assert.AreEqual(true, deserialisedResponse.Content);

            _mediator.Verify(x => x.Send(It.IsAny<GetFeedbackExistsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
