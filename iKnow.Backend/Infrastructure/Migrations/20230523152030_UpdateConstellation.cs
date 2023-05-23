using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateConstellation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StarRightId",
                table: "Lines",
                newName: "StarRightNumber");

            migrationBuilder.RenameColumn(
                name: "StarLeftId",
                table: "Lines",
                newName: "StarLeftNumber");

            migrationBuilder.AddColumn<string>(
                name: "Theory",
                table: "Subtopics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Explanation",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Theory",
                table: "Subtopics");

            migrationBuilder.DropColumn(
                name: "Explanation",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "StarRightNumber",
                table: "Lines",
                newName: "StarRightId");

            migrationBuilder.RenameColumn(
                name: "StarLeftNumber",
                table: "Lines",
                newName: "StarLeftId");
        }
    }
}
