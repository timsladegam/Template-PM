export class PriceMovementRecord {

  constructor(data: any) {
    this.portfolio = data.portfolio;
    this.securityId = data.securityId;
    this.description = data.description;
    this.assetClass = data.assetClass;
    this.assetSubClass = data.assetSubClass;
    this.assetType = data.assetType;
    this.assetSubtype = data.assetSubtype;
    this.legType = data.legType;
    this.nominal = data.nominal;
    this.priceCurrency = data.priceCurrency;
    this.price = data.price;
    this.priceDate = this.setDate(data.priceDate);
    this.priceSource = data.priceSource;
    this.businessDaysSincePriceDate = data.businessDaysSincePriceDate;
    this.price1d = data.price1d;
    this.priceDate1d = this.setDate(data.priceDate1d);
    this.priceSource1d = data.priceSource1d;
    this.priceChange = data.priceChange;
    this.proportion = data.proportion;
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
    this.recordType = data.recordType;
  }

  public portfolio: string;
  public securityId: string;
  public description: string;
  public assetClass: string;
  public assetSubClass: string;
  public assetType: string;
  public assetSubtype: string;
  public legType: string;
  public nominal: number;
  public priceCurrency: string;
  public price: number;
  public priceDate: Date;
  public priceSource: string;
  public businessDaysSincePriceDate: number;
  public price1d: number;
  public priceDate1d: Date;
  public priceSource1d: string;
  public priceChange: number;
  public proportion: number;
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
  public recordType: string;
  public staleException: string;

  public get exception() {
    switch (this.recordType) {
      case 'BOND':
        return Math.abs(this.priceChange) >= 0.025 ? 'Price change 2.5% movement' : '';
      case 'ETD':
        return this.price <= 5 && Math.abs(this.priceChange) >= 0.5 ? 'Price change 50% movement' :
          this.price > 5 && Math.abs(this.priceChange) >= 0.025 ? 'Price change 2.5% movement' : '';
      case 'OTC':
        return this.price > 5 && Math.abs(this.priceChange) >= 0.1
          ? 'Price change 10% movement'
          : this.price > 2 && this.price <= 5 && Math.abs(this.priceChange) >= 0.2
            ? 'Price change 20% movement'
            : this.price <= 2 && Math.abs(this.priceChange) >= 0.5
              ? 'Price change 50% movement' : '';
      default:
        return Math.abs(this.marketValueImpact) > 0.0001 ? 'Movement contributes > 0.01% of NAV' : '';
    }
  }

  public setDate(date: any): Date | any {
    const temp = new Date(date);
    if (temp.getFullYear() !== 1970) {
      return temp;
    }

    return '';
  }
}
