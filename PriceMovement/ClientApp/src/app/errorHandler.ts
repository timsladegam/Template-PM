import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor } from '@angular/common/http';
import { HttpHandler, HttpRequest, HttpErrorResponse, HttpClient, HTTP_INTERCEPTORS  } from '@angular/common/http';
import { Observable, EMPTY, throwError, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private http: HttpClient) {}

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse) => {

        if (err.error instanceof Error) {
          // A client-side or network error occurred. Handle it accordingly.
          console.error('An error occurred:', err.error.message);
        } else {
          // The backend returned an unsuccessful response code.
          // The response body may contain clues as to what went wrong,
          console.error(`Backend returned code ${err.status}, body was: ${err.error}`);
        }

        this.sendToServer(err.error);

        // ...optionally return a default fallback value so app can continue (pick one)
        // which could be a default value (which has to be a HttpResponse here)
        // return Observable.of(new HttpResponse({body: [{name: "Default value..."}]}));
        // or simply an empty observable
        return EMPTY;
      })
    );
  }

  private sendToServer(error: any): void {

    this.http.post('/api/PriceMovement/logClient',
        {
          type: error.name || error.status,
          message: error.message || error.statusText,
          stack: error.stack || error._body,
          location: window.location.href
        }
      ).subscribe();  // fire and forget post, nothing to do here.
  }
}
