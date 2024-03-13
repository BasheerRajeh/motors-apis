using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Changingcolumnname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedData",
                table: "Messages");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Messages");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedData",
                table: "Messages",
                type: "datetime2",
                nullable: true);
        }
    }
}
