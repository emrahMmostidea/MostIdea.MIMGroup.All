using Microsoft.EntityFrameworkCore.Migrations;

namespace MostIdea.MIMGroup.Migrations
{
    public partial class AddWebsiteColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Website",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "AbpUsers");
        }
    }
}
