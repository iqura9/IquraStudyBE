using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IquraStudyBE.Migrations
{
    /// <inheritdoc />
    public partial class missedDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupCompetition_AspNetUsers_CreateByUserId",
                table: "GroupCompetition");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCompetition_Competitions_CompetitionId",
                table: "GroupCompetition");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCompetition_Groups_GroupId",
                table: "GroupCompetition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupCompetition",
                table: "GroupCompetition");

            migrationBuilder.RenameTable(
                name: "GroupCompetition",
                newName: "GroupCompetitions");

            migrationBuilder.RenameIndex(
                name: "IX_GroupCompetition_GroupId",
                table: "GroupCompetitions",
                newName: "IX_GroupCompetitions_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupCompetition_CreateByUserId",
                table: "GroupCompetitions",
                newName: "IX_GroupCompetitions_CreateByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupCompetition_CompetitionId",
                table: "GroupCompetitions",
                newName: "IX_GroupCompetitions_CompetitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupCompetitions",
                table: "GroupCompetitions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCompetitions_AspNetUsers_CreateByUserId",
                table: "GroupCompetitions",
                column: "CreateByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCompetitions_Competitions_CompetitionId",
                table: "GroupCompetitions",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCompetitions_Groups_GroupId",
                table: "GroupCompetitions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupCompetitions_AspNetUsers_CreateByUserId",
                table: "GroupCompetitions");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCompetitions_Competitions_CompetitionId",
                table: "GroupCompetitions");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCompetitions_Groups_GroupId",
                table: "GroupCompetitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupCompetitions",
                table: "GroupCompetitions");

            migrationBuilder.RenameTable(
                name: "GroupCompetitions",
                newName: "GroupCompetition");

            migrationBuilder.RenameIndex(
                name: "IX_GroupCompetitions_GroupId",
                table: "GroupCompetition",
                newName: "IX_GroupCompetition_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupCompetitions_CreateByUserId",
                table: "GroupCompetition",
                newName: "IX_GroupCompetition_CreateByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupCompetitions_CompetitionId",
                table: "GroupCompetition",
                newName: "IX_GroupCompetition_CompetitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupCompetition",
                table: "GroupCompetition",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCompetition_AspNetUsers_CreateByUserId",
                table: "GroupCompetition",
                column: "CreateByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCompetition_Competitions_CompetitionId",
                table: "GroupCompetition",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCompetition_Groups_GroupId",
                table: "GroupCompetition",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
