using Microsoft.EntityFrameworkCore.Migrations;

namespace Praca_dyplomowa.Migrations
{
    public partial class ChangeNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Places");

            migrationBuilder.AddColumn<string>(
                name: "RouteName",
                table: "Routes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlaceName",
                table: "Places",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteName",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "PlaceName",
                table: "Places");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Routes",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Regions",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Places",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
