using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ODataBookStore.Migrations
{
    public partial class fixtablename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Address_LocationName",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_Press_PressId",
                table: "Book");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Press",
                table: "Press");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Book",
                table: "Book");

            migrationBuilder.RenameTable(
                name: "Press",
                newName: "Presses");

            migrationBuilder.RenameTable(
                name: "Book",
                newName: "Books");

            migrationBuilder.RenameIndex(
                name: "IX_Book_PressId",
                table: "Books",
                newName: "IX_Books_PressId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_LocationName",
                table: "Books",
                newName: "IX_Books_LocationName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Presses",
                table: "Presses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Address_LocationName",
                table: "Books",
                column: "LocationName",
                principalTable: "Address",
                principalColumn: "City",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Presses_PressId",
                table: "Books",
                column: "PressId",
                principalTable: "Presses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Address_LocationName",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Presses_PressId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Presses",
                table: "Presses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "Presses",
                newName: "Press");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "Book");

            migrationBuilder.RenameIndex(
                name: "IX_Books_PressId",
                table: "Book",
                newName: "IX_Book_PressId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_LocationName",
                table: "Book",
                newName: "IX_Book_LocationName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Press",
                table: "Press",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Book",
                table: "Book",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Address_LocationName",
                table: "Book",
                column: "LocationName",
                principalTable: "Address",
                principalColumn: "City",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Press_PressId",
                table: "Book",
                column: "PressId",
                principalTable: "Press",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
