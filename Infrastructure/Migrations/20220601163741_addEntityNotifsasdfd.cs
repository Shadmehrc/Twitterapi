using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addEntityNotifsasdfd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTagged_Tweets_TweetId",
                table: "UserTagged");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTagged",
                table: "UserTagged");

            migrationBuilder.RenameTable(
                name: "UserTagged",
                newName: "UserTaggeds");

            migrationBuilder.RenameIndex(
                name: "IX_UserTagged_TweetId",
                table: "UserTaggeds",
                newName: "IX_UserTaggeds_TweetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTaggeds",
                table: "UserTaggeds",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTaggeds_Tweets_TweetId",
                table: "UserTaggeds",
                column: "TweetId",
                principalTable: "Tweets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTaggeds_Tweets_TweetId",
                table: "UserTaggeds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTaggeds",
                table: "UserTaggeds");

            migrationBuilder.RenameTable(
                name: "UserTaggeds",
                newName: "UserTagged");

            migrationBuilder.RenameIndex(
                name: "IX_UserTaggeds_TweetId",
                table: "UserTagged",
                newName: "IX_UserTagged_TweetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTagged",
                table: "UserTagged",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTagged_Tweets_TweetId",
                table: "UserTagged",
                column: "TweetId",
                principalTable: "Tweets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
