using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReformaTributaria.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "clientes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestorId = table.Column<string>(type: "varchar(50)", nullable: false),
                    RazaoSocial = table.Column<string>(type: "varchar(255)", nullable: false),
                    InscricaoFederal = table.Column<string>(type: "varchar(20)", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ChavePrivada = table.Column<string>(type: "text", nullable: true),
                    ClientId = table.Column<string>(type: "varchar(255)", nullable: true),
                    ClientSecret = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_clientes_ativo",
                schema: "public",
                table: "clientes",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "ix_clientes_inscricao_federal",
                schema: "public",
                table: "clientes",
                column: "InscricaoFederal",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_clientes_questor_id",
                schema: "public",
                table: "clientes",
                column: "QuestorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clientes",
                schema: "public");
        }
    }
}
