using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Praca_dyplomowa.Migrations
{
    public partial class ModifyTraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Trainings");

            migrationBuilder.AlterColumn<string>(
                name: "StartTime",
                table: "Trainings",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "Distance",
                table: "Trainings",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActivityTime",
                table: "Trainings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TrainingDescription",
                table: "Trainings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityTime",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "TrainingDescription",
                table: "Trainings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Trainings",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Distance",
                table: "Trainings",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Trainings",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Trainings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Speed",
                table: "Trainings",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Trainings",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
