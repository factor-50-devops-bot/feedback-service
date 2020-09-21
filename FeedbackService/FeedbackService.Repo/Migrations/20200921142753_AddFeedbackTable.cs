using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FeedbackService.Repo.Migrations
{
    public partial class AddFeedbackTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Feedback");

            migrationBuilder.CreateTable(
                name: "Feedback",
                schema: "Feedback",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeedbackDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    JobId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    RequestRoleTypeID = table.Column<byte>(nullable: false),
                    FeedbackRatingTypeID = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedback",
                schema: "Feedback");
        }
    }
}
