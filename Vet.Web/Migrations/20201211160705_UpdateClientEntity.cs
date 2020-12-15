using Microsoft.EntityFrameworkCore.Migrations;

namespace Vet.Web.Migrations
{
    public partial class UpdateClientEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceId",
                table: "Clients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InsuranceId",
                table: "Clients",
                nullable: true);
        }
    }
}
