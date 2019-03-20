import { Component, Input } from '@angular/core';

import { PriceMovementService } from '../services/price-movement.service';
import { PortfolioRecord } from '../domain/portfolio-data';
import { PriceMovementRecord } from '../domain/price-movement';

@Component({
  selector: 'app-portfolio-details',
  templateUrl: './portfolio-details.component.html'
})
export class PortfolioComponent {
  @Input() public securityId: string;
  public gridData: PortfolioRecord[];

  constructor(private service: PriceMovementService) {
  }

  ngOnInit(): void {

    this.getPortfolios();
  }

  public getPortfolios(): void {

    console.log('onclick getPortfolios: ' + this.securityId);

    const portfolioList = this.service.getPortfolios(this.securityId);
    this.gridData = this.GetPorfolioData(portfolioList);
  }

  private GetPorfolioData(data: PriceMovementRecord[]): PortfolioRecord[] {

    const portfolioList: PortfolioRecord[] = [];
    let portfolio: PortfolioRecord;

    for (const i of data) {
      portfolio = new PortfolioRecord(i);
      portfolioList.push(portfolio);
    }

    console.log(portfolioList);
    return portfolioList;
  }
}
