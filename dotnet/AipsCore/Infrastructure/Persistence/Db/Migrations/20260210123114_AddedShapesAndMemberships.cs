using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AipsCore.Infrastructure.Persistence.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddedShapesAndMemberships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhiteboardOwnerId",
                table: "Whiteboards",
                newName: "OwnerId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Whiteboards",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Whiteboards",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JoinPolicy",
                table: "Whiteboards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxParticipants",
                table: "Whiteboards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Whiteboards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shapes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WhiteboardId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    PositionX = table.Column<int>(type: "integer", nullable: false),
                    PositionY = table.Column<int>(type: "integer", nullable: false),
                    Color = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    EndPositionX = table.Column<int>(type: "integer", nullable: true),
                    EndPositionY = table.Column<int>(type: "integer", nullable: true),
                    Thickness = table.Column<int>(type: "integer", nullable: true),
                    TextValue = table.Column<string>(type: "text", nullable: true),
                    TextSize = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shapes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shapes_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shapes_Whiteboards_WhiteboardId",
                        column: x => x.WhiteboardId,
                        principalTable: "Whiteboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WhiteboardMemberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WhiteboardId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsBanned = table.Column<bool>(type: "boolean", nullable: false),
                    EditingEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CanJoin = table.Column<bool>(type: "boolean", nullable: false),
                    LastInteractedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhiteboardMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhiteboardMemberships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WhiteboardMemberships_Whiteboards_WhiteboardId",
                        column: x => x.WhiteboardId,
                        principalTable: "Whiteboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Whiteboards_OwnerId",
                table: "Whiteboards",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Shapes_AuthorId",
                table: "Shapes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Shapes_WhiteboardId",
                table: "Shapes",
                column: "WhiteboardId");

            migrationBuilder.CreateIndex(
                name: "IX_WhiteboardMemberships_UserId",
                table: "WhiteboardMemberships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WhiteboardMemberships_WhiteboardId",
                table: "WhiteboardMemberships",
                column: "WhiteboardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Whiteboards_Users_OwnerId",
                table: "Whiteboards",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Whiteboards_Users_OwnerId",
                table: "Whiteboards");

            migrationBuilder.DropTable(
                name: "Shapes");

            migrationBuilder.DropTable(
                name: "WhiteboardMemberships");

            migrationBuilder.DropIndex(
                name: "IX_Whiteboards_OwnerId",
                table: "Whiteboards");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Whiteboards");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Whiteboards");

            migrationBuilder.DropColumn(
                name: "JoinPolicy",
                table: "Whiteboards");

            migrationBuilder.DropColumn(
                name: "MaxParticipants",
                table: "Whiteboards");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Whiteboards");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Whiteboards",
                newName: "WhiteboardOwnerId");
        }
    }
}
