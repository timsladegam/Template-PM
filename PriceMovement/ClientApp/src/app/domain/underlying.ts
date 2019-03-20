export class UnderlyingRecord {

  constructor(data: any) {
    this.portfolio = data.portfolio;
    this.securityId = data.securityId;

    this.linked = data.linked;

    this.price = data.price;
    this.price1d = data.price1d;
    this.priceDate = this.setDate(data.priceDate);
    this.priceDate1d = this.setDate(data.priceDate1d);

    this.businessDaysSincePriceDate = data.businessDaysSincePriceDate;
    this.portfolioNavCustom = data.portfolioNavCustom;
  }

  public portfolio: string;
  public securityId: string;
  public description: string;
  public priceCurrency: string;
  public price: number;
  public priceDate: Date;
  public businessDaysSincePriceDate: number;
  public price1d: number;
  public priceDate1d: Date;
  public portfolioNavCustom: number;

  public linked: string;
  public displayFactor: number;
  public factor: number;
  public factor1d: number;

  public get staleException() {
    return this.businessDaysSincePriceDate > 5 ? 'Stale Price - Over 5 Days' : '';
  }

  public setDate(date: any): Date | any {
    const temp = new Date(date);
    if (temp.getFullYear() !== 1970) {
      return temp;
    }

    return '';
  }
}
