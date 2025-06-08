using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentnRoll.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LockerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locker",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Address_City = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Address_Country = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Address_State = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Address_Street = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Address_ZipCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cell",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IotDeviceUrl = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LockerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cell", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cell_Business_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cell_Locker_LockerId",
                        column: x => x.LockerId,
                        principalTable: "Locker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LockerPricingPolicies",
                columns: table => new
                {
                    LockerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PricingPoliciesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LockerPricingPolicies", x => new { x.LockerId, x.PricingPoliciesId });
                    table.ForeignKey(
                        name: "FK_LockerPricingPolicies_Locker_LockerId",
                        column: x => x.LockerId,
                        principalTable: "Locker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LockerPricingPolicies_PricingPolicy_PricingPoliciesId",
                        column: x => x.PricingPoliciesId,
                        principalTable: "PricingPolicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cell_BusinessId",
                table: "Cell",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Cell_LockerId",
                table: "Cell",
                column: "LockerId");

            migrationBuilder.CreateIndex(
                name: "IX_LockerPricingPolicies_PricingPoliciesId",
                table: "LockerPricingPolicies",
                column: "PricingPoliciesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cell");

            migrationBuilder.DropTable(
                name: "LockerPricingPolicies");

            migrationBuilder.DropTable(
                name: "Locker");
        }
    }
}
