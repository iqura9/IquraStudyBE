using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IquraStudyBE.Migrations
{
    /// <inheritdoc />
    public partial class QuizTaskQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupTaskQuiz_GroupTasks_GroupTaskId",
                table: "GroupTaskQuiz");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupTaskQuiz_Quizzes_QuizId",
                table: "GroupTaskQuiz");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupTaskQuiz",
                table: "GroupTaskQuiz");

            migrationBuilder.RenameTable(
                name: "GroupTaskQuiz",
                newName: "GroupTaskQuizzes");

            migrationBuilder.RenameIndex(
                name: "IX_GroupTaskQuiz_QuizId",
                table: "GroupTaskQuizzes",
                newName: "IX_GroupTaskQuizzes_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupTaskQuiz_GroupTaskId",
                table: "GroupTaskQuizzes",
                newName: "IX_GroupTaskQuizzes_GroupTaskId");

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "GroupTaskQuizzes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GroupTaskId",
                table: "GroupTaskQuizzes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupTaskQuizzes",
                table: "GroupTaskQuizzes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTaskQuizzes_GroupTasks_GroupTaskId",
                table: "GroupTaskQuizzes",
                column: "GroupTaskId",
                principalTable: "GroupTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTaskQuizzes_Quizzes_QuizId",
                table: "GroupTaskQuizzes",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupTaskQuizzes_GroupTasks_GroupTaskId",
                table: "GroupTaskQuizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupTaskQuizzes_Quizzes_QuizId",
                table: "GroupTaskQuizzes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupTaskQuizzes",
                table: "GroupTaskQuizzes");

            migrationBuilder.RenameTable(
                name: "GroupTaskQuizzes",
                newName: "GroupTaskQuiz");

            migrationBuilder.RenameIndex(
                name: "IX_GroupTaskQuizzes_QuizId",
                table: "GroupTaskQuiz",
                newName: "IX_GroupTaskQuiz_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupTaskQuizzes_GroupTaskId",
                table: "GroupTaskQuiz",
                newName: "IX_GroupTaskQuiz_GroupTaskId");

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "GroupTaskQuiz",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "GroupTaskId",
                table: "GroupTaskQuiz",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupTaskQuiz",
                table: "GroupTaskQuiz",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTaskQuiz_GroupTasks_GroupTaskId",
                table: "GroupTaskQuiz",
                column: "GroupTaskId",
                principalTable: "GroupTasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTaskQuiz_Quizzes_QuizId",
                table: "GroupTaskQuiz",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }
    }
}
