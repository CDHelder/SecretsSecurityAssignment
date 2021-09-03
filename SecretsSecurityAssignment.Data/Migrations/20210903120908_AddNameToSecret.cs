using Microsoft.EntityFrameworkCore.Migrations;

namespace SecretsSecurityAssignment.Data.Migrations
{
    public partial class AddNameToSecret : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StateSecrets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StateSecrets",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TopSecrets",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TopSecrets",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TopSecrets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StateSecrets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SensitiveSecrets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SensitiveSecrets",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Roddel 1");

            migrationBuilder.UpdateData(
                table: "SensitiveSecrets",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Roddel 2");

            migrationBuilder.InsertData(
                table: "StateSecrets",
                columns: new[] { "Id", "Content", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "StateSecret 1", "Staatsgeheim 1", null },
                    { 2, "StateSecret 2", "Staatsgeheim 2", null }
                });

            migrationBuilder.InsertData(
                table: "TopSecrets",
                columns: new[] { "Id", "Content", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "TopSecret 1", "Topgeheim 1", null },
                    { 2, "TopSecret 2", "Topgeheim 2", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StateSecrets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StateSecrets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TopSecrets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TopSecrets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TopSecrets");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "StateSecrets");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SensitiveSecrets");

            migrationBuilder.InsertData(
                table: "StateSecrets",
                columns: new[] { "Id", "Content", "UserId" },
                values: new object[,]
                {
                    { 3, "StateSecret 1", null },
                    { 4, "StateSecret 2", null }
                });

            migrationBuilder.InsertData(
                table: "TopSecrets",
                columns: new[] { "Id", "Content", "UserId" },
                values: new object[,]
                {
                    { 5, "TopSecret 1", null },
                    { 6, "TopSecret 2", null }
                });
        }
    }
}
