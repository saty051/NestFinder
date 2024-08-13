import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { IPropertyBase } from '../model/Ipropertybase';
import { Property } from '../model/property';

@Injectable({
  providedIn: 'root'
})
export class HousingService {

  constructor(private http:HttpClient) { }

  getProperty(id: number){
    return this.getAllProperties().pipe(
      map(propertiesArray => {
        return propertiesArray.find(property => property.Id === id) as Property; 
      })
    );
  }

  getAllProperties(SellRent?:number): Observable<IPropertyBase[]>{
    return this.http.get<IPropertyBase[]>('assets/data/properties.json').pipe(
      map(data => {
        const propertiesArray: Array<IPropertyBase> = [];

        const localProperties = JSON.parse(localStorage.getItem('newProp') || '{}');
        if(localProperties){
          for(const id in localProperties){
            if(SellRent){
              if(localProperties.hasOwnProperty(id)&& localProperties[id].SellRent === SellRent){
                propertiesArray.push(localProperties[id]);
              }
            }else{
              propertiesArray.push(localProperties[id]);
            }
            
            }
        }

        for(const id in data){
          if(SellRent){
            if(data.hasOwnProperty(id)&& data[id].SellRent === SellRent){
              propertiesArray.push(data[id]);
            }
          }
          else{
            propertiesArray.push(data[id]);
          }
        }
        return propertiesArray;
      }
    )
    )
  }
  addProperty(property: Property){
    let newProp: Property[] = [property];

    if(localStorage.getItem('newProp')){
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
