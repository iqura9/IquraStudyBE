using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IquraStudyBE.Migrations
{
    /// <inheritdoc />
    public partial class GroupTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupTaskId",
                table: "QuizSubmittions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QuizSubmittions_GroupTaskId",
                table: "QuizSubmittions",
                column: "GroupTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizSubmittions_GroupTasks_GroupTaskId",
                table: "QuizSubmittions",
                column: "GroupTaskId",
                principalTable: "GroupTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizSubmittions_GroupTasks_GroupTaskId",
                table: "QuizSubmittions");

            migrationBuilder.DropIndex(
                name: "IX_QuizSubmittions_GroupTaskId",
                table: "QuizSubmittions");

            migrationBuilder.DropColumn(
                name: "GroupTaskId",
                table: "QuizSubmittions");
        }
    }
}
