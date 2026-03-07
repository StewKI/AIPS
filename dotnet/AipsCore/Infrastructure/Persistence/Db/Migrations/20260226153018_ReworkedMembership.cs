using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AipsCore.Infrastructure.Persistence.Db.Migrations
{
    /// <inheritdoc />
    public partial class ReworkedMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanJoin",
                table: "WhiteboardMemberships");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "WhiteboardMemberships");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "WhiteboardMemberships",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "WhiteboardMemberships");

            migrationBuilder.AddColumn<bool>(
                name: "CanJoin",
                table: "WhiteboardMemberships",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "WhiteboardMemberships",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
