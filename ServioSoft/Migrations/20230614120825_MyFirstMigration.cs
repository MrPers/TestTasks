using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServioSoft.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GetCompanies",
                columns: table => new
                {
                    CompanyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetCompanies", x => x.CompanyID);
                });

            migrationBuilder.CreateTable(
                name: "GetGuests",
                columns: table => new
                {
                    GuestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetGuests", x => x.GuestID);
                    table.ForeignKey(
                        name: "FK_GetGuests_GetCompanies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "GetCompanies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GetGuests_CompanyID",
                table: "GetGuests",
                column: "CompanyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GetGuests");

            migrationBuilder.DropTable(
                name: "GetCompanies");
        }
    }
}
