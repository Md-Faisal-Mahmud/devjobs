using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevJobs.Api.Migrations
{
    /// <inheritdoc />
    public partial class ProfileImageNameColumnInUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe"),
                column: "ImageName",
                value: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff"),
                column: "ImageName",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "AspNetUsers");
        }
    }
}
