-- =========================================
-- TABLA ROLES
-- =========================================
IF OBJECT_ID('Roles', 'U') IS NOT NULL DROP TABLE Roles;

CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);

-- =========================================
-- TABLA AREAS
-- =========================================
IF OBJECT_ID('Areas', 'U') IS NOT NULL DROP TABLE Areas;

CREATE TABLE Areas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1
);

-- =========================================
-- TABLA USERS
-- =========================================
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    ContactInfo NVARCHAR(200) NOT NULL,
    RoleId INT NOT NULL,
    Status BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId)
        REFERENCES Roles(Id)
);

-- =========================================
-- TABLA USER AREA
-- =========================================
IF OBJECT_ID('UserArea', 'U') IS NOT NULL DROP TABLE UserArea;

CREATE TABLE UserArea (
    UserId INT PRIMARY KEY,
    AreaId INT NOT NULL,
    AssignedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT FK_UserArea_User FOREIGN KEY (UserId)
        REFERENCES Users(Id),

    CONSTRAINT FK_UserArea_Area FOREIGN KEY (AreaId)
        REFERENCES Areas(Id)
);

-- =========================================
-- TABLA AUDITORÍA
-- =========================================
IF OBJECT_ID('AuditLog', 'U') IS NOT NULL DROP TABLE AuditLog;

CREATE TABLE AuditLog (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    TableName NVARCHAR(100) NOT NULL,
    RecordId INT NOT NULL,
    Operation NVARCHAR(100) NOT NULL,
    OldValue NVARCHAR(MAX) NULL,
    NewValue NVARCHAR(MAX) NULL,
    PerformedBy NVARCHAR(100) NOT NULL,
    PerformedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

-- =========================================
-- INDICES
-- =========================================
CREATE INDEX IX_Users_CreatedAt ON Users (CreatedAt DESC);
CREATE INDEX IX_Users_RoleId ON Users (RoleId);
CREATE INDEX IX_UserArea_AreaId ON UserArea (AreaId);
CREATE INDEX IX_AuditLog_Table_Record ON AuditLog (TableName, RecordId);

-- =========================================
-- DATOS INICIALES
-- =========================================
INSERT INTO Roles (Name)
VALUES ('Admin'), ('User');

INSERT INTO Areas (Name)
VALUES 
('IT'),
('Nomina'),
('Facturacion'),
('Servicio al Cliente');