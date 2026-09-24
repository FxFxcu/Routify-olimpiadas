using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Routify.Logistica.Data.Migrations
{
    /// <inheritdoc />
    public partial class InicialLogistica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    cliente_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    apellido = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    telefono = table.Column<string>(type: "text", nullable: true),
                    fecha_registro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clientes", x => x.cliente_id);
                });

            migrationBuilder.CreateTable(
                name: "vehiculos",
                columns: table => new
                {
                    vehiculo_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    patente = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    tipo = table.Column<string>(type: "text", nullable: false),
                    capacidad_kg = table.Column<decimal>(type: "numeric", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehiculos", x => x.vehiculo_id);
                });

            migrationBuilder.CreateTable(
                name: "direcciones",
                columns: table => new
                {
                    direccion_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cliente_id = table.Column<int>(type: "integer", nullable: true),
                    calle = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    numero = table.Column<string>(type: "text", nullable: false),
                    ciudad = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    provincia = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    codigo_postal = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    latitud = table.Column<decimal>(type: "numeric", nullable: true),
                    longitud = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_direcciones", x => x.direccion_id);
                    table.ForeignKey(
                        name: "fk_direcciones_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "clientes",
                        principalColumn: "cliente_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pedidos",
                columns: table => new
                {
                    pedido_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cliente_id = table.Column<int>(type: "integer", nullable: false),
                    fecha_pedido = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pedidos", x => x.pedido_id);
                    table.ForeignKey(
                        name: "fk_pedidos_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "clientes",
                        principalColumn: "cliente_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "envios",
                columns: table => new
                {
                    envio_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pedido_id = table.Column<int>(type: "integer", nullable: false),
                    direccion_origen_id = table.Column<int>(type: "integer", nullable: false),
                    direccion_destino_id = table.Column<int>(type: "integer", nullable: false),
                    vehiculo_id = table.Column<int>(type: "integer", nullable: true),
                    repartidor_usuario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    fecha_envio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_entrega_estimada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    costo_envio = table.Column<decimal>(type: "numeric", nullable: true),
                    estado_actual = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_envios", x => x.envio_id);
                    table.ForeignKey(
                        name: "fk_envios_direcciones_direccion_destino_id",
                        column: x => x.direccion_destino_id,
                        principalTable: "direcciones",
                        principalColumn: "direccion_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_envios_direcciones_direccion_origen_id",
                        column: x => x.direccion_origen_id,
                        principalTable: "direcciones",
                        principalColumn: "direccion_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_envios_pedidos_pedido_id",
                        column: x => x.pedido_id,
                        principalTable: "pedidos",
                        principalColumn: "pedido_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_envios_vehiculos_vehiculo_id",
                        column: x => x.vehiculo_id,
                        principalTable: "vehiculos",
                        principalColumn: "vehiculo_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pagos",
                columns: table => new
                {
                    pago_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pedido_id = table.Column<int>(type: "integer", nullable: false),
                    monto_pagado = table.Column<decimal>(type: "numeric", nullable: false),
                    medio_pago = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    id_transaccion_externa = table.Column<string>(type: "text", nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    fecha_pago = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pagos", x => x.pago_id);
                    table.ForeignKey(
                        name: "fk_pagos_pedidos_pedido_id",
                        column: x => x.pedido_id,
                        principalTable: "pedidos",
                        principalColumn: "pedido_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "paquetes",
                columns: table => new
                {
                    paquete_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pedido_id = table.Column<int>(type: "integer", nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    peso_kg = table.Column<decimal>(type: "numeric", nullable: false),
                    alto = table.Column<decimal>(type: "numeric", nullable: false),
                    ancho = table.Column<decimal>(type: "numeric", nullable: false),
                    largo = table.Column<decimal>(type: "numeric", nullable: false),
                    valor_declarado = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_paquetes", x => x.paquete_id);
                    table.ForeignKey(
                        name: "fk_paquetes_pedidos_pedido_id",
                        column: x => x.pedido_id,
                        principalTable: "pedidos",
                        principalColumn: "pedido_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "historial_estados_envio",
                columns: table => new
                {
                    historial_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    envio_id = table.Column<int>(type: "integer", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    fecha_hora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    observaciones = table.Column<string>(type: "text", nullable: true),
                    usuario_responsable_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_historial_estados_envio", x => x.historial_id);
                    table.ForeignKey(
                        name: "fk_historial_estados_envio_envios_envio_id",
                        column: x => x.envio_id,
                        principalTable: "envios",
                        principalColumn: "envio_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_clientes_email",
                table: "clientes",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_direcciones_cliente_id",
                table: "direcciones",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "ix_envios_direccion_destino_id",
                table: "envios",
                column: "direccion_destino_id");

            migrationBuilder.CreateIndex(
                name: "ix_envios_direccion_origen_id",
                table: "envios",
                column: "direccion_origen_id");

            migrationBuilder.CreateIndex(
                name: "ix_envios_pedido_id",
                table: "envios",
                column: "pedido_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_envios_vehiculo_id",
                table: "envios",
                column: "vehiculo_id");

            migrationBuilder.CreateIndex(
                name: "ix_historial_estados_envio_envio_id",
                table: "historial_estados_envio",
                column: "envio_id");

            migrationBuilder.CreateIndex(
                name: "ix_pagos_pedido_id",
                table: "pagos",
                column: "pedido_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_paquetes_pedido_id",
                table: "paquetes",
                column: "pedido_id");

            migrationBuilder.CreateIndex(
                name: "ix_pedidos_cliente_id",
                table: "pedidos",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehiculos_patente",
                table: "vehiculos",
                column: "patente",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "historial_estados_envio");

            migrationBuilder.DropTable(
                name: "pagos");

            migrationBuilder.DropTable(
                name: "paquetes");

            migrationBuilder.DropTable(
                name: "envios");

            migrationBuilder.DropTable(
                name: "direcciones");

            migrationBuilder.DropTable(
                name: "pedidos");

            migrationBuilder.DropTable(
                name: "vehiculos");

            migrationBuilder.DropTable(
                name: "clientes");
        }
    }
}
