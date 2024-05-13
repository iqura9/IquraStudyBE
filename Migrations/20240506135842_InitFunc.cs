using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IquraStudyBE.Migrations
{
    /// <inheritdoc />
    public partial class InitFunc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InitFunc",
                table: "Problems",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitFunc",
                table: "Problems");
        }
    }
}
