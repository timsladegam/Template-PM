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
ALTER PROCEDURE [PMR].[GetPriceMovementsNoPrices]
	@priceDate DATETIME
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- declare @paramDate DATETIME = '18 jan 2019'

	-- BOND PRICE MOVEMENTS
	IF OBJECT_ID('tempdb..#BondPriceMovementAsPercentOfNav') IS NOT NULL
	BEGIN
		DROP TABLE #BondPriceMovementAsPercentOfNav
	END

	IF OBJECT_ID('tempdb..#Holdings1d_tmp') IS NOT NULL
	BEGIN
		DROP TABLE #Holdings1d_tmp
	END

	IF OBJECT_ID('tempdb..#ultimateParentValuation1') IS NOT NULL
	BEGIN
		DROP TABLE #ultimateParentValuation1
	END

	IF OBJECT_ID('tempdb..#ultimateParentValuation2') IS NOT NULL
	BEGIN
		DROP TABLE #ultimateParentValuation2
	END

	IF OBJECT_ID('tempdb..#NAV1dTemp') IS NOT NULL
	BEGIN
		DROP TABLE #NAV1dTemp
	END

	DECLARE @ReportHeaderId INT;
	DECLARE @PriceDateMinusOne DATETIME;
	
	SELECT TOP 1
		@ReportHeaderId = ReportHeaderId
		, @PriceDateMinusOne = PricingDateMinusOne
	FROM 
		[PMR].[ReportHeader] 
	WHERE Message = 'Completed' 
		AND pricingdate <= @priceDate 
	ORDER BY 
		ReportHeaderId DESC

	-- A: Load from PriceComparisonHolding_holding for T
	SELECT 
		@ReportHeaderId AS [ReportHeaderId]
		, price.[iRow]
		, price.[Portfolio]
		, price.[Portfolio NAV (Custom)] AS [Portfolio NAV]
		, price.[Security Id]
		, price.[Nominal]
		, price.[Description]
		, price.[Asset Type]
		, price.[Asset Sub-type]
		, price.[Asset Class]
		, price.[Asset Sub-class]
		, price.[Leg Type]
		, price.Currency
		, price.[Price Currency]
		, price.[Current Factor]
		, price.[Latest Factor]
		, price.[Factor -1d (1 days ago)]
		, price.[Factor Change %]
		, price.[%]
		, price.[Total Value]
		, price.[Total Value (Portfolio)]
		, price.[Total Value (USD)]
		, price.Accrued
		, price.[Accrued (Portfolio)]
		, price.[Accrued (USD)]
		, price.[Trade Ref]
		, price.ISIN
		, price.[Indexed Against]
		, price.[Linked Security]
		, price.[Linked Security Desc]
		, price.[Linked Equity]
		, CONVERT( DECIMAL(25,10), NULL) AS [Total Value (USD) -1d Actual]	-- from prev day [Total Value (USD)]
		, CONVERT( DECIMAL(25,10), NULL) AS [Total Value (USD) Change %]	-- = ([Total Value (USD)] - prev day [Total Value (USD)]) / [Total Value (USD)]
		, CONVERT( DECIMAL(25,10), NULL) AS [Share of NAV]					-- = [Total Value (USD)] / [Portfolio NAV (Custom)]
		, CONVERT( DECIMAL(25,10), NULL) AS [Portfolio NAV -1d]				-- from prev day [Portfolio NAV (Custom)]
		, CONVERT( DECIMAL(30,5), NULL) AS [Portfolio NAV (Custom)]			-- = #ultimateParentValuation1.[TotalValuationUltimateParentLevel]	
		, CONVERT( DECIMAL(30,5), NULL) AS [Portfolio NAV (Custom)(T-1)]	-- = #ultimateParentValuation2.[TotalValuationUltimateParentLevel] 
		, CONVERT( VARCHAR(60), NULL) AS [Dealing Desk]						-- from Risk Pos data
		, CASE WHEN [Asset Class] IN ( 'BOND','COMMODITY','CONVERTIBLE','EQUITY','ETF','FUND','INDEX','WARRANT') THEN 1
				WHEN [Asset Sub-class] IN ( 'CURVE CAP','CURVE FLOOR') Then 3
				WHEN [Asset Class] IN ( 'BOND FUTURE','BOND OPTION','COMMODITY FUTURE','INDEX FUTURE','INDEX OPTION','RATE FUTURE','RATE OPTION' ) THEN 2
				WHEN [Asset Class] IN ( 'ASCOT','BASIS SWAP','CDS','CDX','CFD','COMMODITY OPTION','CONVERTIBLE','FRA','FX OPTION','INFLATION SWAP','IRS','IRS X CCY','OIS','STRUCTURED NOTE','SWAPTION IRS','TRS','VARIANCE SWAP' ) THEN 3
				WHEN [Asset Sub-type] IN ( 'Default Swap' ) AND [Leg Type] = 'Primary' Then 4
				ELSE 0 END AS [Report Type]
		, CONVERT( VARCHAR(60), NULL ) AS [Stale]
		, CONVERT( VARCHAR(60), NULL) AS [Exception]
	INTO	#BondPriceMovementAsPercentOfNav	
	FROM	[PMR].PriceComparisonHolding_archive price
	WHERE	ReportHeaderId = @ReportHeaderId
	AND		[SourceT-1] IS NULL
	AND (
		( [Asset Class] IN ( 'BOND','BOND FUTURE','BOND OPTION','COMMODITY','COMMODITY FUTURE','CONVERTIBLE','EQUITY','ETF','FUND','INDEX','INDEX FUTURE','INDEX OPTION','RATE FUTURE','RATE OPTION','SWAPTION IRS','WARRANT' ) AND [Leg Type] = 'Normal' )
		OR
		( [Asset Class] IN ( 'ASCOT','BASIS SWAP','CDS','CDX','CFD','COMMODITY OPTION','CONVERTIBLE','FRA','FX OPTION','Inflation SWAP','IRS','IRS X CCY','OIS','STRUCTURED NOTE','TRS','VARIANCE SWAP' ) AND [Leg Type] != 'Exposure' )
	);

	
	-- SELECT * FROM #BondPriceMovementAsPercentOfNav;

	 -- B: Load from PriceComparisonHolding_holding for T-1
	SELECT 
		holdings.Portfolio
		, holdings.[Security Id]
		, holdings.[Total Value (USD)]
		, holdings.[Trade Ref]
		, holdings.[Leg Type]
	INTO #Holdings1d_tmp
	FROM [PMR].PriceComparisonHolding_archive holdings
	WHERE	ReportHeaderId = @ReportHeaderId
	AND		[SourceT-1] IS NOT NULL
	AND (
		( [Asset Class] IN ( 'BOND','BOND FUTURE','BOND OPTION','COMMODITY','COMMODITY FUTURE','CONVERTIBLE','EQUITY','ETF','FUND','INDEX','INDEX FUTURE','INDEX OPTION','RATE FUTURE','RATE OPTION','SWAPTION CREDIT','SWAPTION IRS','WARRANT' ) AND [Leg Type] = 'Normal' )
		OR
		( [Asset Class] IN ( 'ASCOT','CDS','CDX','CFD','FX OPTION','IRS','TRS' ) AND [Leg Type] != 'Exposure' )
	);

	-- SELECT * FROM #Holdings1d_tmp;

	-- B: 2 for T-1
	SELECT 
		DISTINCT [Portfolio], [Portfolio NAV (Custom)]
	INTO #NAV1dTemp
	FROM [PMR].PriceComparisonHolding_archive
	WHERE ReportHeaderId = @ReportHeaderId
		AND [SourceT-1] IS NOT NULL;

	 -- C: Remove portfolios using exclusion list ( i.e PMR.ExcludePortfolioColumn )
	DELETE BondPrice
	FROM #BondPriceMovementAsPercentOfNav BondPrice
	INNER JOIN [PMR].ExcludePortfolioColumn exList 
		ON exList.ExcludePortfolio = BondPrice.Portfolio
	WHERE BondPrice.ReportHeaderID = @ReportHeaderId;

	-- SELECT * FROM #BondPriceMovementAsPercentOfNav;

	-- D: Remove Zero Portfolio NAVs
	DELETE #BondPriceMovementAsPercentOfNav
	WHERE [Portfolio NAV] <= 0
		AND ReportHeaderId = @ReportHeaderId;

	-- SELECT * FROM #BondPriceMovementAsPercentOfNav;	

	-- E: Remove Zero Nominals
	DELETE #BondPriceMovementAsPercentOfNav
	WHERE Nominal = 0
		AND ReportHeaderId = @ReportHeaderId;

	-- SELECT * FROM #BondPriceMovementAsPercentOfNav;	

	-- F: Remove Dead Securities
	DELETE BondPrice
	FROM #BondPriceMovementAsPercentOfNav BondPrice
	LEFT JOIN [PMR].TFUniverseADNew_Archive Instruments 
		ON BondPrice.[Security Id] = Instruments.Identifier 
			AND BondPrice.ReportHeaderId = Instruments.ReportHeaderId
			AND Instruments.[Is T-1] = 0
	WHERE Instruments.iRow IS NULL
		AND BondPrice.ReportHeaderId = @ReportHeaderId;

	-- SELECT * FROM #BondPriceMovementAsPercentOfNav;	

	-- G: Update Portfolio NAV (Custom) with Parent Value
	WITH hierarchy AS
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

	SELECT 
		[Code]
		, UltimateParentCode
		, [TotalValuationUltimateParentLevel]
	INTO #ultimateParentValuation1 
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
			hierarchyToday MissingUltimateParents
		LEFT JOIN UltimateParentHierarchy1
			ON MissingUltimateParents.Code = UltimateParentHierarchy1.Code
		WHERE 
			UltimateParentHierarchy1.Code IS NULL
	) ultimateParentValuation;

	-- SELECT * FROM #ultimateParentValuation;

	---- Update the [Portfolio NAV (Custom)] ultimate parent Valuation
	UPDATE BondPrice
	SET BondPrice.[Portfolio NAV (Custom)] = u.[TotalValuationUltimateParentLevel]	 
	FROM #BondPriceMovementAsPercentOfNav BondPrice
	LEFT JOIN #ultimateParentValuation1 u 
		ON u.Code = BondPrice.Portfolio
	WHERE ReportHeaderId = @ReportHeaderId;

	-- SELECT * FROM #BondPriceMovementAsPercentOfNav; 

	----Delete PMR.BondPriceMovementAsPercentOfNAV where no active instument is associated to them
	DELETE #BondPriceMovementAsPercentOfNav
	WHERE [Portfolio NAV (Custom)] IS NULL
		AND ReportHeaderId = @ReportHeaderId ;

	-- SELECT * FROM #BondPriceMovementAsPercentOfNav; 

	-- H: Update Portfolio NAV (Custom) (T-1) with Parent Value
	WITH hierarchy AS
	(
	   SELECT *,
			 ROW_NUMBER() OVER (PARTITION BY Code ORDER BY PortfolioHierarchy_archiveId DESC) AS rn
	   FROM [PMR].PortfolioHierarchy_archive
	   where ReportHeaderId = @ReportHeaderId
	),
	hierarchyToday AS
	(
	   SELECT *			 
	   FROM hierarchy
	   WHERE rn = 1
	), 
	UltimateParentHierarchy2 AS 
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
		JOIN UltimateParentHierarchy2  p 
			ON p.[Code] = c.[Parent Portfolio]  -- this is the recursion 
		WHERE 
			c.[Code] <> c.[Parent Portfolio] 
	) 

	SELECT 
		[Code]
		, UltimateParentCode
		, [TotalValuationUltimateParentLevel]
	INTO #ultimateParentValuation2
	FROM (
		--Extract ultimate parent TotalValuation
		SELECT 
			[Code]
			, [Parent Portfolio] AS UltimateParentCode
			, [TotalValuationParentLevel] AS [TotalValuationUltimateParentLevel]
		FROM 
			UltimateParentHierarchy2

		UNION ALL

		--Some child doen't have the parent included in the Hierarchy file
		--Therefore we would need to include these seperately
		SELECT 
			MissingUltimateParents.[Code]
			, MissingUltimateParents.[Parent Portfolio] AS UltimateParentCode
			, MissingUltimateParents.[TotalValuationParentLevel] AS [TotalValuationUltimateParentLevel]
		FROM 
			hierarchyToday MissingUltimateParents
		LEFT JOIN UltimateParentHierarchy2
			ON MissingUltimateParents.Code = UltimateParentHierarchy2.Code
		WHERE UltimateParentHierarchy2.Code IS NULL 
	) ultimateParentValuation;

	-- SELECT * FROM #ultimateParentValuation2; 

	---- Update the [Portfolio NAV (Custom)(T-1)] ultimate parent Valuation
	UPDATE BondPrice
		SET BondPrice.[Portfolio NAV (Custom)(T-1)] = u.[TotalValuationUltimateParentLevel]  
	FROM #BondPriceMovementAsPercentOfNav BondPrice
	LEFT JOIN #ultimateParentValuation2 u 
		ON u.Code = BondPrice.Portfolio
	WHERE ReportHeaderId = @ReportHeaderId;

	-- SELECT * FROM #BondPriceMovementAsPercentOfNav; 

	 --Delete PMR.BondPriceMovementAsPercentOfNAV where no active instument is associated to them
	DELETE #BondPriceMovementAsPercentOfNav 
	WHERE [Portfolio NAV (Custom)(T-1)] IS NULL
		AND ReportHeaderId = @ReportHeaderId;

	-- SELECT * FROM #BondPriceMovementAsPercentOfNav; 

	-- I: Update Historic Prices
	UPDATE BondPrice
		SET BondPrice.[Total Value (USD) -1d Actual] = Hld1d.[Total Value (USD)]
			, BondPrice.[Total Value (USD) Change %] = ((BondPrice.[Total Value (USD)] - ISNULL( Hld1d.[Total Value (USD)] , 0 )) / NULLIF(BondPrice.[Total Value (USD)],0))
	FROM #BondPriceMovementAsPercentOfNav  BondPrice
	LEFT JOIN #Holdings1d_tmp Hld1d 
		ON BondPrice.[Security Id] = Hld1d.[Security Id]
			AND BondPrice.Portfolio = Hld1d.Portfolio
			AND BondPrice.[Leg Type] = Hld1d.[Leg Type]
	WHERE BondPrice.ReportHeaderId = @ReportHeaderId;

	-- SELECT * FROM #BondPriceMovementAsPercentOfNav; 

	UPDATE #BondPriceMovementAsPercentOfNav 
		SET --[Price Change (Decimal)] = [Price Change %]   
			 [Share of NAV] = [Total Value (USD)] / [Portfolio NAV (Custom)]
	WHERE ReportHeaderId = @ReportHeaderId;

	-- SELECT * FROM #BondPriceMovementAsPercentOfNav; 

	UPDATE BondPrice
		SET --[Proportion] = [Price Change (Decimal)] + 1
			 [Portfolio NAV -1d] = nav1d.[Portfolio NAV (Custom)]
	FROM #BondPriceMovementAsPercentOfNav BondPrice
	INNER JOIN #NAV1dTemp nav1d 
		ON BondPrice.[Portfolio] = nav1d.[Portfolio]
	WHERE BondPrice.ReportHeaderId = @ReportHeaderId;

	-- SELECT * FROM #BondPriceMovementAsPercentOfNav; 

	--M: Update Dealing Desk
	UPDATE BondPrice
		SET	[Dealing Desk] = PosHld.[Dealing Desk]
	FROM #BondPriceMovementAsPercentOfNav BondPrice
	LEFT JOIN [PMR].TFRiskPositionADNew_holding PosHld 
		ON BondPrice.[Security Id] = PosHld.[Security Id] 
			AND BondPrice.[Portfolio] =  PosHld.[Portfolio]
	WHERE  [ReportHeaderId] = @ReportHeaderId;

	-- Select Data
	SELECT 
			 pm.Portfolio
			, pm.[Security Id]
			, pm.Description
			, pm.[Asset Class]
			, pm.[Asset Sub-Class]
			, pm.[Asset Type]
			, pm.[Asset Sub-type]
			, pm.[Leg Type]
			, pm.[Nominal]
			, pm.[Price Currency]
			, pm.[Total Value (USD)]
			, pm.[Total Value (USD) -1d Actual]
			, (pm.[Total Value (USD)] - pm.[Total Value (USD) -1d Actual]) / pm.[Total Value (USD)] AS "Total Value (USD) Change %"
			, pm.[Portfolio NAV (Custom)]
			, pm.[Portfolio NAV (Custom)(T-1)]
			, pm.[Portfolio NAV]
			, pm.[Portfolio NAV -1d]
			, pm.[Share of NAV]
			, pm.[Dealing Desk]
			, pm.[Report Type]
	FROM 
		#BondPriceMovementAsPercentOfNav pm
	WHERE
		pm.[Total Value (USD)] != 0
	ORDER BY 
		 pm.[Security Id], pm.Portfolio, pm.[Total Value (USD)] -- , pm.[Total Value (USD) -1d];  -- b.[Price Change %] DESC, 

	END
