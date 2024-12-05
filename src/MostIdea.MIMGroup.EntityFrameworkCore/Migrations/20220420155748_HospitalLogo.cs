using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MostIdea.MIMGroup.Migrations
{
    public partial class HospitalLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LogoId",
                table: "Hospitals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_LogoId",
                table: "Hospitals",
                column: "LogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitals_AppBinaryObjects_LogoId",
                table: "Hospitals",
                column: "LogoId",
                principalTable: "AppBinaryObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hospitals_AppBinaryObjects_LogoId",
                table: "Hospitals");

            migrationBuilder.DropIndex(
                name: "IX_Hospitals_LogoId",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "LogoId",
                table: "Hospitals");
        }
    }
}
