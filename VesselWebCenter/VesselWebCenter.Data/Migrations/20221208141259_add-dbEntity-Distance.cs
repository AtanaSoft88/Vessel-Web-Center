using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VesselWebCenter.Data.Migrations
{
    public partial class adddbEntityDistance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Distances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VesselId = table.Column<int>(type: "int", nullable: true),
                    VesselName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VesselDistance = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Distances_Vessels_VesselId",
                        column: x => x.VesselId,
                        principalTable: "Vessels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Distances_VesselId",
                table: "Distances",
                column: "VesselId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Distances");
        }
    }
}
