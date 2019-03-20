import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';

import { DataStateChangeEvent, GridDataResult } from '@progress/kendo-angular-grid';
import { GroupDescriptor, process, State, CompositeFilterDescriptor } from '@progress/kendo-data-query';
import { ExcelExportData } from '@progress/kendo-angular-excel-export';

import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';

import { DataService } from '../services/data.service';
import { PriceMovementService } from '../services/price-movement.service';
import { StaleYieldPointRecord } from '../domain/stale-yield-point';

@Component({
  selector: 'app-stale-yield',
  encapsulation: ViewEncapsulation.None,
  templateUrl: './stale-yield.component.html',
  styleUrls: ['./price-movement.component.css']
})
export class StaleYieldComponent implements OnInit, OnDestroy {
  public filter: CompositeFilterDescriptor;
  public gridData: StaleYieldPointRecord[];
  public ordersGridView: GridDataResult;
  public loading = false;
  public today: Date = new Date();
  public forDate: string = this.today.getFullYear() + '-' + (this.today.getMonth() + 1) + '-' + this.today.getDate();
  public dayCount = 5;
  public useTest = true;
  public groups: GroupDescriptor[] = [];
  public state: State = { skip: 0, take: 20, filter: { logic: 'and', filters: [] }, group: this.groups };
  public position: 'top' | 'bottom' | 'both' = 'top';
  public filterable = false;
  public groupable = false;
  public assetClassList: IMultiSelectOption[];
  public currencyList: IMultiSelectOption[];
  public dealingDeskList: IMultiSelectOption[];
  public selectedAssetClassList: string[];
  public selectedCurrencyList: string[];
  public selectedDealingDeskList: string[];
  public criteria: string;
  public title: string;

  public multiSelectSettings: IMultiSelectSettings = {
    buttonClasses: 'btn btn-default btn-sm',
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

  constructor(public dataService: DataService, private service: PriceMovementService) {
    this.gridData = [];
    this.exportData = this.exportData.bind(this);
  }

  // on initialisation
  public ngOnInit(): void {

    this.title = 'Stale Yield Points';
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    this.criteria = `Report Date - ${this.dataService.forDatePicker.toLocaleDateString('en-GB', options)}`;

    this.getPriceMovements();
  }

  // prevent memory leak when component destroyed
  public ngOnDestroy() {
  }

  public getPriceMovements(): void {

    console.log('onclick getPriceMovements ' + this.forDate + ' : ' + this.useTest);
    this.loading = true;

    this.service.getStaleMovement().subscribe(
        result => {
          this.gridData = result;
          this.ordersGridView = process(this.gridData, this.state);
          console.log(result);
        },
        error => console.error(error),
        () => {
          this.loading = false;
          console.log('success');
        });
  }

  // data source for export excel
  public exportData(): ExcelExportData {
    const result: ExcelExportData = {
      data: process(this.gridData, {}).data,
      group: this.groups
    };

    return result;
  }

  // when data state changes
  public dataStateChange(state: DataStateChangeEvent): void {
    this.state = state;
    this.ordersGridView = process(this.gridData, this.state);
  }

  // when the grouping changes
  public groupChange(groups: GroupDescriptor[]): void {
    this.groups = groups;
    this.ordersGridView = process(this.gridData, this.state);
  }

  // show hide the filter row
  public filterToggleClick(): void {
    this.filterable = !this.filterable;
  }

  // show hide the group panel
  public groupToggleClick(): void {
    this.groupable = !this.groupable;
  }
}
