using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace my_virtual_pets_api.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
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
                    table.CheckConstraint("CK_GlobalUsers_DateJoined_MinLength", "LEN([DateJoined]) >= 1");
                    table.CheckConstraint("CK_GlobalUsers_Email_MinLength", "LEN([Email]) >= 1");
                    table.CheckConstraint("CK_GlobalUsers_GDPRPermissions_MinLength", "LEN([GDPRPermissions]) >= 1");
                    table.CheckConstraint("CK_GlobalUsers_Username_MinLength", "LEN([Username]) >= 1");
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    table.CheckConstraint("CK_LocalUsers_FirstName_MinLength", "LEN([FirstName]) >= 1");
                    table.CheckConstraint("CK_LocalUsers_LastName_MinLength", "LEN([LastName]) >= 1");
                    table.CheckConstraint("CK_LocalUsers_Password_MinLength", "LEN([Password]) >= 1");
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
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    GlobalUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => new { x.GlobalUserId, x.PetId });
                    table.ForeignKey(
                        name: "FK_Favorites_GlobalUsers_GlobalUserId",
                        column: x => x.GlobalUserId,
                        principalTable: "GlobalUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorites_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
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
                columns: new[] { "Id", "ImageUrl" },
                values: new object[,]
                {
                    { new Guid("550e8400-e29b-41d4-a716-446655440000"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/36fc0d04-407e-4e5e-ba56-6ff5c5f1878b.png" },
                    { new Guid("865fcd99-c3b7-49a4-b813-81fae96b577d"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/36fc0d04-407e-4e5e-ba56-6ff5c5f1878b.png" },
                    { new Guid("89a7cc4d-f68e-4650-b098-90a9f881f70a"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/36fc0d04-407e-4e5e-ba56-6ff5c5f1878b.png" },
                    { new Guid("9deefb0a-f90f-4f33-9379-c9f1b45d05ea"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/36fc0d04-407e-4e5e-ba56-6ff5c5f1878b.png" },
                    { new Guid("9e58b73d-2e74-4c80-a19e-9b4c818d4cc4"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/36fc0d04-407e-4e5e-ba56-6ff5c5f1878b.png" },
                    { new Guid("a9bcbce5-bc93-49d0-b7cd-68442ad73bb5"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/36fc0d04-407e-4e5e-ba56-6ff5c5f1878b.png" },
                    { new Guid("ce34c78f-6f2f-4ea9-9fa9-730b504ad202"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/36fc0d04-407e-4e5e-ba56-6ff5c5f1878b.png" },
                    { new Guid("d7f7c57b-472f-4237-b464-caeacdb46738"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/36fc0d04-407e-4e5e-ba56-6ff5c5f1878b.png" },
                    { new Guid("d93e2be0-3a04-4f2f-9f4d-9ff45e17da1d"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/36fc0d04-407e-4e5e-ba56-6ff5c5f1878b.png" },
                    { new Guid("e2d7c9b9-607f-4289-bf44-fc0e35ffcf67"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/36fc0d04-407e-4e5e-ba56-6ff5c5f1878b.png" },
                    { new Guid("fa4b84c6-e4c9-4ab4-b47a-9238a499c52f"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/36fc0d04-407e-4e5e-ba56-6ff5c5f1878b.png" }
                });

            migrationBuilder.InsertData(
                table: "LocalUsers",
                columns: new[] { "Id", "FirstName", "GlobalUserId", "LastName", "Password" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-5678-9101-1121-314151617181"), "Alex", new Guid("cb7153f7-3101-444b-a91d-8c45280672d6"), "Johnson", "$2a$11$hcwCsC725HvXv7gAwH1fO.Z4FdLqhFNyGtVxOwjBCUesgKJBwMB1S" },
                    { new Guid("b2c3d4e5-6789-0123-4567-890123456789"), "Jamie", new Guid("f3922887-7fc2-447b-a08f-4fbbc663ba81"), "Smith", "$2a$11$IY9LFG9lTEUqD4WAOZQH/ejt5I3lrxhjFoeXSDDQQKVHE1i/Z.AAu" },
                    { new Guid("c3d4e5f6-7890-1234-5678-901234567890"), "Taylor", new Guid("3e2db5ff-0ac8-48b9-b341-e14371c42e7a"), "Brown", "$2a$11$X4gus70kuwq2M1l2e6ASheZHruGGmxaNXmtoCwXve/sv/g4AnM3xi" }
                });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "DateCreated", "Description", "GlobalUserId", "ImageId", "Name", "Personality", "Score", "Type" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-5678-9012-3456-789012345678"), new DateTime(2019, 7, 30, 18, 0, 0, 0, DateTimeKind.Utc), null, new Guid("cb7153f7-3101-444b-a91d-8c45280672d6"), new Guid("d93e2be0-3a04-4f2f-9f4d-9ff45e17da1d"), "Tiger", 3, 60, 0 },
                    { new Guid("a5b6c7d8-9012-3456-7890-123456789012"), new DateTime(2024, 4, 30, 18, 0, 0, 0, DateTimeKind.Utc), null, new Guid("cb7153f7-3101-444b-a91d-8c45280672d6"), new Guid("865fcd99-c3b7-49a4-b813-81fae96b577d"), "Mittens", 3, 80, 0 },
                    { new Guid("b2c3d4e5-6789-0123-4567-890123456789"), new DateTime(2018, 7, 30, 18, 0, 0, 0, DateTimeKind.Utc), null, new Guid("f3922887-7fc2-447b-a08f-4fbbc663ba81"), new Guid("ce34c78f-6f2f-4ea9-9fa9-730b504ad202"), "Baxter", 2, 55, 1 },
                    { new Guid("b6c7d8e9-0123-4567-8901-234567890123"), new DateTime(2024, 3, 30, 18, 0, 0, 0, DateTimeKind.Utc), null, new Guid("f3922887-7fc2-447b-a08f-4fbbc663ba81"), new Guid("e2d7c9b9-607f-4289-bf44-fc0e35ffcf67"), "Rocky", 2, 10, 1 },
                    { new Guid("c7d8e9f0-1234-5678-9012-345678901234"), new DateTime(2023, 1, 30, 18, 0, 0, 0, DateTimeKind.Utc), null, new Guid("3e2db5ff-0ac8-48b9-b341-e14371c42e7a"), new Guid("d7f7c57b-472f-4237-b464-caeacdb46738"), "Hopper", 1, 20, 2 },
                    { new Guid("d1e2f3a4-5678-9101-1121-314151617181"), new DateTime(2025, 7, 30, 18, 0, 0, 0, DateTimeKind.Utc), null, new Guid("cb7153f7-3101-444b-a91d-8c45280672d6"), new Guid("550e8400-e29b-41d4-a716-446655440000"), "Whiskers", 2, 100, 0 },
                    { new Guid("d8e9f0a1-2345-6789-0123-456789012345"), new DateTime(2020, 7, 30, 18, 0, 0, 0, DateTimeKind.Utc), null, new Guid("cb7153f7-3101-444b-a91d-8c45280672d6"), new Guid("9deefb0a-f90f-4f33-9379-c9f1b45d05ea"), "Shadow", 2, 30, 0 },
                    { new Guid("e2f3a4b5-6789-0123-4567-890123456789"), new DateTime(2025, 6, 30, 18, 0, 0, 0, DateTimeKind.Utc), null, new Guid("f3922887-7fc2-447b-a08f-4fbbc663ba81"), new Guid("a9bcbce5-bc93-49d0-b7cd-68442ad73bb5"), "Buddy", 3, 90, 1 },
                    { new Guid("e9f0a1b2-3456-7890-1234-567890123456"), new DateTime(2022, 7, 30, 18, 0, 0, 0, DateTimeKind.Utc), null, new Guid("f3922887-7fc2-447b-a08f-4fbbc663ba81"), new Guid("89a7cc4d-f68e-4650-b098-90a9f881f70a"), "Luna", 3, 45, 1 },
                    { new Guid("f0a1b2c3-4567-8901-2345-678901234567"), new DateTime(2021, 7, 30, 18, 0, 0, 0, DateTimeKind.Utc), null, new Guid("3e2db5ff-0ac8-48b9-b341-e14371c42e7a"), new Guid("9e58b73d-2e74-4c80-a19e-9b4c818d4cc4"), "Cocoa", 1, 40, 2 },
                    { new Guid("f3a4b5c6-7890-1234-5678-901234567890"), new DateTime(2024, 7, 30, 18, 0, 0, 0, DateTimeKind.Utc), null, new Guid("3e2db5ff-0ac8-48b9-b341-e14371c42e7a"), new Guid("fa4b84c6-e4c9-4ab4-b47a-9238a499c52f"), "Floppy", 1, 85, 2 }
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
                name: "IX_Favorites_PetId",
                table: "Favorites",
                column: "PetId");

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
                name: "Favorites");

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
