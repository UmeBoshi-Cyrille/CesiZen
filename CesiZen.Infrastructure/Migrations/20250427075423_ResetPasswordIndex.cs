using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CesiZen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ResetPasswordIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResetPassword_Logins_LoginId",
                table: "ResetPassword");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResetPassword",
                table: "ResetPassword");

            migrationBuilder.RenameTable(
                name: "ResetPassword",
                newName: "ResetPasswords");

            migrationBuilder.RenameIndex(
                name: "IX_ResetPassword_LoginId",
                table: "ResetPasswords",
                newName: "IX_ResetPasswords_LoginId");

            migrationBuilder.AlterColumn<int>(
                name: "ResetFailedCount",
                table: "Logins",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResetPasswords",
                table: "ResetPasswords",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ResetPasswords_Id",
                table: "ResetPasswords",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResetPasswords_ResetToken",
                table: "ResetPasswords",
                column: "ResetToken");

            migrationBuilder.AddForeignKey(
                name: "FK_ResetPasswords_Logins_LoginId",
                table: "ResetPasswords",
                column: "LoginId",
                principalTable: "Logins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResetPasswords_Logins_LoginId",
                table: "ResetPasswords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResetPasswords",
                table: "ResetPasswords");

            migrationBuilder.DropIndex(
                name: "IX_ResetPasswords_Id",
                table: "ResetPasswords");

            migrationBuilder.DropIndex(
                name: "IX_ResetPasswords_ResetToken",
                table: "ResetPasswords");

            migrationBuilder.RenameTable(
                name: "ResetPasswords",
                newName: "ResetPassword");

            migrationBuilder.RenameIndex(
                name: "IX_ResetPasswords_LoginId",
                table: "ResetPassword",
                newName: "IX_ResetPassword_LoginId");

            migrationBuilder.AlterColumn<int>(
                name: "ResetFailedCount",
                table: "Logins",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResetPassword",
                table: "ResetPassword",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResetPassword_Logins_LoginId",
                table: "ResetPassword",
                column: "LoginId",
                principalTable: "Logins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
