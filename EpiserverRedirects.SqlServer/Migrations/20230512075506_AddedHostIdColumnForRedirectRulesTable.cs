using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forte.EpiserverRedirects.SqlServer.Migrations
{
    public partial class AddedHostIdColumnForRedirectRulesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HostId",
                table: "RedirectRules",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HostId",
                table: "RedirectRules");
        }
    }
}
