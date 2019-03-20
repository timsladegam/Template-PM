export class Settings {

  constructor() {
    this.dayCount = 5;
    this.urgent = 50;
    this.staleDays = 5;
    this.showFlat = false;
  }

  public dayCount: number;
  public urgent: number;
  public staleDays: number;
  public showFlat: boolean;

  // dropdown selected data
  public selectedAssetClassList: string[] = [];
  public selectedCurrencyList: string[] = [];
  public selectedDealingDeskList: string[] = [];

  // copy from raw js object to type
  public copyFrom(fields?:
    {
      dayCount?: number,
      urgent?: number,
      staleDays?: number,
      showFlat?: boolean,
      selectedAssetClassList?: string[],
      selectedCurrencyList?: string[],
      selectedDealingDeskList?: string[]
    }): Settings {
    this.dayCount = fields && fields.dayCount;
    this.urgent = fields && fields.urgent;
    this.staleDays = fields && fields.staleDays;
    this.showFlat = fields && fields.showFlat;
    this.selectedAssetClassList = fields && fields.selectedAssetClassList;
    this.selectedCurrencyList = fields && fields.selectedCurrencyList;
    this.selectedDealingDeskList = fields && fields.selectedDealingDeskList;

    return this;
  }
}
