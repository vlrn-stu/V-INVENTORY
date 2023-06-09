using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace V_INVENTORY_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "InventoryItemLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_InventoryItemLocations", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BuyDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_InventoryItems_InventoryItemLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "InventoryItemLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "InventoryItemImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageData = table.Column<byte[]>(type: "bytea", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_InventoryItemImages", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_InventoryItemImages_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_InventoryItemImages_InventoryItemId",
                table: "InventoryItemImages",
                column: "InventoryItemId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_LocationId",
                table: "InventoryItems",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "InventoryItemImages");

            _ = migrationBuilder.DropTable(
                name: "InventoryItems");

            _ = migrationBuilder.DropTable(
                name: "InventoryItemLocations");
        }
    }
}