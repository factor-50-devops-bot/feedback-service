using FeedbackService.Core.Interfaces.Repositories;
using FeedbackService.Handlers;
using HelpMyStreet.Contracts.FeedbackService.Request;
using HelpMyStreet.Utils.Enums;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FeedbackService.UnitTests.Handlers
{
    public class GetFeedbackExistsHandlerTests
    {
        private GetFeedbackExistsHandler _classUnderTest;
        private Mock<IRepository> _repository;
        private bool _feedbackExists;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IRepository>();
            _repository.Setup(x => x.FeedbackExists(It.IsAny<int>(), It.IsAny<RequestRoles>(), It.IsAny<int?>()))
                .ReturnsAsync(() => _feedbackExists);

            _classUnderTest = new GetFeedbackExistsHandler(_repository.Object);

        }

        [TestCase(1,1,true)]
        [TestCase(1,2, false)]
        [Test]
        public void HappyPath_ReturnsCorrectResponse(int userId, int jobId, bool feedbackExists)
        {
            _feedbackExists = feedbackExists;
            var result = _classUnderTest.Handle(new GetFeedbackExistsRequest()
            {
                UserId = userId,
                JobId = jobId,
                RequestRoleType = new RequestRoleType()
                {
                    RequestRole = RequestRoles.Requestor
                }
            }, CancellationToken.None).Result;

            Assert.AreEqual(feedbackExists, result);
            _repository.Verify(x => x.FeedbackExists(It.IsAny<int>(), It.IsAny<RequestRoles>(), It.IsAny<int?>()), Times.Once);
        }
    }
}
