using System;

namespace FeedbackService.Core.Exceptions
{
    public class FeedbackExistsException : Exception
    {
        public FeedbackExistsException() : base("FeedbackAlreadyExists")
        {
        }
    }
}
