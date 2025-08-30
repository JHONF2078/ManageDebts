-- Nos aseguramos de estar en la base de datos inicial
\connect manage_debs_db

-- Crear un esquema propio para la app
CREATE SCHEMA IF NOT EXISTS app AUTHORIZATION admin_user;

-- Crear una tabla de ejemplo
CREATE TABLE app.items (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT now()
);

-- Darle permisos al usuario admin_user (aunque ya es dueño)
GRANT ALL PRIVILEGES ON SCHEMA app TO admin_user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA app TO admin_user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA app TO admin_user;
