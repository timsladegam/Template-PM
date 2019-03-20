export class SecurityRecord {

  constructor(data: any) {
    this.securityId = data.securityId;
    this.description = data.description;
    this.assetClass = data.assetClass;
    this.assetSubClass = data.assetSubClass;
    this.assetType = data.assetType;
    this.assetSubtype = data.assetSubtype;
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
    this.recordType = data.recordType;
  }

  public securityId: string;
  public description: string;
  public assetClass: string;
  public assetSubClass: string;
  public assetType: string;
  public assetSubtype: string;

  public nominal = 0;

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

  public recordType: string;

  public staleException: string;

  public setDate(date: any): Date | any {
    const temp = Date.parse(date);
    if (!isNaN(temp)) {
      return new Date(temp);
    }

    return '';
  }
}
