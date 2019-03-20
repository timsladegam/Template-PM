import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { map } from 'rxjs/operators';
import { of } from 'rxjs';

import { DataService } from '../services/data.service';
import { PriceMovementRecord } from '../domain/price-movement';
import { YieldPointRecord } from '../domain//yield-point';
import { StaleYieldPointRecord } from '../domain//stale-yield-point';
import { UnderlyingRecord } from '../domain//underlying';
import { DropDowns } from '../domain//dropdowns';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class PriceMovementService {
  private url: string;
  private httpClient: HttpClient;
  private resultCache: PriceMovementRecord[];
  private reportDate: string;
  private assetClasses: string[] = [];
  private currencies: string[] = [];
  private dealingDesks: string[] = [];
  private recordType = '';
  private dataUrl = 'api/PriceMovement/Data';
  private staleUrl = 'api/PriceMovement/Stale';
  private underlyingUrl = 'api/PriceMovement/Underlying';
  private yieldUrl = 'api/PriceMovement/YieldPoint';

  private dropdownUrl = 'api/PriceMovement/DropDownData';

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private dataService: DataService) {
    console.log('service: ' + baseUrl);
    this.httpClient = http;
    this.url = baseUrl;
  }

  // get price movement records
  public getPriceMovement(force: boolean = false): Observable<PriceMovementRecord[]> {

    console.log('getPriceMovements: force = ' + force);

    console.log('reportDate: ' + this.reportDate + ' : ' + this.dataService.forDate);
    console.log('assetClasses: ' + JSON.stringify(this.assetClasses) + ' : ' + JSON.stringify(this.dataService.selectedAssetClassList));
    console.log('currencies: ' + JSON.stringify(this.currencies) + ' : ' + JSON.stringify(this.dataService.selectedCurrencyList));
    console.log('dealingDesks: ' + JSON.stringify(this.dealingDesks) + ' : ' + JSON.stringify(this.dataService.selectedDealingDeskList));
    console.log('recordType: ' + this.recordType + ' : ' + this.dataService.recordType);

    // check if we can use cached data
    if (!force &&
      this.reportDate === this.dataService.forDate &&
      JSON.stringify(this.assetClasses) === JSON.stringify(this.dataService.selectedAssetClassList) &&
      JSON.stringify(this.currencies) === JSON.stringify(this.dataService.selectedCurrencyList) &&
      JSON.stringify(this.dealingDesks) === JSON.stringify(this.dataService.selectedDealingDeskList) &&
      this.recordType === this.dataService.recordType) {

      console.log('getPriceMovements: using cached results ...');
      return of(this.resultCache);
    }

    // build http post payload
    const data = this.buildPayload();

    console.log('getPriceMovements: getting data ... ' + data);

    const result = this.httpClient.post<PriceMovementRecord[]>(`${this.url}${this.dataUrl}`, data, httpOptions)
      .pipe(map(response => {
        console.log('getPriceMovements: retrieved data ... ' + response.length + ' records');
        this.resultCache = response.map(data2 => new PriceMovementRecord(data2));
        this.reportDate = this.dataService.forDate;
        this.assetClasses = this.dataService.selectedAssetClassList.slice(0);
        this.currencies = this.dataService.selectedCurrencyList.slice(0);
        this.dealingDesks = this.dataService.selectedDealingDeskList.slice(0);
        this.recordType = this.dataService.recordType;
        return this.resultCache;
      }));

    return result;
  }

  // extract portfolios from the cached results
  public getPortfolios(securityId: string): PriceMovementRecord[] {
    const portfolios = this.resultCache.filter(r => r.securityId === securityId);
    console.log('found portfolios: ' + portfolios);
    return portfolios;
  }

  // get drop down data
  public getDropDownData(): Observable<DropDowns> {

    console.log('getDropDownData');

    const result3 = this.httpClient.get<DropDowns>(`${this.url}${this.dropdownUrl}`)
      .map(response => {
        const reply = this.getData(response);
        return reply;
      });

    return result3;
  }

  // extract data for drop downs
  private getData(dropDownData: DropDowns): DropDowns {
    return new DropDowns(dropDownData);
  }

  // get underlying records
  public getUnderlyingMovement(): Observable<UnderlyingRecord[]> {

    console.log('getUnderlyingMovement');

    // build http post payload
    const payload = this.buildPayload();

    // request for result
    const result = this.httpClient.post<UnderlyingRecord[]>(`${this.url}${this.underlyingUrl}`, payload, httpOptions)
      .pipe(map(response => {
        const details = response.map(data => new UnderlyingRecord(data));
        return details;
      }));

    return result;
  }

  // get stale movement records
  public getStaleMovement(): Observable<StaleYieldPointRecord[]> {

    console.log('getStaleMovement');

    // build http post payload
    const payload = this.buildPayload();

    // request for result
    const result = this.httpClient.post<StaleYieldPointRecord[]>(`${this.url}${this.staleUrl}`, payload, httpOptions)
      .pipe(map(response => {
        const details = response.map(data => new StaleYieldPointRecord(data));
        return details;
      }));

    return result;
  }

  // get yield point records
  public getYieldPointMovement(): Observable<YieldPointRecord[]> {

    console.log('getYieldPointMovement');

    // build http post payload
    const payload = this.buildPayload();

    // request for result
    const result = this.httpClient.post<YieldPointRecord[]>(`${this.url}${this.yieldUrl}`, payload, httpOptions)
      .pipe(map(response => {
        const details = response.map(data => new YieldPointRecord(data));
        return details;
      }));

    return result;
  }

  // build payload
  private buildPayload(): any {

    const forDate = this.dataService.forDate;
    const selectedAssetClasses = this.dataService.selectedAssetClassList.join(',');
    const selectedCurrencies = this.dataService.selectedCurrencyList.join(',');
    const selectedDealingDesks = this.dataService.selectedDealingDeskList.join(',');
    const recordType = this.dataService.recordType;

    const data: any = JSON.stringify({
      forDate: forDate,
      assetClasses: selectedAssetClasses,
      currencies: selectedCurrencies,
      dealingDesks:
        selectedDealingDesks,
      recordType: recordType
    });

    return data;
  }
}
