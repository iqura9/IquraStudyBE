using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IquraStudyBE.Migrations
{
    /// <inheritdoc />
    public partial class AddedGroupIdtoParticipation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Participations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Participations_GroupId",
                table: "Participations",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_Groups_GroupId",
                table: "Participations",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participations_Groups_GroupId",
                table: "Participations");

            migrationBuilder.DropIndex(
                name: "IX_Participations_GroupId",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Participations");
        }
    }
}
