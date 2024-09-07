using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class AddPropertyRelatedEntitiess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove the redundant PostedOn column addition, as it already exists in the table
            // migrationBuilder.AddColumn<DateTime>(
            //     name: "PostedOn",
            //     table: "Properties",
            //     type: "datetime",
            //     nullable: false,
            //     defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            // Add only the new column UserId and the foreign key
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_UserId",
                table: "Properties",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Users_UserId",
                table: "Properties",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key and index related to UserId
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Users_UserId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_UserId",
                table: "Properties");

            // Remove the newly added columns
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Properties");

            // No need to drop the PostedOn column, as it's not being added in this migration
        }
    }
}
