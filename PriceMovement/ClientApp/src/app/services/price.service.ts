import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { PriceRecord } from '../domain/price';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class PriceService {
  private readonly url: string;
  private readonly httpClient: HttpClient;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.url = baseUrl;
  }

  // get price movement records
  public getPrices(securityId: string, fromDate: string, dayCount: string): Observable<PriceRecord[]> {
    // form url
    const params = `?securityId=${securityId}&fromDate=${fromDate}&dayCount=${dayCount}`;

    // request for result
    const result = this.httpClient.get<PriceRecord[]>(`${this.url}api/PriceMovement/Prices${params}`)
      .map(response => {
        const details = response.map(data => new PriceRecord(data));
        return details;
      });

    return result;
  }
}
