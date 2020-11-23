using FeedbackService.Core.Exceptions;
using FeedbackService.Core.Interfaces.Repositories;
using FeedbackService.Handlers;
using HelpMyStreet.Contracts.FeedbackService.Request;
using HelpMyStreet.Utils.Enums;
using Moq;
using NUnit.Framework;
using Polly.Caching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FeedbackService.UnitTests.Handlers
{
    public class PostRecordFeedbackHandlerTests
    {
        private PostRecordFeedbackHandler _classUnderTest;
        private Mock<IRepository> _repository;
        private bool _feedbackAdded;
        private bool _feedbackExists;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IRepository>();
            _repository.Setup(x => x.AddFeedback(It.IsAny<PostRecordFeedbackRequest>()))
                .ReturnsAsync(() => _feedbackAdded);

            _repository.Setup(x => x.FeedbackExists(It.IsAny<int>(), It.IsAny<RequestRoles>(), It.IsAny<int?>()))
               .ReturnsAsync(() => _feedbackExists);

            _classUnderTest = new PostRecordFeedbackHandler(_repository.Object);

        }

        [Test]
        public void HappyPath_ReturnsCorrectResponse()
        {
            _feedbackExists = false;
            _feedbackAdded = true;
            var result = _classUnderTest.Handle(new PostRecordFeedbackRequest()
            {
                FeedbackRatingType = new FeedbackRatingType() { FeedbackRating = FeedbackRating.HappyFace},
                UserId = 1,
                JobId = 1,
                RequestRoleType = new RequestRoleType()
                {
                    RequestRole = RequestRoles.Requestor
                }
            }, CancellationToken.None).Result;

            Assert.AreEqual(_feedbackAdded, result.Success);
            _repository.Verify(x => x.FeedbackExists(It.IsAny<int>(), It.IsAny<RequestRoles>(), It.IsAny<int?>()), Times.Once);
            _repository.Verify(x => x.AddFeedback(It.IsAny<PostRecordFeedbackRequest>()), Times.Once);
        }

        [Test]
        public void SadPath_ThrowsException()
        {
            _feedbackExists = true;
            _feedbackAdded = false;

            Exception ex = Assert.ThrowsAsync<FeedbackExistsException>(() => _classUnderTest.Handle(new PostRecordFeedbackRequest()
            {
                FeedbackRatingType = new FeedbackRatingType() { FeedbackRating = FeedbackRating.HappyFace },
                UserId = 1,
                JobId = 1,
                RequestRoleType = new RequestRoleType()
                {
                    RequestRole = RequestRoles.Requestor
                }
            }, CancellationToken.None));

            _repository.Verify(x => x.FeedbackExists(It.IsAny<int>(), It.IsAny<RequestRoles>(), It.IsAny<int?>()), Times.Once);
            _repository.Verify(x => x.AddFeedback(It.IsAny<PostRecordFeedbackRequest>()), Times.Never);

        }
    }
}
