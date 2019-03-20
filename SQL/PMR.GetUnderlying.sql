USE [FIND]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tim Slade
-- Create date: January 2019
-- Description:	PriceMovement store procedure. Based on existing SSRS sql. Used in new dynamic PM report.
-- =============================================
ALTER PROCEDURE [PMR].[GetUnderlying]
	@priceDate DATETIME
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- UNDERLYING
DECLARE @ReportHeaderId INT;
DECLARE @PricingDate DATETIME;
DECLARE @PriceDateMinusOne DATETIME;

SELECT TOP 1
	@ReportHeaderId = ReportHeaderId
	, @PricingDate = PricingDate
	, @PriceDateMinusOne = PricingDateMinusOne
FROM 
	[PMR].[ReportHeader] 
WHERE Message = 'Completed' 
	AND pricingdate <= @priceDate
ORDER BY 
	ReportHeaderId DESC	;

-- SELECT @ReportHeaderId, @PricingDate, @PriceDateMinusOne
WITH underlyingInst AS
(
	SELECT 
		DISTINCT i.INSTRUMENT_ID
				, pch.[Security Id]
				, pch.[Linked Security]
	FROM PMR.PriceComparisonHolding_archive pch
	INNER JOIN INST_IDENTIFIERS i 
		ON pch.[Linked Security] = i.IDENTIFIER_VALUE 
			AND i.IDENTIFIER_NAME = 'THINKFOLIO'
	WHERE
		ReportHeaderId = @ReportHeaderId
		AND pch.[Linked Security] IS NOT NULL
		AND pch.[Asset Class] IN ( 'CFD','TRS' )
		AND  pch.[SourceT-1] IS NULL
)

-- select * from underlyingInst 

, underlyingCfdTrs AS
(
	SELECT
		'CFD' AS securityType
		, UNDERLYING_INSTRUMENT_ID 
	FROM INST_CFD u 
	INNER JOIN underlyingInst i 
		ON i.INSTRUMENT_ID = u.UNDERLYING_INSTRUMENT_ID
	UNION
	SELECT
		'TSR' AS securityType
		, UNDERLYING_INSTRUMENT_ID 
	FROM INST_TOTAL_RETURN_SWAP u 
	INNER JOIN underlyingInst i 
		ON i.INSTRUMENT_ID = u.UNDERLYING_INSTRUMENT_ID
)

--  select * from underlyingCfdTrs -- order by [Security Id]

, prices AS
(
	SELECT
		a.INSTRUMENT_ID
		, i.IDENTIFIER_VALUE
		, VALUE AS Price
		, date AS PriceDate
		, ROW_NUMBER() OVER (PARTITION BY a.instrument_id ORDER BY date DESC) AS T
	FROM INST_ANALYTICS a
	JOIN underlyingCfdTrs u 
		ON u.UNDERLYING_INSTRUMENT_ID = a.INSTRUMENT_ID 
			AND SOURCE = 'THINKFOLIO' 
			AND NAME = 'CURRENT_PRICE' 
			AND DATE = @PriceDateMinusOne 
	JOIN INST_IDENTIFIERS i 
		ON i.INSTRUMENT_ID = a.INSTRUMENT_ID 
			AND i.IDENTIFIER_NAME = 'THINKFOLIO'
)

--  select * from prices 

, withMaxDateAndParentSecurity AS 
(
	SELECT
		pch.Portfolio
		, pch.[Security Id] AS [Security Id]
		, p.[IDENTIFIER_VALUE] AS [Linked Security] 
		, pch.[Price Date]
		, p.Price AS CurrentPrice
		, pch.[Price Currency]
		, DATEDIFF(dd, pch.[Price Date], @PriceDateMinusOne)
			-(DATEDIFF(wk, pch.[Price Date], @PriceDateMinusOne) * 2)
			-(CASE WHEN DATENAME(dw, pch.[Price Date]) = 'Sunday' THEN 1 ELSE 0 END)
			-(CASE WHEN DATENAME(dw, @PriceDateMinusOne) = 'Saturday' THEN 1 ELSE 0 END) AS 'Business Days Stale'
	FROM
		prices p 
	INNER JOIN
		PMR.PriceComparisonHolding_archive pch 
			ON pch.[Linked Security] = p.[IDENTIFIER_VALUE] 			
	WHERE
		ReportHeaderId = @ReportHeaderId
		AND pch.[Linked Security] IS NOT NULL
		AND pch.[Asset Class] IN ( 'CFD','TRS' )
		AND  pch.[SourceT-1] IS NULL
)

, hierarchy AS
	(
	   SELECT *,
			 ROW_NUMBER() OVER (PARTITION BY Code ORDER BY PortfolioHierarchy_archiveId DESC) AS rn
	   FROM [PMR].PortfolioHierarchy_archive
	   WHERE ReportHeaderId = @ReportHeaderId
	)
	, hierarchyToday AS
	(
	   SELECT *			 
	   FROM hierarchy
	   WHERE rn = 2
	)

, UltimateParentHierarchy1 AS 
(
	SELECT 
		[Code]
		, [Parent Portfolio]
		, [TotalValuationParentLevel]  
	FROM 
		hierarchyToday
	WHERE 
		[Code] = [Parent Portfolio] 

	UNION ALL
   
	SELECT 
		c.[Code]
		, p.[Parent Portfolio]
		, p.[TotalValuationParentLevel]
	FROM 
		hierarchyToday c
	JOIN UltimateParentHierarchy1 p 
		ON p.[Code] = c.[Parent Portfolio]  -- this is the recursion 
	WHERE c.[Code] <> c.[Parent Portfolio] 
) 

--select * from UltimateParentHierarchy1

, ultimateParentValuation as 
(
SELECT 
	[Code]
	, UltimateParentCode
	, [TotalValuationUltimateParentLevel]
FROM (
	--Extract ultimate parent TotalValuation
	SELECT 
		[Code],
		[Parent Portfolio] AS UltimateParentCode,
		[TotalValuationParentLevel] AS [TotalValuationUltimateParentLevel]
	FROM 
		UltimateParentHierarchy1

	UNION ALL

	--Some child doen't have the parent included in the Hierarchy file
	--Therefore we would need to include these seperately
	SELECT 
		MissingUltimateParents.[Code],
		MissingUltimateParents.[Parent Portfolio] AS UltimateParentCode,
		MissingUltimateParents.[TotalValuationParentLevel] AS [TotalValuationUltimateParentLevel]
	FROM 
		hierarchy MissingUltimateParents
	LEFT JOIN UltimateParentHierarchy1
		ON MissingUltimateParents.Code = UltimateParentHierarchy1.Code
	WHERE 
		UltimateParentHierarchy1.Code IS NULL
	) ultimateParentValuation
)

--SELECT * FROM ultimateParentValuation order by code;

, removeDead as 
(
	SELECT i.*, h.HLDG_DATE, h.HLDG_QTY, h.SECURITY_ID, h.MATURITY_DATE
	FROM withMaxDateAndParentSecurity i 
	LEFT JOIN dbo.HOLDINGS_BREAKDOWN h
		ON i.[Security Id] = h.SECURITY_ID 
			AND h.SOURCE = 'THINKFOLIO' 
			AND h.HLDG_DATE = @PriceDateMinusOne
	 WHERE h.SECURITY_ID IS NOT NULL
)

-- SELECT * FROM removeDead

SELECT
	Portfolio
	, [Security Id]
	, [Linked Security] 
	, [Price Currency]
	, [Price Date]
	, [CurrentPrice]
	, [Business Days Stale]
	, HLDG_DATE
	, HLDG_QTY
	, SECURITY_ID
	, MATURITY_DATE
	, [Portfolio NAV (Custom)] = u.[TotalValuationUltimateParentLevel]	
	, CASE WHEN [Business Days Stale] > 5 THEN 'Stale Price - Over 5 Days' END AS Stale
FROM removeDead underlying
LEFT JOIN ultimateParentValuation u 
	ON u.Code = underlying.Portfolio
ORDER BY [Linked Security]

END
