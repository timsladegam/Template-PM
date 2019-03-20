export class PortfolioRecord {

  constructor(data: any) {
    this.portfolio = data.portfolio;
    this.legType = data.legType;
    this.nominal = data.nominal;
    this.totalValueUsd = data.totalValueUsd;
    this.totalValueUsd1d = data.totalValueUsd1d;
    this.totalValueUsd1dActual = data.totalValueUsd1dActual;
    this.totalValueUsdChange = data.totalValueUsdChange;
    this.portfolioNav = data.portfolioNav;
    this.portfolioNav1d = data.portfolioNav1d;
    this.portfolioNavCustom = data.portfolioNavCustom;
    this.portfolioNav1dCustom = data.portfolioNav1dCustom;
    this.shareOfNav = data.shareOfNav;
    this.shareOfNav1d = data.shareOfNav1d;
    this.shareOfNavChange = data.shareOfNavChange;
    this.marketValueImpact = data.marketValueImpact;
    this.dealingDesk = data.dealingDesk;
  }

  public portfolio: string;
  public legType: string;
  public nominal: number;
  public totalValueUsd: number;
  public totalValueUsd1d: number;
  public totalValueUsd1dActual: number;
  public totalValueUsdChange: number;
  public portfolioNav: number;
  public portfolioNav1d: number;
  public portfolioNavCustom: number;
  public portfolioNav1dCustom: number;
  public shareOfNav: number;
  public shareOfNav1d: number;
  public shareOfNavChange: number;
  public marketValueImpact: number;
  public dealingDesk: string;
}
