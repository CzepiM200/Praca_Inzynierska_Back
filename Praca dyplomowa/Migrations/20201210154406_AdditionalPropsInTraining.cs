using Microsoft.EntityFrameworkCore.Migrations;

namespace Praca_dyplomowa.Migrations
{
    public partial class AdditionalPropsInTraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Distance",
                table: "Trainings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Speed",
                table: "Trainings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Trainings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Trainings");
        }
    }
}
