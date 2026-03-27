IF DB_ID('TechbodiaNotes') IS NULL
BEGIN
    CREATE DATABASE TechbodiaNotes;
END
GO

USE TechbodiaNotes;
GO

IF OBJECT_ID('dbo.Notes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Notes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserName NVARCHAR(100) NOT NULL,
        Title NVARCHAR(200) NOT NULL,
        Content NVARCHAR(MAX) NULL,
        CreatedAt DATETIME2 NOT NULL,
        UpdatedAt DATETIME2 NOT NULL
    );

    CREATE INDEX IX_Notes_UserName_CreatedAt
        ON dbo.Notes (UserName, CreatedAt DESC);
END
GO
