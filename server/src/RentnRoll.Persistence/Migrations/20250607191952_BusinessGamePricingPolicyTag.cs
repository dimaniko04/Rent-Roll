using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentnRoll.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BusinessGamePricingPolicyTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessGame",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    BasePrice = table.Column<int>(type: "int", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessGame_Business_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusinessGame_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PricingPolicy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    TimeUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitCount = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    PricePercent = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingPolicy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricingPolicy_Business_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_Business_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PricingPolicyItem",
                columns: table => new
                {
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingPolicyItem", x => new { x.PolicyId, x.GameId });
                    table.ForeignKey(
                        name: "FK_PricingPolicyItem_BusinessGame_GameId",
                        column: x => x.GameId,
                        principalTable: "BusinessGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PricingPolicyItem_PricingPolicy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "PricingPolicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessGameTags",
                columns: table => new
                {
                    BusinessGamesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessGameTags", x => new { x.BusinessGamesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_BusinessGameTags_BusinessGame_BusinessGamesId",
                        column: x => x.BusinessGamesId,
                        principalTable: "BusinessGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessGameTags_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessGame_BusinessId",
                table: "BusinessGame",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessGame_GameId",
                table: "BusinessGame",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessGameTags_TagsId",
                table: "BusinessGameTags",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_PricingPolicy_BusinessId",
                table: "PricingPolicy",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_PricingPolicyItem_GameId",
                table: "PricingPolicyItem",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_BusinessId",
                table: "Tag",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Name_BusinessId",
                table: "Tag",
                columns: new[] { "Name", "BusinessId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessGameTags");

            migrationBuilder.DropTable(
                name: "PricingPolicyItem");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "BusinessGame");

            migrationBuilder.DropTable(
                name: "PricingPolicy");
        }
    }
}
