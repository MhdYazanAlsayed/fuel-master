CREATE OR ALTER Proc SP_Reports
@StationId INT,
@StartDate NVARCHAR(50),
@EndDate NVARCHAR(50)
AS

SELECT 
[Employees].FullName,
[Stations].ArabicName AS StationArabicName, 
[Stations].EnglishName AS StationEnglishName, 
Sum([T].Volume) AS Volume, 
Sum([T].Amount) AS Amount 
FROM [Transactions] AS [T]
INNER JOIN [Stations]
ON [Stations].Id = [T].StationId
INNER JOIN [Employees]
ON [Employees].Id = [T].EmployeeId
WHERE @StationId IS NULL OR [T].StationId = @StationId
GROUP BY 
[Employees].FullName,
[Stations].ArabicName,
[Stations].EnglishName

