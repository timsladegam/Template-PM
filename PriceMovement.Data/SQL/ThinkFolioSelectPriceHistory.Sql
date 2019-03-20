WITH prices AS 
(
	SELECT		p.[sSecurityId]
				, p.[dtPricePoint]
				, p.[sSource]
				, p.[sPriceCurrency]
				, p.[dfMidPrice]
				, d.[dfExternalPriceFactor]
				, ROW_NUMBER() OVER (PARTITION BY [sSecurityId], CONVERT(date, p.[dtPricePoint]) ORDER BY p.[dtPricePoint] DESC) AS RowNum
	FROM		[Thinkfolio_GAM].[dbo].[AssetPrices] p
	INNER JOIN	[AssetDetails] d ON p.[sSecurityId] = d.[sSecId]
	WHERE		p.[iType] = 0
	AND			[sSecurityId] = @SecurityId
	AND			p.[dtPricePoint] >= @FromDate AND p.[dtPricePoint] < DATEADD(d, 1, @ToDate)
)

SELECT		[sSecurityId]
			, [dtPricePoint]
			, [sSource]
			, [sPriceCurrency]
			, [dfMidPrice]
			, [dfExternalPriceFactor]
			, RowNum
FROM		prices
WHERE		RowNum = 1
ORDER BY	[dtPricePoint] DESC