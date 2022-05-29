using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ChangeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tweets_TweetId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_TweetId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "TweetId",
                table: "Tags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TweetId",
                table: "Tags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TweetId",
                table: "Tags",
                column: "TweetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tweets_TweetId",
                table: "Tags",
                column: "TweetId",
                principalTable: "Tweets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
