using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevJobs.Api.Migrations
{
    /// <inheritdoc />
    public partial class RolesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("77c3aef9-9444-4c71-2db4-08dc050745fe"), "067ee691-1c33-4e92-9dcb-fddd60698f51", "Admin", "ADMIN" },
                    { new Guid("78c3aef9-9444-4c71-2db4-08dc050745ff"), "067ee691-1c33-4e92-9dcb-fddd60698f52", "Developer", "DEVELOPER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff"),
                column: "ConcurrencyStamp",
                value: "067ee691-1c33-4e92-9dcb-fddd60698f48");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("77c3aef9-9444-4c71-2db4-08dc050745fe"), new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe") },
                    { new Guid("78c3aef9-9444-4c71-2db4-08dc050745ff"), new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("77c3aef9-9444-4c71-2db4-08dc050745fe"), new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("78c3aef9-9444-4c71-2db4-08dc050745ff"), new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("77c3aef9-9444-4c71-2db4-08dc050745fe"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("78c3aef9-9444-4c71-2db4-08dc050745ff"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66c3aef9-9444-4c71-2db4-08dc050745ff"),
                column: "ConcurrencyStamp",
                value: "067ee691-1c33-4e92-9dcb-fddd60698f47");
        }
    }
}
