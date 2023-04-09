using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class NewUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SomeDescription",
                table: "Users",
                newName: "Nickname");

            migrationBuilder.RenameColumn(
                name: "Login",
                table: "Users",
                newName: "email_phone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email_phone",
                table: "Users",
                newName: "Login");

            migrationBuilder.RenameColumn(
                name: "Nickname",
                table: "Users",
                newName: "SomeDescription");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
