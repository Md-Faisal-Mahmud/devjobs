using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevJobs.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddClaimInAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "OrganizationClaim", "true", new Guid("66c3aef9-9444-4c71-2db4-08dc050745fe") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
