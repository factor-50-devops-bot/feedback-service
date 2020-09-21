using Microsoft.EntityFrameworkCore.Migrations;

namespace FeedbackService.Repo.Migrations
{
    public partial class AddLookup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Lookup");

            migrationBuilder.CreateTable(
                name: "FeedbackRating",
                schema: "Lookup",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackRating", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RequestRoles",
                schema: "Lookup",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestRoles", x => x.ID);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "FeedbackRating",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "HappyFace" },
                    { 2, "SadFace" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RequestRoles",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Requestor" },
                    { 2, "Recipient" },
                    { 3, "Volunteer" },
                    { 4, "GroupAdmin" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedbackRating",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "RequestRoles",
                schema: "Lookup");
        }
    }
}
