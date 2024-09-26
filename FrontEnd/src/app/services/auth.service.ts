/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) {}

  // Method to decode JWT and get user information
  getUserId(): number {
    const token = localStorage.getItem('token');
    console.log('JWT Token:', token); // Check if the token exists
    if (!token) return 0;

    const decodedToken: any = JSON.parse(atob(token.split('.')[1]));
    console.log('Decoded Token:', decodedToken);  // Log the decoded token
    return decodedToken.nameid;  // Assuming `nameid` contains the user's ID
  }

  authUser(user: any): Observable<any> {
    return this.http.post(`${this.baseUrl}Account/login`, user);
  }

  registerUser(user: any): Observable<any> {
    return this.http.post(`${this.baseUrl}Account/register`, user);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token; // Return true if there is a token
  }
}
