import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { UserForLogin, UserForRegister } from '../model/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  authUser(user: UserForLogin): Observable<UserForLogin> {
    return this.http.post<UserForLogin>(`${this.baseUrl}/account/login`, user);
  }

  registerUser(user: UserForRegister): Observable<UserForRegister> {
    return this.http.post<UserForRegister>(`${this.baseUrl}/account/register`, user);
  }
}
