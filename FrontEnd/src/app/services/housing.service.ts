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

  getAllCities(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}City`);
  }

  getPropertyTypes(): Observable<Ikeyvaluepair[]> {
    return this.http.get<Ikeyvaluepair[]>(`${this.baseUrl}Propertytype/list`);
  }

  getfurnishingTypes(): Observable<Ikeyvaluepair[]> {
    return this.http.get<Ikeyvaluepair[]>(`${this.baseUrl}furnishingtype/list`);
  }

  // Add the missing getProperty method
  getProperty(id: number): Observable<Property> {
    return this.http.get<Property>(`${this.baseUrl}Property/detail/${id.toString()}`);
  }

  getAllProperties(SellRent?: number): Observable<Property[]> {
    if(SellRent === undefined)
      return of([]);  // Handle undefined SellRent by returning an empty array
    return this.http.get<Property[]>(`${this.baseUrl}Property/list/${SellRent.toString()}`);
  }

  addProperty(property: Property) {
    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + localStorage.getItem('token')
      })
    };
    return this.http.post(`${this.baseUrl}Property/add`, property, httpOptions);
  }

  newPropID() {
    const currentPID = localStorage.getItem('PID'); // Get the value from localStorage

    if (currentPID) { // Check if currentPID is truthy (not null or undefined)
        const newPID = +currentPID + 1; // Increment the PID
        localStorage.setItem('PID', String(newPID)); // Store the incremented PID
        return newPID; // Return the incremented value
    } else {
        localStorage.setItem('PID', '101'); // Set the default PID if it was null
        return 101; // Return the default value
    }
  }

  getPropertyAge(dateofEstablishment: string): string {
    const today = new Date();
    const estDate = new Date(dateofEstablishment);

    // If the establishment date is in the future
    if (today < estDate) {
      return '0';
    }

    let age = today.getFullYear() - estDate.getFullYear();
    const monthDifference = today.getMonth() - estDate.getMonth();

    // If the current month is before the establishment month, or it's the same month but today's date is earlier
    if (monthDifference < 0 || (monthDifference === 0 && today.getDate() < estDate.getDate())) {
      age--;
    }

    // Age is less than a year
    if (age === 0) {
      return "Less than a year";
    }

    return age.toString();
  }

  setPrimaryPhoto(propertyId: number, propertyPhotoId: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + localStorage.getItem('token')
      })
    };
    return this.http.post(`${this.baseUrl}Property/set-primary-photo/${propertyId.toString()}/${propertyPhotoId}`, {}, httpOptions);
  }
}
