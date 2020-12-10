using Microsoft.EntityFrameworkCore.Migrations;

namespace Praca_dyplomowa.Migrations
{
    public partial class ChangeNames2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Places");

            migrationBuilder.AddColumn<int>(
                name: "TrainingType",
                table: "Trainings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RouteType",
                table: "Routes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlaceType",
                table: "Places",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainingType",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "RouteType",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "PlaceType",
                table: "Places");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Trainings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Places",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
