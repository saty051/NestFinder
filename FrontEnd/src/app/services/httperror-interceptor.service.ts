import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { catchError, concatMap, delay, Observable, of, retryWhen, throwError } from 'rxjs';
import { AlertifyService } from './alertify.service';
import { ErrorCode } from '../Enums/enums';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorInterceptorService implements HttpInterceptor { // Implement HttpInterceptor interface
  constructor(private alertify: AlertifyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    console.log('Intercepting request...');
    return next.handle(request).pipe(
      retryWhen(error => this.retryRequest(error, )),
      catchError((error: HttpErrorResponse) => {
        const errorMessage = this.setError(error);
        console.log(error);
        this.alertify.error(errorMessage);
        return throwError(() => new Error(errorMessage)); // Use throwError with a new Error instance
      })
    );
  }
// Retry the requests in case of errors
retryRequest(error: Observable<unknown>, retryCount: number): Observable<unknown> {
  return error.pipe(
    concatMap((err: unknown, count: number) => {
      const checkErr = err as HttpErrorResponse; // Explicit cast to HttpErrorResponse
      if(count <= retryCount){
        switch(checkErr.status)
        {
          case ErrorCode.serverDown:
            return of(checkErr).pipe(delay(1000));

          case ErrorCode.unauthorised: 
            return of(checkErr).pipe(delay(1000));
        }
    }
    return throwError(checkErr);
    })
  );
}

  setError(error: HttpErrorResponse): string{
    let errorMessage = 'Unknown error occured';
    if(error.error instanceof ErrorEvent){
      // Client side error
      errorMessage = error. error.message;
    }else{
      // server side error
      if(error.status!==0){
        errorMessage = error.error.errorMessage;
      }
    }
    return errorMessage;
  } 
}
