using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleRpgEntities.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectingRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdjacentRooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    ConnectingRoomId = table.Column<int>(type: "int", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdjacentRooms", x => new { x.RoomId, x.ConnectingRoomId });
                    table.ForeignKey(
                        name: "FK_AdjacentRooms_Rooms_ConnectingRoomId",
                        column: x => x.ConnectingRoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdjacentRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdjacentRooms_ConnectingRoomId",
                table: "AdjacentRooms",
                column: "ConnectingRoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdjacentRooms");
        }
    }
}
