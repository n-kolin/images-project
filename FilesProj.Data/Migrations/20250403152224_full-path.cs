using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilesProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class fullpath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageTags_Files_FilesId",
                table: "ImageTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageTags_Tags_TagsId",
                table: "ImageTags");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionsId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Roles_RolesId",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageTags",
                table: "ImageTags");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "PermissionRole");

            migrationBuilder.RenameTable(
                name: "ImageTags",
                newName: "FileTag");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_RolesId",
                table: "PermissionRole",
                newName: "IX_PermissionRole_RolesId");

            migrationBuilder.RenameIndex(
                name: "IX_ImageTags_TagsId",
                table: "FileTag",
                newName: "IX_FileTag_TagsId");

            migrationBuilder.AddColumn<string>(
                name: "FullPath",
                table: "Files",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermissionRole",
                table: "PermissionRole",
                columns: new[] { "PermissionsId", "RolesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileTag",
                table: "FileTag",
                columns: new[] { "FilesId", "TagsId" });

            migrationBuilder.CreateTable(
                name: "FramedImgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    FrameId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FramedImgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FramedImgs_Files_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FramedImgs_Frames_FrameId",
                        column: x => x.FrameId,
                        principalTable: "Frames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FramedImgs_FrameId",
                table: "FramedImgs",
                column: "FrameId");

            migrationBuilder.CreateIndex(
                name: "IX_FramedImgs_ImageId",
                table: "FramedImgs",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileTag_Files_FilesId",
                table: "FileTag",
                column: "FilesId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileTag_Tags_TagsId",
                table: "FileTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRole_Permissions_PermissionsId",
                table: "PermissionRole",
                column: "PermissionsId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRole_Roles_RolesId",
                table: "PermissionRole",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileTag_Files_FilesId",
                table: "FileTag");

            migrationBuilder.DropForeignKey(
                name: "FK_FileTag_Tags_TagsId",
                table: "FileTag");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRole_Permissions_PermissionsId",
                table: "PermissionRole");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRole_Roles_RolesId",
                table: "PermissionRole");

            migrationBuilder.DropTable(
                name: "FramedImgs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermissionRole",
                table: "PermissionRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileTag",
                table: "FileTag");

            migrationBuilder.DropColumn(
                name: "FullPath",
                table: "Files");

            migrationBuilder.RenameTable(
                name: "PermissionRole",
                newName: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "FileTag",
                newName: "ImageTags");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionRole_RolesId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_RolesId");

            migrationBuilder.RenameIndex(
                name: "IX_FileTag_TagsId",
                table: "ImageTags",
                newName: "IX_ImageTags_TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                columns: new[] { "PermissionsId", "RolesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageTags",
                table: "ImageTags",
                columns: new[] { "FilesId", "TagsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ImageTags_Files_FilesId",
                table: "ImageTags",
                column: "FilesId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageTags_Tags_TagsId",
                table: "ImageTags",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionsId",
                table: "RolePermissions",
                column: "PermissionsId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Roles_RolesId",
                table: "RolePermissions",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
