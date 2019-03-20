import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { DataService } from '../services/data.service';


@Component({
  selector: 'app-fetch-data',
  encapsulation: ViewEncapsulation.None,
  templateUrl: './price-movement.component.html',
  styleUrls: ['./price-movement.component.css']
})
export class FetchDataComponent implements OnInit, OnDestroy {
  public loading = false;
  public today: Date = new Date();
  public criteria: string;
  public title: string;
  public showFlat: boolean;

  constructor(private readonly route: ActivatedRoute, private readonly router: Router, public dataService: DataService) {
    // force route reload whenever params change
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  // on initialization
  public ngOnInit(): void {

    // extract router param
    let id = this.route.snapshot.paramMap.get('id');

    this.dataService.recordType = undefined;
    this.showFlat = this.dataService.showFlat;
    this.title = 'Price Movement - All records';

    // set title
    if (id) {
      id = id.toUpperCase();
      if (id === 'ETD') {
        this.title = 'Price Movement - ETD';
        this.dataService.recordType = id;
      } else if (id === 'OTC') {
        this.title = 'Price Movement - OTC';
        this.dataService.recordType = id;
      } else if (id === 'URGENT') {
        this.title = 'Price Movement - URGENT';
      }
    }

    // set criteria
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    this.criteria = `Report Date - ${this.dataService.forDatePicker.toLocaleDateString('en-GB', options)}`;
    if (this.dataService.selectedAssetClassList.length > 0) {
      this.criteria += `; Asset classes - ${this.dataService.selectedAssetClassList}`;
    }
    if (this.dataService.selectedCurrencyList.length > 0) {
      this.criteria += `; Currencies - ${this.dataService.selectedCurrencyList}`;
    }
    if (this.dataService.selectedDealingDeskList.length > 0) {
      this.criteria += `; Dealing Desks - ${this.dataService.selectedDealingDeskList}`;
    }

    console.log(`title : ${this.title}`);
    console.log(`criteria : ${this.criteria}`);
  }

  // prevent memory leak when component destroyed
  public ngOnDestroy() {
    // nothing
  }

  // log flat\security toggle
  public onflatChanged(value: number): void {
    console.log(value ? 'Display Flat' : 'Display Security');
  }
}
