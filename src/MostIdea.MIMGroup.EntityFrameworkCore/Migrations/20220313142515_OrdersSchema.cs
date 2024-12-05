using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MostIdea.MIMGroup.Migrations
{
    public partial class OrdersSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AddressInformations_AddressInformationId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "AddressInformationId",
                table: "Orders",
                newName: "InvoiceAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_AddressInformationId",
                table: "Orders",
                newName: "IX_Orders_InvoiceAddressId");

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryAddressId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "OrderComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_OrderComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderComments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryAddressId",
                table: "Orders",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderComments_OrderId",
                table: "OrderComments",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AddressInformations_DeliveryAddressId",
                table: "Orders",
                column: "DeliveryAddressId",
                principalTable: "AddressInformations",
                principalColumn: "Id"
                );

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AddressInformations_InvoiceAddressId",
                table: "Orders",
                column: "InvoiceAddressId",
                principalTable: "AddressInformations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AddressInformations_DeliveryAddressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AddressInformations_InvoiceAddressId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderComments");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryAddressId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "InvoiceAddressId",
                table: "Orders",
                newName: "AddressInformationId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_InvoiceAddressId",
                table: "Orders",
                newName: "IX_Orders_AddressInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AddressInformations_AddressInformationId",
                table: "Orders",
                column: "AddressInformationId",
                principalTable: "AddressInformations",
                principalColumn: "Id");
        }
    }
}
