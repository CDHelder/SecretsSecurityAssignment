using Microsoft.EntityFrameworkCore.Migrations;

namespace SecretsSecurityAssignment.Data.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Blocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SensitiveSecrets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncludeUserName = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensitiveSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensitiveSecrets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StateSecrets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncludeUserName = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateSecrets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopSecrets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncludeUserName = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopSecrets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "SensitiveSecrets",
                columns: new[] { "Id", "Content", "IncludeUserName", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "SensitiveSecret 1", true, "Roddel 1", null },
                    { 2, "SensitiveSecret 2", false, "Roddel 2", null }
                });

            migrationBuilder.InsertData(
                table: "StateSecrets",
                columns: new[] { "Id", "Content", "IncludeUserName", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "StateSecret 1", false, "Staatsgeheim 1", null },
                    { 2, "StateSecret 2", false, "Staatsgeheim 2", null }
                });

            migrationBuilder.InsertData(
                table: "TopSecrets",
                columns: new[] { "Id", "Content", "IncludeUserName", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "TopSecret 1", false, "Topgeheim 1", null },
                    { 2, "TopSecret 2", false, "Topgeheim 2", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensitiveSecrets_UserId",
                table: "SensitiveSecrets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StateSecrets_UserId",
                table: "StateSecrets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TopSecrets_UserId",
                table: "TopSecrets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensitiveSecrets");

            migrationBuilder.DropTable(
                name: "StateSecrets");

            migrationBuilder.DropTable(
                name: "TopSecrets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
