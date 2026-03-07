IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Citizens')
BEGIN
    CREATE TABLE Citizens (
        CitizenId INT IDENTITY(1,1) PRIMARY KEY,
        IdentityNumber NVARCHAR(9) NOT NULL UNIQUE,
        FullName NVARCHAR(100) NOT NULL,
        CreatedAt DATETIME DEFAULT GETDATE()
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Incomes')
BEGIN
    CREATE TABLE Incomes (
        IncomeId INT IDENTITY(1,1) PRIMARY KEY,
        CitizenId INT NOT NULL,
        IncomeYear INT NOT NULL,
        IncomeMonth INT NOT NULL CHECK (IncomeMonth BETWEEN 1 AND 12),
        Amount DECIMAL(18, 2) NOT NULL DEFAULT 0,
        CreatedAt DATETIME DEFAULT GETDATE(),
        
        CONSTRAINT UQ_Citizen_Month UNIQUE (CitizenId, IncomeYear, IncomeMonth),
        FOREIGN KEY (CitizenId) REFERENCES Citizens(CitizenId)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Applications')
BEGIN
    CREATE TABLE Applications (
        ApplicationId INT IDENTITY(1,1) PRIMARY KEY,
        CitizenId INT NOT NULL,
        TaxYear INT NOT NULL,
        CalculatedRefund DECIMAL(18, 2) DEFAULT 0,
        AverageIncome DECIMAL(18, 2) DEFAULT 0,
        Status NVARCHAR(20) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Approved', 'Rejected', 'Calculated')),
        OfficerDecisionDate DATETIME NULL,
        CreatedAt DATETIME DEFAULT GETDATE(),
        
        CONSTRAINT UQ_Citizen_TaxYear UNIQUE (CitizenId, TaxYear),
        FOREIGN KEY (CitizenId) REFERENCES Citizens(CitizenId)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Budget')
BEGIN
    CREATE TABLE Budget (
        BudgetID INT PRIMARY KEY DEFAULT 1,
        TotalAvailableBudget DECIMAL(18, 2) NOT NULL,
        UpdatedAt DATETIME DEFAULT GETDATE(),
        CONSTRAINT CK_OnlyOneRow CHECK (BudgetID = 1)
    );
END