WITH prices AS 
(
	SELECT		p.[sSecurityId]
				, p.[dtPricePoint]
				, p.[sSource]
				, p.[sPriceCurrency]
				, p.[dfMidPrice]
				, ROW_NUMBER() OVER (PARTITION BY p.[sSecurityId], CONVERT(date, p.[dtPricePoint]) ORDER BY p.[dtPricePoint] DESC) AS RowNum
	FROM		[Thinkfolio_GAM].[dbo].[AssetPrices] p
	WHERE		p.[iType] = 0
	AND			p.[dtPricePoint] < DATEADD(d, 1, @PriceDate)
),

top2 as
(
	SELECT		p.[sSecurityId]
				, p.[dtPricePoint]
				, p.[sSource]
				, p.[sPriceCurrency]
				, p.[dfMidPrice]
				, ROW_NUMBER() OVER (PARTITION BY p.[sSecurityId] ORDER BY p.[dtPricePoint] DESC) AS RowNum2
	FROM		prices p
	WHERE		P.RowNum = 1
)

SELECT		t.[sSecurityId]
			, t.[dtPricePoint]
			, t.[sSource]
			, t.[sPriceCurrency]
			, t.[dfMidPrice]
			, d.[dfExternalPriceFactor]
			, t.RowNum2 AS RowNum
FROM		top2 t
INNER JOIN	[AssetDetails] d ON t.[sSecurityId] = d.[sSecId]
WHERE		t.RowNum2 <= 2
ORDER BY	t.[sSecurityId], t.[dtPricePoint] DESC
