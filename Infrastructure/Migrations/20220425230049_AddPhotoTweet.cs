using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddPhotoTweet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoAddress",
                table: "Tweets");

            migrationBuilder.CreateTable(
                name: "PhotoTweets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    TweetViewCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoTweets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoTweets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhotoTweets_UserId",
                table: "PhotoTweets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotoTweets");

            migrationBuilder.AddColumn<string>(
                name: "PhotoAddress",
                table: "Tweets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
