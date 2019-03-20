export class DropDowns {

  constructor(data: any) {
    this.assetClasses = data.assetClasses;
    this.currencies = data.currencies;
    this.dealingDesks = data.dealingDesks;
  }

  public assetClasses: string[];
  public currencies: string[];
  public dealingDesks: string[];
}
