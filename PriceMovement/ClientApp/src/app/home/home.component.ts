import { Component, OnInit } from '@angular/core';

import { DataService } from '../services/data.service';
import { PriceMovementService } from '../services/price-movement.service';
import { StatePersistingService } from '../services/state-persistence.service';

import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {
  public loading = false;
  public today: Date = new Date();
  public assetClasses: IMultiSelectOption[];
  public currencies: IMultiSelectOption[];
  public dealingDesks: IMultiSelectOption[];
  public selectedAssetClassList: string[] = [];
  public selectedCurrencyList: string[] = [];
  public selectedDealingDeskList: string[] = [];
  public multiSelectSettings: IMultiSelectSettings = {
    buttonClasses: 'btn btn-outline-dark btn-sm',
    dynamicTitleMaxItems: 5,
    displayAllSelectedText: true
  };
  public multiSelectTexts: IMultiSelectTexts = {
    checkAll: 'Select all',
    uncheckAll: 'Unselect all',
    checked: 'item selected',
    checkedPlural: 'items selected',
    defaultTitle: 'All',
    allSelected: 'All selected'
  };

  constructor(private service: PriceMovementService, public dataService: DataService, public persistingService: StatePersistingService) {
  }

  // on initialization
  public ngOnInit(): void {

    this.selectedAssetClassList = this.dataService.selectedAssetClassList;
    this.selectedCurrencyList = this.dataService.selectedCurrencyList;
    this.selectedDealingDeskList = this.dataService.selectedDealingDeskList;

    if (this.dataService.assetClassList.length === 0 ||
      this.dataService.currencyList.length === 0 ||
      this.dataService.dealingDeskList.length === 0) {

      this.getDropdowns();
      return;
    }

    console.log('dropdowns cached');
    this.assetClasses = this.dataService.assetClassList.map(r => <IMultiSelectOption>{ id: r, name: r });
    this.currencies = this.dataService.currencyList.map(r => <IMultiSelectOption>{ id: r, name: r });
    this.dealingDesks = this.dataService.dealingDeskList.map(r => <IMultiSelectOption>{ id: r, name: r });
  }

  // retrieve the dropdown list data
  public getDropdowns(): void {

    console.log('getDropdowns');
    this.loading = true;

    this.service.getDropDownData().subscribe(
      result => {
        this.assetClasses = result.assetClasses.map(r => <IMultiSelectOption>{ id: r, name: r });
        this.currencies = result.currencies.map(r => <IMultiSelectOption>{ id: r, name: r });
        this.dealingDesks = result.dealingDesks.map(r => <IMultiSelectOption>{ id: r, name: r });
        this.dataService.assetClassList = result.assetClasses;
        this.dataService.currencyList = result.currencies;
        this.dataService.dealingDeskList = result.dealingDesks;
        this.loading = false;
      },
      error => { this.loading = false; console.error(error); },
      () => {
        this.loading = false;
        console.log('success getDropdowns');
      });
  }

  // on asset class drop down change
  public onAssetClassChange(obj) {
    this.dataService.selectedAssetClassList = this.selectedAssetClassList.slice(0);
    this.persistingService.setDataSettings('dataSettings', this.dataService.settings);
    console.log('save settings: ' + JSON.stringify(this.dataService.settings));
  }

  // on currency drop down change
  public onCurrencyChange(obj) {
    this.dataService.selectedCurrencyList = this.selectedCurrencyList.slice(0);
    this.persistingService.setDataSettings('dataSettings', this.dataService.settings);
    console.log('save settings: ' + JSON.stringify(this.dataService.settings));
  }

  // on dealing desk drop down change
  public onDealingDeskChange(obj) {
    this.dataService.selectedDealingDeskList = this.selectedDealingDeskList.slice(0);
    this.persistingService.setDataSettings('dataSettings', this.dataService.settings);
    console.log('save settings: ' + JSON.stringify(this.dataService.settings));
  }
}
