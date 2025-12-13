CREATE OR ALTER Proc SP_Reports
@StationId INT,
@StartDate NVARCHAR(50),
@EndDate NVARCHAR(50)
AS

SELECT 
[Stations].ArabicName AS StationArabicName , 
[Stations].EnglishName AS StationEnglishName, 
[Nozzles].Number, 
[Tanks].FuelType, 
Sum([T].Volume) AS Volume, 
Sum([T].Amount) AS Amount 
FROM [Transactions] AS [T]
INNER JOIN [Nozzles] 
ON [Nozzles].Id = NozzleId
INNER JOIN [Tanks]
ON [Tanks].Id = [Nozzles].TankId
INNER JOIN [Stations]
ON [Stations].Id = [Tanks].StationId
WHERE @StationId IS NULL OR [T].StationId = @StationId
GROUP BY 
[Stations].ArabicName,
[Stations].EnglishName,
[Nozzles].Number,
[Tanks].FuelType