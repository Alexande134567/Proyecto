USE GestionProyectosDB;
GO

-- 1. ELIMINAR CUALQUIER RESTRICCIÓN DE CLAVE FORÁNEA EXISTENTE
DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) + 
              ' DROP CONSTRAINT ' + QUOTENAME(name) + ';' + CHAR(13)
FROM sys.foreign_keys;

EXEC sp_executesql @sql;
GO

-- 2. ELIMINAR TABLAS SI EXISTEN
IF OBJECT_ID('dbo.Observaciones', 'U') IS NOT NULL DROP TABLE dbo.Observaciones;
IF OBJECT_ID('dbo.Entregas', 'U') IS NOT NULL DROP TABLE dbo.Entregas;
IF OBJECT_ID('dbo.Proyectos', 'U') IS NOT NULL DROP TABLE dbo.Proyectos;
IF OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL DROP TABLE dbo.Usuarios;
IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL DROP TABLE dbo.Roles;
GO

-- 3. CREAR TABLA ROLES
CREATE TABLE Roles (
    IdRol INT PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL
);

INSERT INTO Roles (IdRol, NombreRol) VALUES 
(1, 'Administrador'),
(2, 'Docente'),
(3, 'Estudiante');
GO

-- 4. CREAR TABLA USUARIOS
CREATE TABLE Usuarios (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Clave VARCHAR(100) NOT NULL,
    NombreCompleto VARCHAR(150) NOT NULL,
    IdRol INT NOT NULL FOREIGN KEY REFERENCES Roles(IdRol)
);

INSERT INTO Usuarios (NombreUsuario, Clave, NombreCompleto, IdRol) VALUES 
('admin', 'admin123', 'Administrador del Sistema', 1),
('adelgado', 'tutora2026', 'Lic. Adriana Delgado Mg.', 2),
('kintriago', 'docente2026', 'Lic. Katherin Johana Intriago Carreña', 2),
('dalcivar', 'estudiante123', 'Alcívar Farías Diego Joseph', 3),
('dmacias', 'estudiante123', 'Macías Briones Darwin Isaac', 3);
GO

-- 5. CREAR TABLA PROYECTOS
CREATE TABLE Proyectos (
    IdProyecto INT PRIMARY KEY IDENTITY(1,1),
    Titulo VARCHAR(300) NOT NULL,
    Descripcion TEXT,
    FechaInicio DATE,
    FechaFin DATE,
    Estado VARCHAR(50) DEFAULT 'En Desarrollo'
);

INSERT INTO Proyectos (Titulo, Descripcion, FechaInicio, FechaFin, Estado) VALUES 
('Incidencia del voleibol en el trabajo en equipo del Bachillerato Técnico UEMAM 2026-2027', 
 'Estudio de caso sobre la práctica del voleibol para fortalecer la colaboración y convivencia en estudiantes del Bachillerato Técnico.', 
 '2026-05-02', '2027-02-15', 'En Desarrollo');
GO

-- 6. CREAR TABLA ENTREGAS
CREATE TABLE Entregas (
    IdEntrega INT PRIMARY KEY IDENTITY(1,1),
    IdProyecto INT NOT NULL FOREIGN KEY REFERENCES Proyectos(IdProyecto),
    IdEstudiante INT NOT NULL FOREIGN KEY REFERENCES Usuarios(IdUsuario),
    Fase VARCHAR(100) NOT NULL,
    RutaArchivo VARCHAR(500),
    FechaEntrega DATETIME DEFAULT GETDATE(),
    Estado VARCHAR(50) DEFAULT 'Enviado'
);

INSERT INTO Entregas (IdProyecto, IdEstudiante, Fase, RutaArchivo, Estado) VALUES 
(1, 4, '1. Propuesta e Introducción', '~/uploads/proyectos/Propuesta_Alcivar_Macias.pdf', 'Aprobado');
GO

-- 7. CREAR TABLA OBSERVACIONES
CREATE TABLE Observaciones (
    IdObservacion INT PRIMARY KEY IDENTITY(1,1),
    IdEntrega INT NOT NULL FOREIGN KEY REFERENCES Entregas(IdEntrega),
    IdDocente INT NOT NULL FOREIGN KEY REFERENCES Usuarios(IdUsuario),
    Comentario TEXT NOT NULL,
    FechaObservacion DATETIME DEFAULT GETDATE()
);

INSERT INTO Observaciones (IdEntrega, IdDocente, Comentario) VALUES 
(1, 2, '1. Revisar y estructurar la formulación de los Objetivos Generales y Específicos.' + CHAR(13) + CHAR(10) + '2. Aplicar normas APA 7ma Edición en las referencias.');
GO