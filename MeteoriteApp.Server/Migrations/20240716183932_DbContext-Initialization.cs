using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeteoriteApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class DbContextInitialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meteorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecClass = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Mass = table.Column<double>(type: "float", nullable: true),
                    Fall = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meteorites", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_Mass",
                table: "Meteorites",
                column: "Mass");

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_Name",
                table: "Meteorites",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_RecClass",
                table: "Meteorites",
                column: "RecClass");

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_Year",
                table: "Meteorites",
                column: "Year");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meteorites");
        }
    }
}
