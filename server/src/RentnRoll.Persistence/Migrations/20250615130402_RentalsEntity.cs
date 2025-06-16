using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentnRoll.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RentalsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rental",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Expectation"),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false),
                    BusinessGameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rental", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rental_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rental_BusinessGame_BusinessGameId",
                        column: x => x.BusinessGameId,
                        principalTable: "BusinessGame",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LockerRental",
                columns: table => new
                {
                    RentalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LockerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LockerRental", x => x.RentalId);
                    table.ForeignKey(
                        name: "FK_LockerRental_Locker_LockerId",
                        column: x => x.LockerId,
                        principalTable: "Locker",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LockerRental_Rental_RentalId",
                        column: x => x.RentalId,
                        principalTable: "Rental",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreRental",
                columns: table => new
                {
                    RentalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreRental", x => x.RentalId);
                    table.ForeignKey(
                        name: "FK_StoreRental_Rental_RentalId",
                        column: x => x.RentalId,
                        principalTable: "Rental",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreRental_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LockerRental_LockerId",
                table: "LockerRental",
                column: "LockerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_BusinessGameId",
                table: "Rental",
                column: "BusinessGameId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_UserId",
                table: "Rental",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreRental_StoreId",
                table: "StoreRental",
                column: "StoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LockerRental");

            migrationBuilder.DropTable(
                name: "StoreRental");

            migrationBuilder.DropTable(
                name: "Rental");
        }
    }
}
