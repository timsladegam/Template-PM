import { Component, Input } from '@angular/core';

import { PriceService } from '../services/price.service';
import { PriceRecord } from '../domain/price';

@Component({
  selector: 'app-price-details',
  providers: [PriceService],
  templateUrl: './price-details.component.html'
})
export class PriceDetailsComponent {
  @Input() public securityId: string;
  @Input() public forDate: string;
  @Input() public dayCount: string;
  public gridData: PriceRecord[];

  constructor(private service: PriceService) {
  }

  ngOnInit(): void {

    this.getPrices();
  }

  public getPrices(): void {

    console.log('onclick getPrices: ' + this.securityId + ' : ' + this.forDate + ' : ' + this.dayCount);

    this.service.getPrices(this.securityId, this.forDate, this.dayCount).subscribe(
      result => {
        this.gridData = result;
        console.log(result);
      }, error => console.error(error),
      () => console.log('success getPrices'));
  }
}

