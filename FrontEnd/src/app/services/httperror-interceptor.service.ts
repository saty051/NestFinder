import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { AlertifyService } from './alertify.service';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorInterceptorService implements HttpInterceptor { // Implement HttpInterceptor interface
  constructor(private alertify: AlertifyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    console.log('Intercepting request...');
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        const errorMessage = this.setError(error);
        console.log(error);
        this.alertify.error(errorMessage);
        return throwError(() => new Error(errorMessage)); // Use throwError with a new Error instance
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
