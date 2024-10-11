/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable prefer-const */
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from './alertify.service';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorInterceptorService implements HttpInterceptor {
  constructor(private alertify: AlertifyService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<any> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = this.setError(error);
        
        // Displaying the error using Alertify
        this.alertify.error(errorMessage);
        console.error('HTTP Error:', errorMessage, error);

        return throwError(() => new Error(errorMessage));
      })
    );
  }

  private setError(error: HttpErrorResponse): string {
    let errorMessage = 'An unknown error occurred';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Handle plain text responses
      if (typeof error.error === 'string') {
        errorMessage = error.error;
      } else {
        switch (error.status) {
          case 401:
            errorMessage = 'You are not authorized to perform this action.';
            break;
          case 400:
            errorMessage = 'Bad request. Please check your input and try again.';
            break;
          default:
            errorMessage = 'A server error occurred. Please try again later.';
            break;
        }
      }
    }

    return errorMessage;
  }
}
