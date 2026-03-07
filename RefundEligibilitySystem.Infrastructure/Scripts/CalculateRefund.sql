CREATE PROCEDURE sp_CalculateRefund
    @ApplicationId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @CitizenId    INT;
    DECLARE @TaxYear      INT;
    DECLARE @MonthCount   INT;
    DECLARE @AvgIncome    DECIMAL(18,2);
    DECLARE @RefundAmount DECIMAL(18,2) = 0;

    SELECT @CitizenId = CitizenId, @TaxYear = TaxYear
    FROM Applications
    WHERE ApplicationId = @ApplicationId AND Status = 'Pending';

    IF @CitizenId IS NULL
        THROW 50000, 'Application not found or not in Pending status.', 1;

    IF EXISTS (
        SELECT 1 FROM Applications
        WHERE CitizenId = @CitizenId
          AND TaxYear   = @TaxYear
          AND Status    = 'Approved'
    )
        THROW 50001, 'Citizen already has an approved application for this tax year.', 1;

    SELECT
        @MonthCount = COUNT(*),
        @AvgIncome  = AVG(Amount)
    FROM Incomes
    WHERE CitizenId  = @CitizenId
      AND IncomeYear = @TaxYear;

    IF @MonthCount < 6
    BEGIN
        UPDATE Applications
        SET [CalculatedRefund] = 0,
            [AverageIncome]    = @AvgIncome,
            [Status]           = 'Calculated'
        WHERE ApplicationId = @ApplicationId;

        SELECT 0 AS CalculatedRefund;
        THROW 50002, 'Fewer than 6 income months recorded for this tax year.', 1;
    END
    

    IF @AvgIncome > 0
    BEGIN
        DECLARE @Tier1 DECIMAL(18,2) = CASE WHEN @AvgIncome > 5000 THEN 5000 ELSE @AvgIncome END;
        SET @RefundAmount = @Tier1 * 0.15;
    END

    IF @AvgIncome > 5000
    BEGIN
        DECLARE @Tier2 DECIMAL(18,2) = CASE WHEN @AvgIncome > 8000 THEN 3000 ELSE (@AvgIncome - 5000) END;
        SET @RefundAmount = @RefundAmount + (@Tier2 * 0.10);
    END

    IF @AvgIncome > 8000
    BEGIN
        DECLARE @Tier3 DECIMAL(18,2) = CASE WHEN @AvgIncome > 9000 THEN 1000 ELSE (@AvgIncome - 8000) END;
        SET @RefundAmount = @RefundAmount + (@Tier3 * 0.05);
    END

    UPDATE Applications
    SET [CalculatedRefund] = @RefundAmount,
        [AverageIncome]    = @AvgIncome,
        [Status]           = 'Calculated'
    WHERE ApplicationId = @ApplicationId;

    SELECT @RefundAmount AS CalculatedRefund;
END