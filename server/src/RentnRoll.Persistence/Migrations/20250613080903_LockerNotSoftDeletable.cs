using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentnRoll.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LockerNotSoftDeletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Locker");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Locker");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Locker",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Locker");

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
        }
    }
}
