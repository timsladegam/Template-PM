USE [FIND]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tim Slade
-- Create date: January 2019
-- Description:	PriceMovement store procedure. Retrieve lookup data of distinct Asset Classes, Currencies and dealing desks.
-- =============================================
CREATE PROCEDURE [PMR].[GetStaticData]
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	-- get asset classes
	SELECT DISTINCT([Asset class]) FROM pmr.PriceComparisonHolding_archive p ORDER BY 1;

	-- get price currencies
	SELECT DISTINCT([Price Currency]) FROM pmr.PriceComparisonHolding_archive p ORDER BY 1;

	-- get dealing desks
	SELECT DISTINCT([Dealing Desk]) FROM pmr.TFRiskPositionADNew_holding p ORDER BY 1;

END
