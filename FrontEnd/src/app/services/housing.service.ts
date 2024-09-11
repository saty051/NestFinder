import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { Property } from '../model/property';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HousingService {

  constructor(private http: HttpClient) { }

    baseUrl = environment.baseUrl;
  getAllCities(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}City`);
  }

  getProperty(id: number) {
    return this.getAllProperties(1).pipe(
      map(propertiesArray => {
        // throw new Error("Some error");
        return propertiesArray.find(property => property.id === id) as Property; 
      })
    );
  }

  getAllProperties(SellRent?: number): Observable<Property[]> {
    if(SellRent === undefined)
      return of([]);  // Handle undefined SellRent by returning an empty array
    return this.http.get<Property[]>(`${this.baseUrl}Property/list/${SellRent.toString()}`);
  }

  addProperty(property: Property) {
    let newProp: Property[] = [property];

    if (localStorage.getItem('newProp')) {
      newProp = [property, ...JSON.parse(localStorage.getItem('newProp') || '{}')];
    }
    localStorage.setItem('newProp', JSON.stringify(newProp));
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
}
