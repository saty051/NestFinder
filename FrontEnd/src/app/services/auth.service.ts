/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { UserForLogin } from '../model/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) {}

  // Method to decode JWT and get user information
  getUserId(): number {
    const token = this.getToken();
    if (!token) return 0;

    const decodedToken: any = JSON.parse(atob(token.split('.')[1])); // Decodes the JWT token
    return parseInt(decodedToken.nameid, 10); // Assuming `nameid` contains the user's ID
  }

  authUser(user: { userName: string; password: string }): Observable<UserForLogin> {
    return this.http.post<UserForLogin>(`${this.baseUrl}Account/login`, user);
  }

  registerUser(userData: any): Observable<any> {
  return this.http.post(`${this.baseUrl}Account/register`, userData);
}


  loggedIn() {
    const token = this.getToken();
    return !!token; // Return true if there is a token
  }

  // Method to clear user data on logout or session expiration
  clearUserData() {
    localStorage.removeItem('token');
    localStorage.removeItem('userName');
  }

  // Method to get token from localStorage
  getToken(): string | null {
    return localStorage.getItem('token');
  }
}
