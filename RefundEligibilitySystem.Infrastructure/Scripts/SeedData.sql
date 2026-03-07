INSERT INTO Budget (TotalAvailableBudget) VALUES (500000.00);

INSERT INTO Citizens (IdentityNumber, FullName) VALUES 
('123456789', N'Israel Cohen'),
('987654321', N'Sara Levi'),
('555666777', N'Avi Green'),
('111222333', N'Miriam Shapiro');

DECLARE @m INT = 1;
WHILE @m <= 12 BEGIN
    INSERT INTO Incomes (CitizenID, IncomeYear, IncomeMonth, Amount) VALUES (1, 2021, @m, 5000.00);
    SET @m = @m + 1;
END

SET @m = 1;
WHILE @m <= 12 BEGIN
    INSERT INTO Incomes (CitizenID, IncomeYear, IncomeMonth, Amount) VALUES (1, 2022, @m, 4800.00);
    SET @m = @m + 1;
END

SET @m = 1;
WHILE @m <= 12 BEGIN
    INSERT INTO Incomes (CitizenID, IncomeYear, IncomeMonth, Amount) VALUES (1, 2025, @m, 5200.00);
    SET @m = @m + 1;
END

INSERT INTO Incomes (CitizenID, IncomeYear, IncomeMonth, Amount) VALUES 
(2, 2022, 1, 7000), (2, 2022, 2, 7000), (2, 2022, 3, 7000);
INSERT INTO Incomes (CitizenID, IncomeYear, IncomeMonth, Amount) VALUES 
(2, 2025, 1, 6500), (2, 2025, 2, 6500), (2, 2025, 3, 6500), (2, 2025, 4, 6500);

SET @m = 1;
WHILE @m <= 12 BEGIN
    INSERT INTO Incomes (CitizenID, IncomeYear, IncomeMonth, Amount) VALUES (3, 2021, @m, 9500.00);
    SET @m = @m + 1;
END

SET @m = 1;
WHILE @m <= 12 BEGIN
    INSERT INTO Incomes (CitizenID, IncomeYear, IncomeMonth, Amount) VALUES (3, 2022, @m, 10000.00);
    SET @m = @m + 1;
END

SET @m = 1;
WHILE @m <= 12 BEGIN
    INSERT INTO Incomes (CitizenID, IncomeYear, IncomeMonth, Amount) VALUES (3, 2025, @m, 9800.00);
    SET @m = @m + 1;
END

SET @m = 1;
WHILE @m <= 12 BEGIN
    INSERT INTO Incomes (CitizenID, IncomeYear, IncomeMonth, Amount) VALUES (4, 2022, @m, 7000.00);
    SET @m = @m + 1;
END
SET @m = 1;
WHILE @m <= 12 BEGIN
    INSERT INTO Incomes (CitizenID, IncomeYear, IncomeMonth, Amount) VALUES (4, 2025, @m, 7200.00);
    SET @m = @m + 1;
END


INSERT INTO Applications (CitizenID, TaxYear, CalculatedRefund, AverageIncome, Status, OfficerDecisionDate) VALUES
(1, 2021, 750.00,  5000.00, 'Approved', '2022-03-15'),
(1, 2022, 720.00,  4800.00, 'Approved', '2023-02-20');
INSERT INTO Applications (CitizenID, TaxYear, Status) VALUES
(1, 2025, 'Pending');

INSERT INTO Applications (CitizenID, TaxYear, CalculatedRefund, AverageIncome, Status, OfficerDecisionDate) VALUES
(2, 2022, 0, 0, 'Rejected', '2023-04-10');
INSERT INTO Applications (CitizenID, TaxYear, Status) VALUES
(2, 2025, 'Pending');

INSERT INTO Applications (CitizenID, TaxYear, CalculatedRefund, AverageIncome, Status, OfficerDecisionDate) VALUES
(3, 2021, 1100.00, 9500.00,  'Approved', '2022-04-01'),
(3, 2022, 1100.00, 10000.00, 'Approved', '2023-03-18');
INSERT INTO Applications (CitizenID, TaxYear, Status) VALUES
(3, 2025, 'Pending');

INSERT INTO Applications (CitizenID, TaxYear, CalculatedRefund, AverageIncome, Status, OfficerDecisionDate) VALUES
(4, 2022, 950.00, 7000.00, 'Approved', '2023-05-12');
INSERT INTO Applications (CitizenID, TaxYear, Status) VALUES
(4, 2025, 'Pending');