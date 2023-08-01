using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFileFilds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "ImageWork",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FileSize",
                table: "ImageWork",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "ImageWork",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "ImageWork",
                type: "Varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "ImageWork");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "ImageWork");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ImageWork");

            migrationBuilder.AlterColumn<int>(
                name: "FileName",
                table: "ImageWork",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);
        }
    }
}
