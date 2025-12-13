 
CREATE OR ALTER Proc SP_Reports
@StationId INT,
@StartDate NVARCHAR(50),
@EndDate NVARCHAR(50)
AS

SELECT 
[Stations].ArabicName, 
[Stations].EnglishName, 
[T].PaymentMethod,
COUNT([T].PaymentMethod) AS TransactionCount,
Sum([T].Volume) AS Volume, 
Sum([T].Amount) AS Amount 
FROM [Transactions] AS [T]
INNER JOIN [Stations]
ON [Stations].Id = [T].StationId
WHERE @StationId IS NULL OR [T].StationId = @StationId
GROUP BY 
[T].PaymentMethod,
[Stations].ArabicName,
[Stations].EnglishName
ORDER BY ArabicName,EnglishName