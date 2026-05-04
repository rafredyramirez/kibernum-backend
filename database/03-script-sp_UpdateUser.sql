CREATE OR ALTER PROCEDURE sp_UpdateUser
    @UserId INT,
    @Name NVARCHAR(100),
    @ContactInfo NVARCHAR(200),
    @RoleId INT,
    @PerformedBy NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @OldData NVARCHAR(MAX);

        SELECT @OldData = (
            SELECT Name, ContactInfo, RoleId
            FROM Users
            WHERE Id = @UserId
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        );

        IF @OldData IS NULL
        BEGIN
            THROW 50001, 'Usuario no existe', 1;
        END

        UPDATE Users
        SET 
            Name = @Name,
            ContactInfo = @ContactInfo,
            RoleId = @RoleId,
            UpdatedAt = SYSDATETIME()
        WHERE Id = @UserId;

        DECLARE @NewData NVARCHAR(MAX);

        SELECT @NewData = (
            SELECT @Name AS Name, @ContactInfo AS ContactInfo, @RoleId AS RoleId
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
            'UPDATE_USER',
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