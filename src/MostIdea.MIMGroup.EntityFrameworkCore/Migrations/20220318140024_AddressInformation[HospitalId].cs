using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MostIdea.MIMGroup.Migrations
{
    public partial class AddressInformationHospitalId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HospitalId",
                table: "AddressInformations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("CDC09AE1-CCFF-484D-D5B3-08DA0717564A"));

            migrationBuilder.CreateIndex(
                name: "IX_AddressInformations_HospitalId",
                table: "AddressInformations",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressInformations_Hospitals_HospitalId",
                table: "AddressInformations",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressInformations_Hospitals_HospitalId",
                table: "AddressInformations");

            migrationBuilder.DropIndex(
                name: "IX_AddressInformations_HospitalId",
                table: "AddressInformations");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "AddressInformations");
        }
    }
}
