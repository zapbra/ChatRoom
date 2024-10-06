using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthentication.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "hashing_algorithms",
                columns: new[] { "id", "algorithm_name" },
                values: new object[] { 1L, "SHA256" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "hashing_algorithms",
                keyColumn: "id",
                keyValue: 1L);
        }
    }
}
