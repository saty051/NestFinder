/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LikeService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) {}

  // Add like to a property
  addLike(propertyId: number): Observable<any> {
    const token = localStorage.getItem('token');
    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: token ? `Bearer ${token}` : ''
      }),
      responseType: 'text' as 'json' // Specify response type as text
    };
    return this.http.post(`${this.baseUrl}Like/${propertyId}`, {}, httpOptions);
  }

  // Remove like from a property using propertyId
  removeLike(propertyId: number): Observable<any> {
    const token = localStorage.getItem('token');
    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: token ? `Bearer ${token}` : ''
      }),
      responseType: 'text' as 'json' // Specify response type as text
    };
    return this.http.delete(`${this.baseUrl}Like/${propertyId}`, httpOptions);
  }

  // Check if the property is liked by the current user
  isPropertyLiked(propertyId: number): Observable<boolean> {
    const token = localStorage.getItem('token');
    if (!token) {
      return new Observable(observer => {
        observer.next(false);
        observer.complete();
      });
    }

    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: token ? `Bearer ${token}` : ''
      })
    };
    return this.http.get<boolean>(`${this.baseUrl}Like/check/${propertyId}`, httpOptions);
  }

  // Get total likes for a property
  getLikesForProperty(propertyId: number): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}Like/property/${propertyId}`);
  }
}
