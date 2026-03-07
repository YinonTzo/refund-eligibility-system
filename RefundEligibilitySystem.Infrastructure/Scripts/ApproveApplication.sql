CREATE PROCEDURE sp_ApproveApplication
    @ApplicationId INT,
    @IsApproved    BIT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;
    BEGIN TRY

        DECLARE @CurrentBudget DECIMAL(18,2);
        SELECT @CurrentBudget = TotalAvailableBudget
        FROM Budget WITH (UPDLOCK, ROWLOCK)
        WHERE BudgetId = 1;

        DECLARE @RequiredAmount DECIMAL(18,2);
        SELECT @RequiredAmount = CalculatedRefund
        FROM Applications WITH (UPDLOCK, ROWLOCK)
        WHERE ApplicationId = @ApplicationId AND Status = 'Calculated';

        IF @RequiredAmount IS NULL
            THROW 50003, 'Application not found or not in Calculated status.', 1;

        IF @IsApproved = 1
        BEGIN
            IF @CurrentBudget < @RequiredAmount
                THROW 50004, 'Insufficient budget for this refund.', 1;

            UPDATE Budget
            SET TotalAvailableBudget = TotalAvailableBudget - @RequiredAmount
            WHERE BudgetId = 1;

            UPDATE Applications
            SET [Status]             = 'Approved',
                OfficerDecisionDate = GETDATE()
            WHERE ApplicationId = @ApplicationId;
        END
        ELSE
        BEGIN
            UPDATE Applications
            SET Status             = 'Rejected',
                OfficerDecisionDate = GETDATE()
            WHERE ApplicationId = @ApplicationId;
        END

        COMMIT TRANSACTION;
        SELECT 'Success' AS Result;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END