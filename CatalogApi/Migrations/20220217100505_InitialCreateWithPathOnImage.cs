using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogApi.Migrations
{
    public partial class InitialCreateWithPathOnImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Films",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Films");
        }
    }
}
