using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IquraStudyBE.Migrations
{
    /// <inheritdoc />
    public partial class GiupTaskQuiztable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupTaskQuiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupTaskId = table.Column<int>(type: "integer", nullable: true),
                    QuizId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTaskQuiz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupTaskQuiz_GroupTasks_GroupTaskId",
                        column: x => x.GroupTaskId,
                        principalTable: "GroupTasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroupTaskQuiz_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupTaskQuiz_GroupTaskId",
                table: "GroupTaskQuiz",
                column: "GroupTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTaskQuiz_QuizId",
                table: "GroupTaskQuiz",
                column: "QuizId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupTaskQuiz");
        }
    }
}
