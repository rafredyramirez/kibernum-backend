CREATE OR ALTER PROCEDURE sp_GetAuditHistory
    @TableName NVARCHAR(100) = NULL,
    @RecordId INT = NULL,
    @Operation NVARCHAR(100) = NULL,
    @PerformedBy NVARCHAR(100) = NULL,
    @DateFrom DATETIME2 = NULL,
    @DateTo DATETIME2 = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF @PageNumber <= 0 SET @PageNumber = 1;
        IF @PageSize <= 0 SET @PageSize = 50;

        DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

        SELECT 
            Id,
            TableName,
            RecordId,
            Operation,
            OldValue,
            NewValue,
            PerformedBy,
            PerformedAt
        FROM AuditLog
        WHERE 
            (@TableName IS NULL OR TableName = @TableName)
            AND (@RecordId IS NULL OR RecordId = @RecordId)
            AND (@Operation IS NULL OR Operation = @Operation)
            AND (@PerformedBy IS NULL OR PerformedBy = @PerformedBy)
            AND (@DateFrom IS NULL OR PerformedAt >= @DateFrom)
            AND (@DateTo IS NULL OR PerformedAt <= @DateTo)
        ORDER BY PerformedAt DESC
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY;

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END