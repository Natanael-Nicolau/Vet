using Microsoft.EntityFrameworkCore.Migrations;

namespace Vet.Web.Migrations
{
    public partial class ChangedUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CC",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CC",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
