import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DataStateChangeEvent, GridDataResult } from '@progress/kendo-angular-grid';
import { ExcelExportData } from '@progress/kendo-angular-excel-export';
import { GroupDescriptor, process, State, filterBy, FilterDescriptor, CompositeFilterDescriptor } from '@progress/kendo-data-query';

import { DataService } from '../services/data.service';
import { PriceMovementService } from '../services/price-movement.service';
import { PriceMovementRecord } from '../domain/price-movement';

@Component({
  selector: 'app-flat-data',
  encapsulation: ViewEncapsulation.None,
  templateUrl: './flat-movement.component.html',
  styleUrls: ['./price-movement.component.css']
})
export class FlatDataComponent implements OnInit, OnDestroy {
  public filter: CompositeFilterDescriptor;
  public gridData: PriceMovementRecord[];
  public ordersGridView: GridDataResult;
  public loading = false;
  public today: Date = new Date();
  public groups: GroupDescriptor[] = [];
  public state: State = { skip: 0, take: 20, filter: { logic: 'and', filters: [] }, group: this.groups };
  public position: 'top' | 'bottom' | 'both' = 'top';
  public filterable = false;
  public groupable = false;

  constructor(public dataService: DataService, private route: ActivatedRoute, private service: PriceMovementService) {
    this.gridData = [];
    this.exportData = this.exportData.bind(this);
  }

  // on initialization
  public ngOnInit(): void {

    // extract id from route info
    let id = this.route.snapshot.paramMap.get('id');
    console.log('id = ' + id);

    // set up data
    this.dataService.recordType = undefined;

    if (id) {
      id = id.toUpperCase();
      if (id === 'ETD' || id === 'OTC') {
        this.dataService.recordType = id;
      } else if (id === 'URGENT') {
        const val = this.dataService.urgent / 100;
        this.state.filter.filters = this.updateFilter('priceChange', this.state.filter.filters, [val], 'gte');
        this.ordersGridView = process(filterBy(this.gridData, this.state.filter), this.state);
      }
    }

    const data: any = JSON.stringify({
      forDate: this.dataService.forDate,
      assetClasses: this.dataService.selectedAssetClassList,
      currencies: this.dataService.selectedCurrencyList,
      dealingDesks: this.dataService.selectedDealingDeskList,
      recordType: this.dataService.recordType
    });
    console.log('FLAT data : ' + data);

    // get data on load
    this.getPriceMovements(false);
  }

  // on exit
  public ngOnDestroy() {
  }

  // get price movement data from server
  public getPriceMovements(force: boolean): void {

    this.loading = true;

    this.service.getPriceMovement(force).subscribe(
      result => {
        this.gridData = this.getResult(result);
        this.ordersGridView = process(this.gridData, this.state);
        console.log('flat data: ' + this.gridData.length + ' records');
      },
      error => {
        this.loading = false;
        console.error(error);
      },
      () => {
        this.loading = false;
        console.log('success getPriceMovements');
      });
  }

  // data source for export excel
  public exportData(): ExcelExportData {
    const s: State = { filter: this.state.filter };
    const result: ExcelExportData = {
      data: process(this.gridData, s).data,
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

  // force a refresh of data from server
  public refreshClick(): void {
    this.gridData = [];
    this.ordersGridView = process(this.gridData, this.state);
    this.getPriceMovements(true);
  }

  // update result with any calculations ... if staleDays changes then this needs to be recalculated!
  private getResult(data: PriceMovementRecord[]): PriceMovementRecord[] {

    data.forEach(d => d.staleException = d.businessDaysSincePriceDate > this.dataService.staleDays ? 'Stale Price - Over ' + this.dataService.staleDays + ' Days' : '');
    return data;
  }

  // remove an item from an array with the given value
  private arrayRemove(arr, value) {
    return arr.filter(ele => ele['field'] !== value);
  }

  // update the grid filters
  private updateFilter(field, filters, values, operator = 'contains') {

    console.log('Filter start:' + filters.map(n => n['field'] + ' ' + n['operator'] + ' ' + n['value']));

    // remove existing filter
    let i = filters.length;
    while (i > 0) {
      if (filters.some(f => f['field'] === field)) {
        filters = this.arrayRemove(filters, field);
      }
      i--;
    }

    // add new selections
    let j = values.length;
    while (j > 0) {
      const n = values[j - 1];
      const fd: FilterDescriptor = {
        field: field,
        operator: operator,
        value: n,
        ignoreCase: true
      };
      filters.push(fd);
      j--;
    }

    console.log('Filter end:' + filters.map(n => n['field'] + ' ' + n['operator'] + ' ' + n['value']));

    return filters;
  }
}
