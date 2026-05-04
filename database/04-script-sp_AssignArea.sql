CREATE OR ALTER PROCEDURE sp_AssignArea
    @UserId INT,
    @AreaId INT,
    @PerformedBy NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validar usuario
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
        BEGIN
            THROW 50002, 'Usuario no existe', 1;
        END

        -- Validar área
        IF NOT EXISTS (SELECT 1 FROM Areas WHERE Id = @AreaId)
        BEGIN
            THROW 50003, 'Área no existe', 1;
        END

        DECLARE @OldData NVARCHAR(MAX);
        DECLARE @NewData NVARCHAR(MAX);

        IF EXISTS (SELECT 1 FROM UserArea WHERE UserId = @UserId)
        BEGIN
            SELECT @OldData = (
                SELECT 
                    ua.UserId,
                    ua.AreaId,
                    a.Name AS AreaName
                FROM UserArea ua
                LEFT JOIN Areas a ON ua.AreaId = a.Id
                WHERE ua.UserId = @UserId
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            );

            -- UPDATE
            UPDATE UserArea
            SET AreaId = @AreaId,
                AssignedAt = SYSDATETIME()
            WHERE UserId = @UserId;

            -- NEW (estado real después del cambio)
            SELECT @NewData = (
                SELECT 
                    ua.UserId,
                    ua.AreaId,
                    a.Name AS AreaName
                FROM UserArea ua
                LEFT JOIN Areas a ON ua.AreaId = a.Id
                WHERE ua.UserId = @UserId
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            );

            INSERT INTO AuditLog (
                TableName,
                RecordId,
                Operation,
                OldValue,
                NewValue,
                PerformedBy
            )
            VALUES (
                'UserArea',
                @UserId,
                'ASSIGN_AREA',
                @OldData,
                @NewData,
                @PerformedBy
            );
        END
        ELSE
        BEGIN
            INSERT INTO UserArea (UserId, AreaId)
            VALUES (@UserId, @AreaId);

            SELECT @NewData = (
                SELECT 
                    ua.UserId,
                    ua.AreaId,
                    a.Name AS AreaName
                FROM UserArea ua
                LEFT JOIN Areas a ON ua.AreaId = a.Id
                WHERE ua.UserId = @UserId
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            );

            INSERT INTO AuditLog (
                TableName,
                RecordId,
                Operation,
                NewValue,
                PerformedBy
            )
            VALUES (
                'UserArea',
                @UserId,
                'ASSIGN_AREA',
                @NewData,
                @PerformedBy
            );
        END

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END