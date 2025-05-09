using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentnRoll.Persistence.Migrations;

/// <inheritdoc />
public partial class ConfiguredUserEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "BirthDate",
            table: "AspNetUsers",
            type: "date",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<string>(
            name: "Country",
            table: "AspNetUsers",
            type: "varchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "AspNetUsers",
            type: "datetime",
            nullable: false,
            defaultValueSql: "GETDATE()");

        migrationBuilder.AddColumn<DateTime>(
            name: "DeletedAt",
            table: "AspNetUsers",
            type: "datetime",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "FirstName",
            table: "AspNetUsers",
            type: "varchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "AspNetUsers",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "LastName",
            table: "AspNetUsers",
            type: "varchar(200)",
            maxLength: 200,
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUsers_IsDeleted",
            table: "AspNetUsers",
            column: "IsDeleted",
            filter: "IsDeleted = 0");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_AspNetUsers_IsDeleted",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "BirthDate",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "Country",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "DeletedAt",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "FirstName",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "LastName",
            table: "AspNetUsers");
    }
}