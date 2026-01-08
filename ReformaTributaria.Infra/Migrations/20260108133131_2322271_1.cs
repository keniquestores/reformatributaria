using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReformaTributaria.Infra.Migrations
{
    /// <inheritdoc />
    public partial class _2322271_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filas_clientes_ClienteId",
                schema: "public",
                table: "Filas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_clientes",
                schema: "public",
                table: "clientes");

            migrationBuilder.DropIndex(
                name: "ix_clientes_ativo",
                schema: "public",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "ClientId",
                schema: "public",
                table: "Filas");

            migrationBuilder.RenameTable(
                name: "clientes",
                schema: "public",
                newName: "Clientes",
                newSchema: "public");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                schema: "public",
                table: "Clientes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Filas_Clientes_ClienteId",
                schema: "public",
                table: "Filas",
                column: "ClienteId",
                principalSchema: "public",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filas_Clientes_ClienteId",
                schema: "public",
                table: "Filas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes",
                schema: "public",
                table: "Clientes");

            migrationBuilder.RenameTable(
                name: "Clientes",
                schema: "public",
                newName: "clientes",
                newSchema: "public");

            migrationBuilder.AddColumn<long>(
                name: "ClientId",
                schema: "public",
                table: "Filas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_clientes",
                schema: "public",
                table: "clientes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "ix_clientes_ativo",
                schema: "public",
                table: "clientes",
                column: "Ativo");

            migrationBuilder.AddForeignKey(
                name: "FK_Filas_clientes_ClienteId",
                schema: "public",
                table: "Filas",
                column: "ClienteId",
                principalSchema: "public",
                principalTable: "clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
