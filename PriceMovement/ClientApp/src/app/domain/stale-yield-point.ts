export class StaleYieldPointRecord {

  constructor(data: any) {
    this.securityId = data.securityId;
    this.description = data.description;
    this.priceCurrency = data.priceCurrency;

    this.displayFactor = data.displayFactor;

    this.priceChange = data.priceChange;

    this.price = data.price;
    this.price1d = data.price1d;
    this.priceDate = this.setDate(data.priceDate);
    this.priceDate1d = this.setDate(data.priceDate1d);

    this.factor = data.factor;
    this.factor1d = data.factor1d;

    this.assetClass = data.assetClass;
    this.assetSubClass = data.assetSubClass;
    this.assetType = data.assetType;
    this.assetSubtype = data.assetSubtype;

    this.businessDaysSincePriceDate = data.businessDaysSincePriceDate;
  }

  public securityId: string;
  public description: string;
  public assetClass: string;
  public assetSubClass: string;
  public assetType: string;
  public assetSubtype: string;
  public priceCurrency: string;
  public price: number;
  public priceDate: Date;
  public businessDaysSincePriceDate: number;
  public price1d: number;
  public priceDate1d: Date;
  public priceChange: number;
  public proportion: number;

  public displayFactor: number;
  public factor: number;
  public factor1d: number;

  public setDate(date: any): Date | any {
    const temp = new Date(date);
    if (temp.getFullYear() !== 1970) {
      return temp;
    }

    return '';
  }
}
