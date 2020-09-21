using FeedbackService.Repo.EntityFramework.Entities;
using HelpMyStreet.Utils.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace FeedbackService.Repo.Helpers
{
    public static class EnumRequestRolesExtensions
    {
        public static void SetRequestRolesData(this EntityTypeBuilder<EnumRequestRoles> entity)
        {
            var requestRoles = Enum.GetValues(typeof(RequestRoles)).Cast<RequestRoles>();

            foreach (var requestRole in requestRoles)
            {
                entity.HasData(new EnumRequestRoles { Id = (int)requestRole, Name = requestRole.ToString() });
            }
        }
    }
}
