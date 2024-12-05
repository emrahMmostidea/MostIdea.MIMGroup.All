using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MostIdea.MIMGroup.Migrations
{
    public partial class AssistanceVsUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssistanceVsUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssistanceId = table.Column<long>(type: "bigint", nullable: false),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssistanceVsUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssistanceVsUsers_AbpUsers_AssistanceId",
                        column: x => x.AssistanceId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AssistanceVsUsers_AbpUsers_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssistanceVsUsers_AssistanceId",
                table: "AssistanceVsUsers",
                column: "AssistanceId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistanceVsUsers_DoctorId",
                table: "AssistanceVsUsers",
                column: "DoctorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssistanceVsUsers");
        }
    }
}
