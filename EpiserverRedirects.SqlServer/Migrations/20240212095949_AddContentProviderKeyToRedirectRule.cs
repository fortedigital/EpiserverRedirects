using Microsoft.EntityFrameworkCore.Migrations;

namespace Forte.EpiserverRedirects.SqlServer.Migrations
{
    public partial class AddContentProviderKeyToRedirectRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentProviderKey",
                table: "RedirectRules",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentProviderKey",
                table: "RedirectRules");
        }
    }
}
