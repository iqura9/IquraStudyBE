using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IquraStudyBE.Migrations
{
    /// <inheritdoc />
    public partial class createdByUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "GroupTasks",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupTasks_CreatedByUserId",
                table: "GroupTasks",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTasks_AspNetUsers_CreatedByUserId",
                table: "GroupTasks",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupTasks_AspNetUsers_CreatedByUserId",
                table: "GroupTasks");

            migrationBuilder.DropIndex(
                name: "IX_GroupTasks_CreatedByUserId",
                table: "GroupTasks");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "GroupTasks");
        }
    }
}
