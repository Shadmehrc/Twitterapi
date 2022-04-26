using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddTweetTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "PhotoTweets");

            migrationBuilder.DropColumn(
                name: "TweetViewCount",
                table: "PhotoTweets");

            migrationBuilder.CreateTable(
                name: "TweetTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TweetId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TweetTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TweetTags_Tweets_TweetId",
                        column: x => x.TweetId,
                        principalTable: "Tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TweetTags_TweetId",
                table: "TweetTags",
                column: "TweetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TweetTags");

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "PhotoTweets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TweetViewCount",
                table: "PhotoTweets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
