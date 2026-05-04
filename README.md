# 💻 User Management System (.NET 8 - WPF)

## 📌 Descripción

Aplicación de escritorio desarrollada en **.NET 8 (WPF)** que permite la gestión de usuarios, incluyendo:

- Creación de usuarios
- Actualización de información
- Asignación de áreas
- Eliminación lógica (Soft Delete)
- Auditoría completa de cambios

El sistema implementa **arquitectura en capas**, uso de **Entity Framework Core**, y **Stored Procedures** para operaciones críticas.

---

## 🏗️ Arquitectura

El proyecto sigue una arquitectura en capas:
```text
src/
│
├── Domain → Entidades del dominio
├── Application → Interfaces y lógica de negocio
├── Infrastructure → Acceso a datos (EF + SP)
└── WPF → Interfaz de usuario (MVVM)
```

### Principios aplicados

- SOLID
- Separation of Concerns
- Dependency Injection
- MVVM (en capa WPF)

---

## 🗄️ Base de Datos

Ubicación de scripts:
database/

### 📌 Orden de ejecución

01. `script-estructura-bd.sql`
02. `script-sp_CreateUser.sql`
03. `script-sp_UpdateUser.sql`
04. `script-sp_AssignArea.sql`
05. `script-sp_DeleteUser.sql`
06. `script-sp_GetAuditHistory.sql`

---

## 🔍 Consulta de Auditoría

Se implementa el SP:
sp_GetAuditHistory


Soporta:

- Filtros dinámicos
- Paginación
- Consulta por usuario, fecha, operación

### Ejemplos de uso:
1. `Consultar todo el historial: EXEC sp_GetAuditHistory;`
2. `Filtrar por tabla: EXEC sp_GetAuditHistory @TableName = 'Users';`
3. `Consultar historial de un registro específico: EXEC sp_GetAuditHistory @TableName = 'Users', @RecordId = 1;`
4. `Filtrar por tipo de operación: EXEC sp_GetAuditHistory @Operation = 'DELETE_USER';`
5. `Filtrar por usuario que ejecutó la acción: EXEC sp_GetAuditHistory @PerformedBy = 'SYSTEM';`
6. `Filtrar por rango de fechas: EXEC sp_GetAuditHistory @DateFrom = '2026-05-01', @DateTo = '2026-05-03';`
7. `Combinar filtros: EXEC sp_GetAuditHistory @TableName = 'Users', @Operation = 'UPDATE_USER', @DateFrom = '2026-05-01';`
8. `Paginación: EXEC sp_GetAuditHistory @PageNumber = 2, @PageSize = 10;`
9. `Consulta completa:
EXEC sp_GetAuditHistory 
    @TableName = 'Users',
    @RecordId = 1,
    @Operation = 'UPDATE_USER',
    @PerformedBy = 'SYSTEM',
    @DateFrom = '2026-05-01',
    @DateTo = '2026-05-10',
    @PageNumber = 1,
    @PageSize = 10;`

---

## 🧠 Decisiones Técnicas

### ✔ Soft Delete
Se utiliza `Status = 0` en lugar de eliminación física para:

- Mantener integridad referencial
- Preservar historial

---

### ✔ Auditoría basada en eventos
Se utilizan eventos de negocio:

- CREATE_USER
- UPDATE_USER
- ASSIGN_AREA
- DELETE_USER

---

### ✔ Uso de Stored Procedures
Para:

- Control de transacciones
- Validaciones en BD
- Auditoría centralizada

---

## 📌 Tecnologías

- .NET 8
- WPF
- Entity Framework Core
- SQL Server
- MVVM

---

## 🚀 Cómo ejecutar

1. Clonar repositorio
2. Ejecutar scripts en SQL Server
3. Configurar cadena de conexión en `App.config` 
4. Ejecutar proyecto WPF
