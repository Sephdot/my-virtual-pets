using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace my_virtual_pets_api.Migrations
{
    /// <inheritdoc />
    public partial class AddFavouritesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Images",
                columns: new[] { "Id", "ImageUrl" },
                values: new object[,]
                {
                    { new Guid("865fcd99-c3b7-49a4-b813-81fae96b577d"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/dogyesyes" },
                    { new Guid("89a7cc4d-f68e-4650-b098-90a9f881f70a"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/dogyesyes" },
                    { new Guid("9deefb0a-f90f-4f33-9379-c9f1b45d05ea"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/dogyesyes" },
                    { new Guid("9e58b73d-2e74-4c80-a19e-9b4c818d4cc4"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/dogyesyes" },
                    { new Guid("a9bcbce5-bc93-49d0-b7cd-68442ad73bb5"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/dogyesyes" },
                    { new Guid("ce34c78f-6f2f-4ea9-9fa9-730b504ad202"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/dogyesyes" },
                    { new Guid("d7f7c57b-472f-4237-b464-caeacdb46738"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/dogyesyes" },
                    { new Guid("d93e2be0-3a04-4f2f-9f4d-9ff45e17da1d"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/dogyesyes" },
                    { new Guid("e2d7c9b9-607f-4289-bf44-fc0e35ffcf67"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/dogyesyes" },
                    { new Guid("fa4b84c6-e4c9-4ab4-b47a-9238a499c52f"), "https://my-virtual-pets-images.s3.eu-west-2.amazonaws.com/dogyesyes" }
                });

            migrationBuilder.UpdateData(
                table: "LocalUsers",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-5678-9101-1121-314151617181"),
                column: "Password",
                value: "$2a$11$hcwCsC725HvXv7gAwH1fO.Z4FdLqhFNyGtVxOwjBCUesgKJBwMB1S");

            migrationBuilder.UpdateData(
                table: "LocalUsers",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-6789-0123-4567-890123456789"),
                column: "Password",
                value: "$2a$11$IY9LFG9lTEUqD4WAOZQH/ejt5I3lrxhjFoeXSDDQQKVHE1i/Z.AAu");

            migrationBuilder.UpdateData(
                table: "LocalUsers",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-7890-1234-5678-901234567890"),
                column: "Password",
                value: "$2a$11$X4gus70kuwq2M1l2e6ASheZHruGGmxaNXmtoCwXve/sv/g4AnM3xi");

            migrationBuilder.UpdateData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("e2f3a4b5-6789-0123-4567-890123456789"),
                column: "ImageId",
                value: new Guid("a9bcbce5-bc93-49d0-b7cd-68442ad73bb5"));

            migrationBuilder.UpdateData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("f3a4b5c6-7890-1234-5678-901234567890"),
                column: "ImageId",
                value: new Guid("fa4b84c6-e4c9-4ab4-b47a-9238a499c52f"));

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Description", "GlobalUserId", "ImageId", "Name", "Personality", "Type" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-5678-9012-3456-789012345678"), null, new Guid("cb7153f7-3101-444b-a91d-8c45280672d6"), new Guid("d93e2be0-3a04-4f2f-9f4d-9ff45e17da1d"), "Tiger", 3, 0 },
                    { new Guid("a5b6c7d8-9012-3456-7890-123456789012"), null, new Guid("cb7153f7-3101-444b-a91d-8c45280672d6"), new Guid("865fcd99-c3b7-49a4-b813-81fae96b577d"), "Mittens", 3, 0 },
                    { new Guid("b2c3d4e5-6789-0123-4567-890123456789"), null, new Guid("f3922887-7fc2-447b-a08f-4fbbc663ba81"), new Guid("ce34c78f-6f2f-4ea9-9fa9-730b504ad202"), "Baxter", 2, 1 },
                    { new Guid("b6c7d8e9-0123-4567-8901-234567890123"), null, new Guid("f3922887-7fc2-447b-a08f-4fbbc663ba81"), new Guid("e2d7c9b9-607f-4289-bf44-fc0e35ffcf67"), "Rocky", 2, 1 },
                    { new Guid("c7d8e9f0-1234-5678-9012-345678901234"), null, new Guid("3e2db5ff-0ac8-48b9-b341-e14371c42e7a"), new Guid("d7f7c57b-472f-4237-b464-caeacdb46738"), "Hopper", 1, 2 },
                    { new Guid("d8e9f0a1-2345-6789-0123-456789012345"), null, new Guid("cb7153f7-3101-444b-a91d-8c45280672d6"), new Guid("9deefb0a-f90f-4f33-9379-c9f1b45d05ea"), "Shadow", 2, 0 },
                    { new Guid("e9f0a1b2-3456-7890-1234-567890123456"), null, new Guid("f3922887-7fc2-447b-a08f-4fbbc663ba81"), new Guid("89a7cc4d-f68e-4650-b098-90a9f881f70a"), "Luna", 3, 1 },
                    { new Guid("f0a1b2c3-4567-8901-2345-678901234567"), null, new Guid("3e2db5ff-0ac8-48b9-b341-e14371c42e7a"), new Guid("9e58b73d-2e74-4c80-a19e-9b4c818d4cc4"), "Cocoa", 1, 2 }
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_LocalUsers_FirstName_MinLength",
                table: "LocalUsers",
                sql: "LEN([FirstName]) >= 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_LocalUsers_LastName_MinLength",
                table: "LocalUsers",
                sql: "LEN([LastName]) >= 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_LocalUsers_Password_MinLength",
                table: "LocalUsers",
                sql: "LEN([Password]) >= 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_GlobalUsers_DateJoined_MinLength",
                table: "GlobalUsers",
                sql: "LEN([DateJoined]) >= 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_GlobalUsers_Email_MinLength",
                table: "GlobalUsers",
                sql: "LEN([Email]) >= 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_GlobalUsers_GDPRPermissions_MinLength",
                table: "GlobalUsers",
                sql: "LEN([GDPRPermissions]) >= 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_GlobalUsers_Username_MinLength",
                table: "GlobalUsers",
                sql: "LEN([Username]) >= 1");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PetId",
                table: "Favorites",
                column: "PetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropCheckConstraint(
                name: "CK_LocalUsers_FirstName_MinLength",
                table: "LocalUsers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_LocalUsers_LastName_MinLength",
                table: "LocalUsers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_LocalUsers_Password_MinLength",
                table: "LocalUsers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_GlobalUsers_DateJoined_MinLength",
                table: "GlobalUsers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_GlobalUsers_Email_MinLength",
                table: "GlobalUsers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_GlobalUsers_GDPRPermissions_MinLength",
                table: "GlobalUsers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_GlobalUsers_Username_MinLength",
                table: "GlobalUsers");

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("a9bcbce5-bc93-49d0-b7cd-68442ad73bb5"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("fa4b84c6-e4c9-4ab4-b47a-9238a499c52f"));

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-5678-9012-3456-789012345678"));

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-9012-3456-7890-123456789012"));

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-6789-0123-4567-890123456789"));

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-0123-4567-8901-234567890123"));

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-1234-5678-9012-345678901234"));

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-2345-6789-0123-456789012345"));

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-3456-7890-1234-567890123456"));

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("f0a1b2c3-4567-8901-2345-678901234567"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("865fcd99-c3b7-49a4-b813-81fae96b577d"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("89a7cc4d-f68e-4650-b098-90a9f881f70a"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("9deefb0a-f90f-4f33-9379-c9f1b45d05ea"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("9e58b73d-2e74-4c80-a19e-9b4c818d4cc4"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("ce34c78f-6f2f-4ea9-9fa9-730b504ad202"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("d7f7c57b-472f-4237-b464-caeacdb46738"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("d93e2be0-3a04-4f2f-9f4d-9ff45e17da1d"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("e2d7c9b9-607f-4289-bf44-fc0e35ffcf67"));

            migrationBuilder.UpdateData(
                table: "LocalUsers",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-5678-9101-1121-314151617181"),
                column: "Password",
                value: "hashedpassword123");

            migrationBuilder.UpdateData(
                table: "LocalUsers",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-6789-0123-4567-890123456789"),
                column: "Password",
                value: "securepassword456");

            migrationBuilder.UpdateData(
                table: "LocalUsers",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-7890-1234-5678-901234567890"),
                column: "Password",
                value: "randompassword789");

            migrationBuilder.UpdateData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("e2f3a4b5-6789-0123-4567-890123456789"),
                column: "ImageId",
                value: new Guid("550e8400-e29b-41d4-a716-446655440000"));

            migrationBuilder.UpdateData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: new Guid("f3a4b5c6-7890-1234-5678-901234567890"),
                column: "ImageId",
                value: new Guid("550e8400-e29b-41d4-a716-446655440000"));
        }
    }
}
