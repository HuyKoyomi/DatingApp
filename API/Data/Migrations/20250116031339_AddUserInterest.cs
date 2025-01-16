using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInterest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInterest",
                table: "UserInterest");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserInterest",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "UserInterest",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInterest",
                table: "UserInterest",
                columns: new[] { "AppUserId", "InterestId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserInterest_Users_AppUserId",
                table: "UserInterest",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInterest_Users_AppUserId",
                table: "UserInterest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInterest",
                table: "UserInterest");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "UserInterest");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserInterest",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInterest",
                table: "UserInterest",
                column: "Id");
        }
    }
}
