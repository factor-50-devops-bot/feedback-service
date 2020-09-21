using FeedbackService.Repo.EntityFramework.Entities;
using HelpMyStreet.Utils.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace FeedbackService.Repo.Helpers
{
    public static class EnumFeedbackRatingExtensions
    {
        public static void SetFeedbackRatingData(this EntityTypeBuilder<EnumFeedbackRating> entity)
        {
            var feedbackRatings = Enum.GetValues(typeof(FeedbackRating)).Cast<FeedbackRating>();

            foreach (var feedbackRating in feedbackRatings)
            {
                entity.HasData(new EnumFeedbackRating { Id = (int)feedbackRating, Name = feedbackRating.ToString() });
            }
        }
    }
}
