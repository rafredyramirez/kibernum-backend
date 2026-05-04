CREATE OR ALTER PROCEDURE sp_CreateUser  
    @Name NVARCHAR(100),  
    @ContactInfo NVARCHAR(200),  
    @RoleId INT,  
    @PerformedBy NVARCHAR(100)  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    BEGIN TRY  
        BEGIN TRANSACTION;  

		IF EXISTS (
			SELECT 1 
			FROM Users 
			WHERE Name = @Name AND ContactInfo = @ContactInfo
		)
		BEGIN
			THROW 50010, 'El usuario ya existe', 1;
		END
  
        INSERT INTO Users (Name, ContactInfo, RoleId)  
        VALUES (@Name, @ContactInfo, @RoleId);  
  
        DECLARE @UserId INT = SCOPE_IDENTITY();  
  
        DECLARE @NewData NVARCHAR(MAX);  
  
        SELECT @NewData = (  
            SELECT @Name AS Name, @ContactInfo AS ContactInfo, @RoleId AS RoleId  
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
            'Users',  
            @UserId,  
            'CREATE_USER',  
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