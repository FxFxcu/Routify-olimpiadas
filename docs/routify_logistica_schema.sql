-- routify_logistica

CREATE TABLE clientes (
    cliente_id       serial PRIMARY KEY,
    nombre           varchar(80) NOT NULL,
    apellido         varchar(80) NOT NULL,
    email            varchar(150) NOT NULL,
    telefono         varchar(30),
    fecha_registro   timestamp NOT NULL DEFAULT now(),

    CONSTRAINT uq_clientes_email UNIQUE (email)
);

CREATE TABLE direcciones (
    direccion_id     serial PRIMARY KEY,
    cliente_id       int REFERENCES clientes(cliente_id) ON DELETE CASCADE,
    calle            varchar(120) NOT NULL,
    numero           varchar(20) NOT NULL,
    ciudad           varchar(80) NOT NULL,
    provincia        varchar(80) NOT NULL,
    codigo_postal    varchar(15),
    latitud          numeric(9,6),
    longitud         numeric(9,6)
);

CREATE TABLE pedidos (
    pedido_id        serial PRIMARY KEY,
    cliente_id       int NOT NULL REFERENCES clientes(cliente_id) ON DELETE RESTRICT,
    fecha_pedido     timestamp NOT NULL DEFAULT now(),
    estado           varchar(20) NOT NULL DEFAULT 'Pendiente',
    observaciones    text
);

CREATE TABLE paquetes (
    paquete_id        serial PRIMARY KEY,
    pedido_id         int NOT NULL REFERENCES pedidos(pedido_id) ON DELETE CASCADE,
    descripcion       varchar(200) NOT NULL,
    peso_kg           numeric(8,2) NOT NULL,
    alto              numeric(8,2) NOT NULL,
    ancho             numeric(8,2) NOT NULL,
    largo             numeric(8,2) NOT NULL,
    valor_declarado   numeric(12,2) NOT NULL
);

CREATE TABLE vehiculos (
    vehiculo_id      serial PRIMARY KEY,
    patente          varchar(10) NOT NULL,
    tipo             varchar(30) NOT NULL,
    capacidad_kg     numeric(8,2) NOT NULL,
    estado           varchar(20) NOT NULL DEFAULT 'Disponible',

    CONSTRAINT uq_vehiculos_patente UNIQUE (patente)
);

CREATE TABLE envios (
    envio_id                serial PRIMARY KEY,
    pedido_id               int NOT NULL UNIQUE REFERENCES pedidos(pedido_id) ON DELETE RESTRICT,
    direccion_origen_id     int NOT NULL REFERENCES direcciones(direccion_id) ON DELETE RESTRICT,
    direccion_destino_id    int NOT NULL REFERENCES direcciones(direccion_id) ON DELETE RESTRICT,
    vehiculo_id             int REFERENCES vehiculos(vehiculo_id) ON DELETE RESTRICT,
    repartidor_usuario_id   uuid,
    fecha_envio             timestamp NOT NULL DEFAULT now(),
    fecha_entrega_estimada  timestamp,
    costo_envio             numeric(12,2),
    estado_actual           varchar(20) NOT NULL DEFAULT 'EnPreparacion'
);

CREATE TABLE historial_estados_envio (
    historial_id             serial PRIMARY KEY,
    envio_id                 int NOT NULL REFERENCES envios(envio_id) ON DELETE CASCADE,
    estado                   varchar(20) NOT NULL,
    fecha_hora               timestamp NOT NULL DEFAULT now(),
    observaciones            text,
    usuario_responsable_id   uuid
);

CREATE TABLE pagos (
    pago_id                  serial PRIMARY KEY,
    pedido_id                int NOT NULL UNIQUE REFERENCES pedidos(pedido_id) ON DELETE RESTRICT,
    monto_pagado             numeric(12,2) NOT NULL,
    medio_pago               varchar(40) NOT NULL DEFAULT 'MercadoPago',
    id_transaccion_externa   varchar(100),
    estado                   varchar(20) NOT NULL DEFAULT 'Pendiente',
    fecha_pago               timestamp
);
