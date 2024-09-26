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
    return next.handle(request)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          // Handle specific status codes and provide custom messages
          if (error.status === 401) {
            this.alertify.error('You are not authorized to perform this action.');
          } else if (error.status === 400) {
            this.alertify.error('You are not authorized to perform this action.');
          } else {
            // Display other errors
            this.alertify.error(this.setError(error));
          }

          // Return the error so it can be handled by the caller
          return throwError(() => new Error(this.setError(error)));
        })
      );
  }

  setError(error: HttpErrorResponse): string {
    let errorMessage = 'An unknown error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = error.error?.message || 'A server error occurred. Please try again later.';
    }

    return errorMessage;
  }
}
