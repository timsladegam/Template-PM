import { Injectable } from '@angular/core';
import { StatePersistingService } from '../services/state-persistence.service';

import { Settings } from '../domain/settings';

@Injectable()
export class DataService {

  public settings: Settings = new Settings;
  private today: Date = new Date();

  constructor(public persistingService: StatePersistingService) {

    console.log('DataService ctor');

    this.forDatePicker = this.today;

    const obj: any = this.persistingService.getDataSettings('dataSettings');
    console.log('read settings: ' + JSON.stringify(obj));
    if (obj != null) {

      this.settings = this.settings.copyFrom({
        dayCount: obj.dayCount,
        urgent: obj.urgent,
        staleDays: obj.staleDays,
        showFlat: obj.showFlat,
        selectedAssetClassList: obj.selectedAssetClassList,
        selectedCurrencyList: obj.selectedCurrencyList,
        selectedDealingDeskList: obj.selectedDealingDeskList
      });
    }

    console.log('Settings: ' + JSON.stringify(this.settings));
  }

  public forDatePicker: Date;
  public recordType: string;
  public priceChange: number;

  public get dayCount() { return this.settings.dayCount; }
  public set dayCount(value) { this.settings.dayCount = value; }

  public get urgent() { return this.settings.urgent; }
  public set urgent(value) { this.settings.urgent = value; }

  public get staleDays() { return this.settings.staleDays; }
  public set staleDays(value) { this.settings.staleDays = value; }

  public get showFlat() { return this.settings.showFlat; }
  public set showFlat(value) { this.settings.showFlat = value; }


  // dropdown selected data
  public get selectedAssetClassList() { return this.settings.selectedAssetClassList; }
  public set selectedAssetClassList(value) { this.settings.selectedAssetClassList = value; }

  public get selectedCurrencyList() { return this.settings.selectedCurrencyList; }
  public set selectedCurrencyList(value) { this.settings.selectedCurrencyList = value; }

  public get selectedDealingDeskList() { return this.settings.selectedDealingDeskList; }
  public set selectedDealingDeskList(value) { this.settings.selectedDealingDeskList = value; }

  // all dropdown data
  public assetClassList: string[] = [];
  public currencyList: string[] = [];
  public dealingDeskList: string[] = [];

  public get forDate() {
    return this.forDatePicker.getFullYear() + '-' + (this.forDatePicker.getMonth() + 1) + '-' + this.forDatePicker.getDate();
  }
}
