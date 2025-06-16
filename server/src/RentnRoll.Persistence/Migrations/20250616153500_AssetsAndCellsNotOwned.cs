using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentnRoll.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AssetsAndCellsNotOwned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LockerRental_Locker_LockerId",
                table: "LockerRental");

            migrationBuilder.DropForeignKey(
                name: "FK_Rental_BusinessGame_BusinessGameId",
                table: "Rental");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreRental_Store_StoreId",
                table: "StoreRental");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreAsset",
                table: "StoreAsset");

            migrationBuilder.DropIndex(
                name: "IX_Rental_BusinessGameId",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "BusinessGameId",
                table: "Rental");

            migrationBuilder.RenameColumn(
                name: "StoreId",
                table: "StoreRental",
                newName: "StoreAssetId");

            migrationBuilder.RenameIndex(
                name: "IX_StoreRental_StoreId",
                table: "StoreRental",
                newName: "IX_StoreRental_StoreAssetId");

            migrationBuilder.RenameColumn(
                name: "LockerId",
                table: "LockerRental",
                newName: "CellId");

            migrationBuilder.RenameIndex(
                name: "IX_LockerRental_LockerId",
                table: "LockerRental",
                newName: "IX_LockerRental_CellId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "StoreAsset",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreAsset",
                table: "StoreAsset",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StoreAsset_StoreId",
                table: "StoreAsset",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_LockerRental_Cell_CellId",
                table: "LockerRental",
                column: "CellId",
                principalTable: "Cell",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreRental_StoreAsset_StoreAssetId",
                table: "StoreRental",
                column: "StoreAssetId",
                principalTable: "StoreAsset",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LockerRental_Cell_CellId",
                table: "LockerRental");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreRental_StoreAsset_StoreAssetId",
                table: "StoreRental");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreAsset",
                table: "StoreAsset");

            migrationBuilder.DropIndex(
                name: "IX_StoreAsset_StoreId",
                table: "StoreAsset");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StoreAsset");

            migrationBuilder.RenameColumn(
                name: "StoreAssetId",
                table: "StoreRental",
                newName: "StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_StoreRental_StoreAssetId",
                table: "StoreRental",
                newName: "IX_StoreRental_StoreId");

            migrationBuilder.RenameColumn(
                name: "CellId",
                table: "LockerRental",
                newName: "LockerId");

            migrationBuilder.RenameIndex(
                name: "IX_LockerRental_CellId",
                table: "LockerRental",
                newName: "IX_LockerRental_LockerId");

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessGameId",
                table: "Rental",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreAsset",
                table: "StoreAsset",
                columns: new[] { "StoreId", "BusinessGameId" });

            migrationBuilder.CreateIndex(
                name: "IX_Rental_BusinessGameId",
                table: "Rental",
                column: "BusinessGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_LockerRental_Locker_LockerId",
                table: "LockerRental",
                column: "LockerId",
                principalTable: "Locker",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_BusinessGame_BusinessGameId",
                table: "Rental",
                column: "BusinessGameId",
                principalTable: "BusinessGame",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreRental_Store_StoreId",
                table: "StoreRental",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id");
        }
    }
}
