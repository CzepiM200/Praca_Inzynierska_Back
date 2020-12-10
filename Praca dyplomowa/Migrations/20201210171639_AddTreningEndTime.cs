using Microsoft.EntityFrameworkCore.Migrations;

namespace Praca_dyplomowa.Migrations
{
    public partial class AddTreningEndTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndTime",
                table: "Trainings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Trainings");
        }
    }
}
