/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from './alertify.service';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorInterceptorService implements HttpInterceptor {

  constructor(private alertify: AlertifyService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<any> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          this.alertify.error('You are not authorized to perform this action.');
        } else if (error.status === 400) {
          this.alertify.error('Bad request. Please check your input and try again.');
        } else {
          this.alertify.error(this.setError(error));
        }
        return throwError(() => new Error(this.setError(error)));
      })
    );
  }

  setError(error: HttpErrorResponse): string {
    let errorMessage = 'An unknown error occurred';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = error.error?.message || 'A server error occurred. Please try again later.';
    }
    return errorMessage;
  }
}
