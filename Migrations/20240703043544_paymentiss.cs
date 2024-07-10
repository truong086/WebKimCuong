using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebKimCuong.Migrations
{
    public partial class paymentiss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "paymentId",
                table: "payments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "paymentId",
                table: "payments");
        }
    }
}
