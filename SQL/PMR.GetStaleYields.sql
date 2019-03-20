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
CREATE PROCEDURE [PMR].[GetStaleYields]
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

--STALE YIELD POINTS
WITH Stale AS
(
SELECT 
	*
	, CASE WHEN p.[Current Price] = p.[Price -1d (1 days ago)] AND p.[Price -1d (1 days ago)] = p.[Price -2d (2 days ago)] AND p.[Price -2d (2 days ago)] = p.[Price -3d (3 days ago)] AND p.[Price -3d (3 days ago)] = p.[Price -4d (4 days ago)] AND p.[Price -4d (4 days ago)] = p.[Price -5d (5 days ago)] THEN 5		   
		   WHEN p.[Current Price] = p.[Price -1d (1 days ago)] AND p.[Price -1d (1 days ago)] = p.[Price -2d (2 days ago)] AND p.[Price -2d (2 days ago)] = p.[Price -3d (3 days ago)] AND p.[Price -3d (3 days ago)] = p.[Price -4d (4 days ago)] THEN 4
		   WHEN p.[Current Price] = p.[Price -1d (1 days ago)] AND p.[Price -1d (1 days ago)] = p.[Price -2d (2 days ago)] AND p.[Price -2d (2 days ago)] = p.[Price -3d (3 days ago)] THEN 3
		   WHEN p.[Current Price] = p.[Price -1d (1 days ago)] AND p.[Price -1d (1 days ago)] = p.[Price -2d (2 days ago)] THEN 2
		   WHEN p.[Current Price] = p.[Price -1d (1 days ago)] THEN 1
		   ELSE NULL
		   END AS Stale
FROM
	[PMR].PriceComparisonIndex_archive p
WHERE
	p.ReportHeaderId = @ReportHeaderId
),

excludeNonStale AS
(
SELECT 
	* 
FROM 
	Stale 
WHERE 
	Stale IS NOT NULL
)

SELECT	
	Identifier
	, Description
	, [Price Currency]
	, [Current Price]
	, [Last Price Date]
	, [Price -1d (1 days ago)]
	, [Price -2d (2 days ago)]
	, [Price -3d (3 days ago)]
	, [Price -4d (4 days ago)]
	, [Price -5d (5 days ago)]
	, [Latest Factor]
	, [Factor -1d (1 days ago)]
	, [Asset Class]
	, [Asset Sub-class]
	, [Stale] AS [Stale Days (inc Weekends)]
FROM 
	excludeNonStale 
ORDER BY 
	Identifier

END
