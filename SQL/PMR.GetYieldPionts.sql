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
CREATE PROCEDURE [PMR].[GetYieldPoints]
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

--YIELD POINTS MOVEMENT
WITH yieldPointMovement AS
(
	SELECT
		Identifier
		, Description
		, [price currency]
		, [price display factor]
		, [Current Price]
		, [Last Price Date]
		, [Price -1d (1 days ago)]
		, [Latest Factor]
		, [Factor -1d (1 days ago)]
		, [Asset Class]
		, [Asset Sub-class]  
	FROM
		[PMR].PriceComparisonIndex_archive
	WHERE
		ReportHeaderId = @ReportHeaderId
)

-- select * from yieldPointMovement

, excludeZero AS
(
	SELECT 
		Identifier
		, Description
		, [price currency]
		, [price display factor]
		, [Current Price]
		, [Last Price Date]
		, [Price -1d (1 days ago)]
		, [Latest Factor]
		, [Factor -1d (1 days ago)]
		, [Asset Class]
		, [Asset Sub-class]  
	FROM
		yieldPointMovement
	WHERE
		[Price -1d (1 days ago)] > 0 
)

, calcPriceChange AS
(
	SELECT
		[Price Change] = ([Current Price] - [Price -1d (1 days ago)]) / [Price -1d (1 days ago)]
		, Identifier
		, Description
		, [price currency]
		, [price display factor]
		, [Current Price]
		, [Last Price Date]
		, [Price -1d (1 days ago)]
		, [Latest Factor]
		, [Factor -1d (1 days ago)]
		, [Asset Class]
		, [Asset Sub-class] 
	FROM 
		excludeZero
)

, removeAbsZero AS
(
	SELECT
	[Price Change]
	, Identifier
	, Description
	, [price currency]
	, [price display factor]
	, [Current Price]
	, [Last Price Date]
	, [Price -1d (1 days ago)]
	, [Latest Factor]
	, [Factor -1d (1 days ago)]
	, [Asset Class]
	, [Asset Sub-class] 
	FROM
		calcPriceChange 
	WHERE 
		ABS([Price Change]) != 0 
)

SELECT
	[Price Change]
	, Identifier
	, Description
	, [price currency]
	, [price display factor]
	, [Current Price]
	, [Last Price Date]
	, [Price -1d (1 days ago)]
	, [Latest Factor]
	, [Factor -1d (1 days ago)]
	, [Asset Class]
	, [Asset Sub-class]
FROM
	removeAbsZero
ORDER BY 
	ABS([Price Change]) DESC, Identifier


END
