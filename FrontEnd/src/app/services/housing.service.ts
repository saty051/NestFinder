import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Property } from '../model/property';
import { environment } from 'src/environments/environment';
import { Ikeyvaluepair } from '../model/Ikeyvaluepair';

@Injectable({
  providedIn: 'root'
})
export class HousingService {

  constructor(private http: HttpClient) { }

  baseUrl = environment.baseUrl;

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      Authorization: token ? `Bearer ${token}` : '',
      'X-Custom-Error': 'true'
    });
  }

  getAllCities(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}City`);
  }

  getPropertyTypes(): Observable<Ikeyvaluepair[]> {
    return this.http.get<Ikeyvaluepair[]>(`${this.baseUrl}Propertytype/list`);
  }

  getfurnishingTypes(): Observable<Ikeyvaluepair[]> {
    return this.http.get<Ikeyvaluepair[]>(`${this.baseUrl}furnishingtype/list`);
  }

  getProperty(id: number): Observable<Property> {
    return this.http.get<Property>(`${this.baseUrl}Property/detail/${id.toString()}`);
  }

  getAllProperties(SellRent?: number): Observable<Property[]> {
    if (SellRent === undefined)
      return of([]);  // Handle undefined SellRent by returning an empty array
    return this.http.get<Property[]>(`${this.baseUrl}Property/list/${SellRent.toString()}`);
  }

  addProperty(property: Property) {
    return this.http.post(`${this.baseUrl}Property/add`, property, { headers: this.getAuthHeaders() });
  }

  newPropID() {
    const currentPID = localStorage.getItem('PID'); 
    if (currentPID) { 
        const newPID = +currentPID + 1; 
        localStorage.setItem('PID', String(newPID));
        return newPID; 
    } else {
        localStorage.setItem('PID', '101');
        return 101; 
    }
  }

  getPropertyAge(dateofEstablishment: string): string {
    const today = new Date();
    const estDate = new Date(dateofEstablishment);
    if (today < estDate) {
      return '0';
    }

    let age = today.getFullYear() - estDate.getFullYear();
    const monthDifference = today.getMonth() - estDate.getMonth();
    if (monthDifference < 0 || (monthDifference === 0 && today.getDate() < estDate.getDate())) {
      age--;
    }

    if (age === 0) {
      return "Less than a year";
    }

    return age.toString();
  }

  setPrimaryPhoto(propertyId: number, propertyPhotoId: string) {
  return this.http.post(
    `${this.baseUrl}Property/set-primary-photo/${propertyId}/${propertyPhotoId}`,
    {},
    { headers: this.getAuthHeaders() }
  );
}

  deletePhoto(propertyId: number, propertyPhotoId: string) {
  return this.http.delete(`${this.baseUrl}Property/delete-photo/${propertyId}/${propertyPhotoId}`, {
    headers: this.getAuthHeaders()
  });
}

  getContactDetails(propertyId: number): Observable<{ email: string; phoneNumber: string }> {
    return this.http.get<{ email: string; phoneNumber: string }>(
      `${this.baseUrl}Property/contact/${propertyId}`
    );
  }
}
