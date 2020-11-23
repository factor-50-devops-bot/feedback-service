using FeedbackService.Core.Exceptions;
using FeedbackService.Core.Interfaces.Repositories;
using HelpMyStreet.Contracts.FeedbackService.Request;
using HelpMyStreet.Contracts.FeedbackService.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FeedbackService.Handlers
{
    public class PostRecordFeedbackHandler : IRequestHandler<PostRecordFeedbackRequest, PostRecordFeedbackResponse>
    {
        private readonly IRepository _repository;

        public PostRecordFeedbackHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<PostRecordFeedbackResponse> Handle(PostRecordFeedbackRequest request, CancellationToken cancellationToken)
        {
            try
            {
                bool feedbackExists = await _repository.FeedbackExists(request.JobId, request.RequestRoleType.RequestRole, request.UserId);

                if (feedbackExists)
                {
                    throw new FeedbackExistsException();
                }
                else
                {
                    bool success = await _repository.AddFeedback(request);
                    var response = new PostRecordFeedbackResponse()
                    {
                        Success = success
                    };
                    return response;
                }
            }
            catch(FeedbackExistsException exc)
            {
                throw exc;
            }

            
        }
    }
}
