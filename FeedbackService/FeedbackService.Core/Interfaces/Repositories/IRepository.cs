using HelpMyStreet.Contracts.FeedbackService.Request;
using HelpMyStreet.Utils.Enums;
using System.Threading.Tasks;

namespace FeedbackService.Core.Interfaces.Repositories
{
    public interface IRepository
    {
        Task<bool> AddFeedback(PostRecordFeedbackRequest request);

        Task<bool> FeedbackExists(int jobId, RequestRoles requestRoles, int? userId);

    }
}
