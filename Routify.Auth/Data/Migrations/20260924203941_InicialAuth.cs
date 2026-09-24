using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Routify.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class InicialAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    apellido = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    nombre_usuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    rol = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    fecha_alta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuarios", x => x.usuario_id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_email",
                table: "usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_nombre_usuario",
                table: "usuarios",
                column: "nombre_usuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
