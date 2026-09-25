-- routify_auth

CREATE TABLE usuarios (
    usuario_id      uuid PRIMARY KEY,
    nombre          varchar(80) NOT NULL,
    apellido        varchar(80) NOT NULL,
    nombre_usuario  varchar(50) NOT NULL,
    email           varchar(150) NOT NULL,
    password_hash   text NOT NULL,
    rol             varchar(30) NOT NULL DEFAULT 'Operador',
    fecha_alta      timestamp NOT NULL DEFAULT now(),

    CONSTRAINT uq_usuarios_nombre_usuario UNIQUE (nombre_usuario),
    CONSTRAINT uq_usuarios_email UNIQUE (email)
);
