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
          // Customize the error log, avoiding unnecessary details like statusText: OK
          console.log(`Error: ${error.status} - ${error.message}`);

          // Handle network errors (e.g., server unreachable)
          if (error.status === 0) {
            this.alertify.error('Server is not reachable. Please try again later.');
          } else if (error.status === 401) {
            // Handle 401 Unauthorized errors
            const apiErrorMessage = error.error?.errorMessage || 'Unauthorized access. Please check your login credentials.';
            this.alertify.error(apiErrorMessage);  // Display the error message
          } else {
            // Display any other errors
            this.alertify.error(this.setError(error));
          }

          // Return the error so it can be handled by the caller
          return throwError(this.setError(error));
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
      if (error.status === 401 && error.error?.errorMessage) {
        // For 401 errors, return the server-provided message directly
        return error.error.errorMessage;
      } else {
        // General error message handling for other statuses
        errorMessage = error.error?.errorMessage || `Error Code: ${error.status}\nMessage: ${error.message}`;
      }
    }

    return errorMessage;
  }
}
