using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VesselWebCenter.Data.Migrations
{
    public partial class crew_member_hire_dateadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateHired",
                table: "CrewMembers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateHired",
                table: "CrewMembers");
        }
    }
}
