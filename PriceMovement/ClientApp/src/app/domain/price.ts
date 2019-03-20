export class PriceRecord {

  constructor(data: any) {
    this.securityId = data.securityId;
    this.price = data.price;
    this.priceCurrency = data.priceCurrency;
    this.pricePoint = new Date(data.pricePoint);
    this.source = data.source;
  }

  public securityId: string;
  public price: string;
  public priceCurrency: string;
  public pricePoint: Date;
  public source: string;
}
