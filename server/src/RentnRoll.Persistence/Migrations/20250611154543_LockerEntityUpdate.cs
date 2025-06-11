using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentnRoll.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LockerEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Locker");

            migrationBuilder.DropColumn(
                name: "IotDeviceUrl",
                table: "Cell");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Locker",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Locker",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Locker",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Locker",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Locker",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Cell",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Empty",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessGameId",
                table: "Cell",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IotDeviceId",
                table: "Cell",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cell_BusinessGameId",
                table: "Cell",
                column: "BusinessGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cell_BusinessGame_BusinessGameId",
                table: "Cell",
                column: "BusinessGameId",
                principalTable: "BusinessGame",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cell_BusinessGame_BusinessGameId",
                table: "Cell");

            migrationBuilder.DropIndex(
                name: "IX_Cell_BusinessGameId",
                table: "Cell");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Locker");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Locker");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Locker");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Locker");

            migrationBuilder.DropColumn(
                name: "BusinessGameId",
                table: "Cell");

            migrationBuilder.DropColumn(
                name: "IotDeviceId",
                table: "Cell");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Locker",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Locker",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Cell",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Empty");

            migrationBuilder.AddColumn<string>(
                name: "IotDeviceUrl",
                table: "Cell",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
