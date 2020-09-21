using FeedbackService.Repo.EntityFramework.Entities;
using FeedbackService.Repo.Extensions;
using FeedbackService.Repo.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FeedbackService.Repo
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            SqlConnection conn = (SqlConnection)Database.GetDbConnection();
            conn.AddAzureToken();
        }

        public virtual DbSet<EnumFeedbackRating> EnumFeedbackRating { get; set; }
        public virtual DbSet<EnumRequestRoles> EnumRequestRoles { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnumFeedbackRating>(entity =>
            {
                entity.ToTable("FeedbackRating", "Lookup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.SetFeedbackRatingData();
            });

            modelBuilder.Entity<EnumRequestRoles>(entity =>
            {
                entity.ToTable("RequestRoles", "Lookup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.SetRequestRolesData();
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback", "Feedback");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FeedbackDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FeedbackRatingTypeId).HasColumnName("FeedbackRatingTypeID");

                entity.Property(e => e.RequestRoleTypeId).HasColumnName("RequestRoleTypeID");
            });
        }
    }
}
