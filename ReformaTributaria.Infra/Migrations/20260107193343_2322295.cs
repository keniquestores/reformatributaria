using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReformaTributaria.Infra.Migrations
{
    /// <inheritdoc />
    public partial class _2322295 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filas",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    InscricaoFederalContribuinte = table.Column<string>(type: "text", nullable: false),
                    DataHoraInserção = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Filas_clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalSchema: "public",
                        principalTable: "clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApuracaoExecucaoCBS",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tiquete = table.Column<string>(type: "text", nullable: true),
                    TiqueteArquivo = table.Column<string>(type: "text", nullable: true),
                    DataHoraSolicitacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataHoraRetornoTicketArquivo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Mensagem = table.Column<string>(type: "text", nullable: true),
                    FilaId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApuracaoExecucaoCBS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApuracaoExecucaoCBS_Filas_FilaId",
                        column: x => x.FilaId,
                        principalSchema: "public",
                        principalTable: "Filas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApuracaoExecucaoCBS_FilaId",
                schema: "public",
                table: "ApuracaoExecucaoCBS",
                column: "FilaId");

            migrationBuilder.CreateIndex(
                name: "IX_Filas_ClienteId",
                schema: "public",
                table: "Filas",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApuracaoExecucaoCBS",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Filas",
                schema: "public");
        }
    }
}
