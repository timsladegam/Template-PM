/// <reference path="../domain/settings.ts" />
import { Component, OnDestroy, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExcelExportData } from '@progress/kendo-angular-excel-export';
import { GridComponent, GridDataResult } from '@progress/kendo-angular-grid';
import { CompositeFilterDescriptor, filterBy, FilterDescriptor, GroupDescriptor, process, State } from '@progress/kendo-data-query';
import { ColumnSettings } from '../domain/column-settings.interface';
import { GridSettings } from '../domain/grid-settings.interface';
import { PriceMovementRecord } from '../domain/price-movement';
import { SecurityRecord } from '../domain/security-data';
import { DataService } from '../services/data.service';
import { PriceMovementService } from '../services/price-movement.service';
import { StatePersistingService } from '../services/state-persistence.service';

@Component({
  selector: 'app-security-data',
  encapsulation: ViewEncapsulation.None,
  templateUrl: './security-portfolio-movement.component.html',
  styleUrls: ['./price-movement.component.css']
})
export class SecurityDataComponent implements OnInit, OnDestroy {

  constructor(public dataService: DataService,
    private route: ActivatedRoute,
    private service: PriceMovementService,
    public persistingService: StatePersistingService) {
    this.gridData = [];
    this.exportData = this.exportData.bind(this);
    this.gridSettings = this.defaultState;

    const saved: GridSettings = this.persistingService.getGridSettings('securityGridSettings');
    if (saved !== null) {
      console.log('Got security grid settings');
      this.gridSettings = this.mapGridSettings(saved);
    }
  }
  public filter: CompositeFilterDescriptor;
  public gridData: SecurityRecord[];
  public ordersGridView: GridDataResult;
  public loading = false;
  public today: Date = new Date();
  public groups: GroupDescriptor[] = [];
  public position: 'top' | 'bottom' | 'both' = 'top';
  public gridSettings: GridSettings;

  @ViewChild('securityGrid')
  securityGrid: GridComponent;

  // the grid default setup
  public defaultState: GridSettings = {
    state: {
      skip: 0, take: 20,
      filter: {
        logic: 'and',
        filters: []
      }
    },
    columnsConfig: [{
      field: 'securityId',
      title: 'Security Id',
      _width: 120,
      headerStyle: '{\'font-weight\': \'bold\'}'
    }, {
      field: 'description',
      title: 'Description',
      _width: 250,
      headerStyle: '{\'font-weight\': \'bold\'}'
    }, {
      field: 'assetClass',
      title: 'Asset Class',
      _width: 120,
      headerStyle: '{\'font-weight\': \'bold\'}'
    }, {
      field: 'assetSubClass',
      title: 'Asset SubClass',
      _width: 140,
      headerStyle: '{\'font-weight\': \'bold\'}'
    }, {
      field: 'assetType',
      title: 'Asset Type',
      _width: 120,
      headerStyle: '{\'font-weight\': \'bold\'}'
    }, {
      field: 'assetSubtype',
      title: 'Asset Subtype',
      _width: 130,
      headerStyle: '{\'font-weight\': \'bold\'}'
    }, {
      field: 'nominal',
      title: 'Nominal',
      _width: 110,
      format: '{0:n0}',
      headerStyle: '{\'font-weight\': \'bold\'}',
      style: '{\'text-align\': \'right\'}'
    }, {
      field: 'priceCurrency',
      title: 'Price Ccy',
      _width: 110,
      headerStyle: '{\'font-weight\': \'bold\'}'
    }, {
      field: 'price',
      title: 'Price',
      filter: 'numeric',
      _width: 110,
      format: '{0:n5}',
      headerStyle: '{\'font-weight\': \'bold\'}'
    }, {
      field: 'priceDate',
      title: 'Price Date',
      filter: 'date',
      _width: 120,
      format: '{0:dd MMM yyyy}',
      headerStyle: '{\'font-weight\': \'bold\'}',
    }, {
      field: 'businessDaysSincePriceDate',
      title: 'Stale',
      filter: 'numeric',
      _width: 100,
      headerStyle: '{\'font-weight\': \'bold\'}',
      style: '{\'text-align\': \'right\'}'
    }, {
      field: 'priceSource',
      title: 'Price Source',
      _width: 130,
      headerStyle: '{\'font-weight\': \'bold\'}',
    }, {
      field: 'price1d',
      title: 'Price 1d',
      _width: 110,
      format: '{0:n5}',
      headerStyle: '{\'font-weight\': \'bold\'}',
      style: '{\'text-align\': \'right\'}'
    }, {
      field: 'priceDate1d',
      title: 'Price Date 1d',
      filter: 'date',
      _width: 140,
      format: '{0:dd MMM yyyy}',
      headerStyle: '{\'font-weight\': \'bold\'}'
    }, {
      field: 'priceSource1d',
      title: 'Price Source 1d',
      _width: 140,
      headerStyle: '{\'font-weight\': \'bold\'}'
    }, {
      field: 'priceChange',
      title: 'Price Change',
      _width: 140,
      filter: 'numeric',
      format: '{0:P5}',
      headerStyle: '{\'font-weight\': \'bold\'}',
      style: '{\'text-align\': \'right\'}'
    }, {
      field: 'proportion',
      title: 'Proportion',
      _width: 120,
      filter: 'numeric',
      format: '{0:n9}',
      headerStyle: '{\'font-weight\': \'bold\'}',
      style: '{\'text-align\': \'right\'}'
    }, {
      field: 'totalValueUsd',
      title: 'Total Value USD',
      _width: 150,
      format: '{0:n0}',
      headerStyle: '{\'font-weight\': \'bold\'}',
      style: '{\'text-align\': \'right\'}'
    }, {
      field: 'totalValueUsd1d',
      title: 'Total Value USD 1d',
      _width: 160,
      format: '{0:n0}',
      headerStyle: '{\'font-weight\': \'bold\'}',
      style: '{\'text-align\': \'right\'}'
    }, {
      field: 'totalValueUsd1dActual',
      title: 'Total Value USD 1d Actual',
      _width: 190,
      format: '{0:n0}',
      headerStyle: '{\'font-weight\': \'bold\'}',
      style: '{\'text-align\': \'right\'}'
    }, {
      field: 'portfolioNavCustom',
      title: 'Portfolio Nav Custom',
      _width: 180,
      format: '{0:n0}',
      headerStyle: '{\'font-weight\': \'bold\'}',
      style: '{\'text-align\': \'right\'}'
    }, {
      field: 'portfolioNav1dCustom',
      title: 'Portfolio Nav 1d Custom',
      _width: 190,
      format: '{0:n0}',
      headerStyle: '{\'font-weight\': \'bold\'}',
      style: '{\'text-align\': \'right\'}'
    }, {
      field: 'portfolioNav',
      title: 'Portfolio Nav',
      _width: 140,
      format: '{0:n0}',
      headerStyle: '{\'font-weight\': \'bold\'}',
      style: '{\'text-align\': \'right\'}'
    }, {
      field: 'portfolioNav1d',
      title: 'Portfolio Nav 1d',
      _width: 150,
      format: '{0:n0}',
      headerStyle: '{\'font-weight\': \'bold\'}',
      style: '{\'text-align\': \'right\'}'
    }, {
      field: 'recordType',
      title: 'Record Type',
      _width: 120,
      headerStyle: '{\'font-weight\': \'bold\'}'
    }, {
      field: 'staleException',
      title: 'Stale Exception',
      _width: 200,
      headerStyle: '{\'font-weight\': \'bold\'}'
    }, {
      field: 'exception',
      title: 'Exception',
      _width: 200,
      headerStyle: '{\'font-weight\': \'bold\'}'
      }],
    filterable: false,
    groupable: false
  };

  // on initialization
  public ngOnInit(): void {

    let id = this.route.snapshot.paramMap.get('id');
    console.log('id = ' + id);

    this.dataService.recordType = undefined;

    if (id) {
      id = id.toUpperCase();
      if (id === 'ETD' || id === 'OTC') {
        this.dataService.recordType = id;
      } else if (id === 'URGENT') {
        const val = this.dataService.urgent / 100;
        this.gridSettings.state.filter.filters = this.updateFilter('priceChange', this.gridSettings.state.filter.filters, [val], 'gte');
        this.ordersGridView = process(filterBy(this.gridData, this.gridSettings.state.filter), this.gridSettings.state);
      }
    }

    const data: any = JSON.stringify({
      forDate: this.dataService.forDate,
      assetClasses: this.dataService.selectedAssetClassList,
      currencies: this.dataService.selectedCurrencyList,
      dealingDesks: this.dataService.selectedDealingDeskList,
      recordType: this.dataService.recordType
    });
    console.log('SECURITY data : ' + data);

    this.getPriceMovements(false);
  }

  // prevent memory leak when component destroyed
  public ngOnDestroy() {
  }

  public getPriceMovements(force: boolean): void {

    this.loading = true;

    this.service.getPriceMovement(force).subscribe(
      result => {
        this.gridData = this.GetSecurityData(result);
        this.ordersGridView = process(this.gridData, this.gridSettings.state);
        console.log('security records: ' + this.gridData.length + ' records');
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

  // filter the price movement data and rollup to security level
  private GetSecurityData(data: PriceMovementRecord[]): SecurityRecord[] {

    const securityData: SecurityRecord[] = [];
    let prevSecurityId: string;
    let securityRecord: SecurityRecord;

    for (const i of data) {
      if (prevSecurityId !== i.securityId) {

        securityRecord = new SecurityRecord(i);

        securityRecord.nominal = i.nominal;
        securityRecord.totalValueUsd = i.totalValueUsd;
        securityRecord.totalValueUsd1d = i.totalValueUsd1d;
        securityRecord.totalValueUsd1dActual = i.totalValueUsd1dActual;
        securityRecord.portfolioNav = i.portfolioNav;
        securityRecord.portfolioNav1d = i.portfolioNav1d;
        securityRecord.portfolioNavCustom = i.portfolioNavCustom;
        securityRecord.portfolioNav1dCustom = i.portfolioNav1dCustom;

        // calc stale days ... if staleDays changes then this needs to be recalculated!
        securityRecord.staleException = i.businessDaysSincePriceDate > this.dataService.staleDays ? 'Stale Price - Over ' + this.dataService.staleDays + ' Days' : '';

        securityData.push(securityRecord);
        prevSecurityId = i.securityId;
      } else {

        securityRecord.nominal += i.nominal;
        securityRecord.totalValueUsd += i.totalValueUsd;
        securityRecord.totalValueUsd1d += i.totalValueUsd1d;
        securityRecord.totalValueUsd1dActual += i.totalValueUsd1dActual;
        securityRecord.portfolioNav += i.portfolioNav;
        securityRecord.portfolioNav1d += i.portfolioNav1d;
        securityRecord.portfolioNavCustom += i.portfolioNavCustom;
        securityRecord.portfolioNav1dCustom += i.portfolioNav1dCustom;
      }
    }

    return securityData;
  }

  // data source for export excel
  public exportData(): ExcelExportData {
    const s: State = { filter: this.gridSettings.state.filter };
    const result: ExcelExportData = {
      data: process(this.gridData, s).data,
      group: this.groups
    };

    return result;
  }

  // when data state changes
  public dataStateChange(state: State): void {
    this.gridSettings.state = state;
    this.ordersGridView = process(this.gridData, state);
    this.saveGridSettings(this.securityGrid);
  }
  // when the grouping changes
  public groupChange(groups: GroupDescriptor[]): void {
    this.groups = groups;
    this.ordersGridView = process(this.gridData, this.gridSettings.state);
  }

  // show hide the filter row
  public filterToggleClick(): void {
    this.gridSettings.filterable = !this.gridSettings.filterable;
    this.saveGridSettings(this.securityGrid);
  }

  // show hide the group panel
  public groupToggleClick(): void {
    this.gridSettings.groupable = !this.gridSettings.groupable;
    this.saveGridSettings(this.securityGrid);
  }

  // reset grid cols to default
  public resetClick(): void {
    this.gridSettings = this.defaultState;
    this.saveGridSettings(this.securityGrid);
  }

  public refreshClick(): void {
    this.gridData = [];
    this.ordersGridView = process(this.gridData, this.gridSettings.state);
    this.getPriceMovements(true);
  }

  private arrayRemove(arr, value) {
    return arr.filter(ele => ele['field'] !== value);
  }

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

  // when a column is reordered
  public onReorder(e: any): void {
    console.log(`reorder: ${e.oldIndex} : ${e.newIndex}`);
    const reorderedColumn = this.gridSettings.columnsConfig.splice(e.oldIndex, 1);
    this.gridSettings.columnsConfig.splice(e.newIndex, 0, ...reorderedColumn);
    this.saveGridSettings(this.securityGrid);
  }

  // when a column is resized
  public onResize(e: any): void {
    console.log(`resize: ${e.oldIndex} : ${e.newIndex}`);
    e.forEach(item => {
      this.gridSettings.columnsConfig.find(col => col.field === item.column.field).width = item.newWidth;
    });

    this.saveGridSettings(this.securityGrid);
  }

  // when a column is hidden\shown
  public onVisibilityChange(e: any): void {
    console.log('visibility');
    e.columns.forEach(column => {
      this.gridSettings.columnsConfig.find(col => col.field === column.field).hidden = column.hidden;
    });

    this.saveGridSettings(this.securityGrid);
  }


  public mapGridSettings(gridSettings: GridSettings): GridSettings {

    const state = gridSettings.state;
    return {
      state: gridSettings.state,
      columnsConfig: gridSettings.columnsConfig.sort((a, b) => a.orderIndex - b.orderIndex),
      filterable: gridSettings.filterable,
      groupable: gridSettings.groupable
    };
  }

  // save grid settings to local starage
  public saveGridSettings(grid: GridComponent): void {
    const columns = grid.columns;
    console.log('Save security grid settings');
    const gridConfig = {
      state: this.gridSettings.state,
      columnsConfig: columns.toArray().map(item => {
        return Object.keys(item)
          .filter(propName => !propName.toLowerCase()
            .includes('template'))
          .reduce((acc, curr) => ({ ...acc, ...{ [curr]: item[curr] } }), <ColumnSettings>{});
      }),
      filterable: this.gridSettings.filterable,
      groupable: this.gridSettings.groupable
    };

    this.persistingService.setGridSettings('securityGridSettings', gridConfig);
  }
}
