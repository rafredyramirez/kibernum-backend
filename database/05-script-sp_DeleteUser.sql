CREATE OR ALTER PROCEDURE sp_DeleteUser
    @UserId INT,
    @PerformedBy NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validar existencia del usuario
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
        BEGIN
            THROW 50020, 'Usuario no existe', 1;
        END

        DECLARE @OldData NVARCHAR(MAX);
        DECLARE @NewData NVARCHAR(MAX);

        SELECT @OldData = (
            SELECT *
            FROM Users
            WHERE Id = @UserId
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        );

        UPDATE Users
        SET Status = 0,
            UpdatedAt = SYSDATETIME()
        WHERE Id = @UserId;

        SELECT @NewData = (
            SELECT *
            FROM Users
            WHERE Id = @UserId
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
            'Users',
            @UserId,
            'DELETE_USER',
            @OldData,
            @NewData,
            @PerformedBy
        );

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END