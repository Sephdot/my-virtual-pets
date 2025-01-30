using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace my_virtual_pets_api.Migrations
{
    /// <inheritdoc />
    public partial class initial_migrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GDPRPermissions = table.Column<bool>(type: "bit", nullable: false),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageObj = table.Column<byte[]>(type: "Binary(8000)", maxLength: 8000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GlobalUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Auth0Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthUsers_GlobalUsers_GlobalUserId",
                        column: x => x.GlobalUserId,
                        principalTable: "GlobalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocalUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GlobalUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalUsers_GlobalUsers_GlobalUserId",
                        column: x => x.GlobalUserId,
                        principalTable: "GlobalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GlobalUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Personality = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pets_GlobalUsers_GlobalUserId",
                        column: x => x.GlobalUserId,
                        principalTable: "GlobalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pets_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GlobalUsers",
                columns: new[] { "Id", "DateJoined", "Email", "GDPRPermissions", "Username" },
                values: new object[,]
                {
                    { new Guid("3e2db5ff-0ac8-48b9-b341-e14371c42e7a"), new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "animalenthusiast@example.com", true, "AnimalEnthusiast" },
                    { new Guid("cb7153f7-3101-444b-a91d-8c45280672d6"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "petlover123@example.com", true, "PetLover123" },
                    { new Guid("f3922887-7fc2-447b-a08f-4fbbc663ba81"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "furryfan@example.com", false, "FurryFriendFan" }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "ImageObj" },
                values: new object[] { new Guid("550e8400-e29b-41d4-a716-446655440000"), new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 2, 0, 0, 0, 2, 8, 2, 0, 0, 0, 253, 212, 154, 115, 0, 0, 0, 22, 73, 68, 65, 84, 120, 218, 99, 252, 255, 159, 161, 63, 51, 50, 50, 50, 0, 6, 16, 1, 1, 253, 159, 2, 8, 102, 90, 89, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 } });

            migrationBuilder.InsertData(
                table: "LocalUsers",
                columns: new[] { "Id", "FirstName", "GlobalUserId", "LastName", "Password" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-5678-9101-1121-314151617181"), "Alex", new Guid("cb7153f7-3101-444b-a91d-8c45280672d6"), "Johnson", "hashedpassword123" },
                    { new Guid("b2c3d4e5-6789-0123-4567-890123456789"), "Jamie", new Guid("f3922887-7fc2-447b-a08f-4fbbc663ba81"), "Smith", "securepassword456" },
                    { new Guid("c3d4e5f6-7890-1234-5678-901234567890"), "Taylor", new Guid("3e2db5ff-0ac8-48b9-b341-e14371c42e7a"), "Brown", "randompassword789" }
                });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Description", "GlobalUserId", "ImageId", "Name", "Personality", "Type" },
                values: new object[,]
                {
                    { new Guid("d1e2f3a4-5678-9101-1121-314151617181"), null, new Guid("cb7153f7-3101-444b-a91d-8c45280672d6"), new Guid("550e8400-e29b-41d4-a716-446655440000"), "Whiskers", 2, 0 },
                    { new Guid("e2f3a4b5-6789-0123-4567-890123456789"), null, new Guid("f3922887-7fc2-447b-a08f-4fbbc663ba81"), new Guid("550e8400-e29b-41d4-a716-446655440000"), "Buddy", 3, 1 },
                    { new Guid("f3a4b5c6-7890-1234-5678-901234567890"), null, new Guid("3e2db5ff-0ac8-48b9-b341-e14371c42e7a"), new Guid("550e8400-e29b-41d4-a716-446655440000"), "Floppy", 1, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthUsers_Auth0Id",
                table: "AuthUsers",
                column: "Auth0Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthUsers_GlobalUserId",
                table: "AuthUsers",
                column: "GlobalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalUsers_Email",
                table: "GlobalUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GlobalUsers_Username",
                table: "GlobalUsers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocalUsers_GlobalUserId",
                table: "LocalUsers",
                column: "GlobalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_GlobalUserId",
                table: "Pets",
                column: "GlobalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_ImageId",
                table: "Pets",
                column: "ImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthUsers");

            migrationBuilder.DropTable(
                name: "LocalUsers");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "GlobalUsers");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
