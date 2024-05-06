using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class MigrationFinal1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenhaHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    SenhaSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Imagens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescricaoImagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativa = table.Column<bool>(type: "bit", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Imagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_Imagens_Tbl_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Tbl_Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Comentarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario_FK = table.Column<int>(type: "int", nullable: false),
                    IdImagem_FK = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_Comentarios_Tbl_Imagens_IdImagem_FK",
                        column: x => x.IdImagem_FK,
                        principalTable: "Tbl_Imagens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tbl_Comentarios_Tbl_Usuarios_IdUsuario_FK",
                        column: x => x.IdUsuario_FK,
                        principalTable: "Tbl_Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Comentarios_IdImagem_FK",
                table: "Tbl_Comentarios",
                column: "IdImagem_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Comentarios_IdUsuario_FK",
                table: "Tbl_Comentarios",
                column: "IdUsuario_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Imagens_IdUsuario",
                table: "Tbl_Imagens",
                column: "IdUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_Comentarios");

            migrationBuilder.DropTable(
                name: "Tbl_Imagens");

            migrationBuilder.DropTable(
                name: "Tbl_Usuarios");
        }
    }
}
