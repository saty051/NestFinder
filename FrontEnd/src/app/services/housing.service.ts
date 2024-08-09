import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { IPropertyBase } from '../model/IPropertyBase';

@Injectable({
  providedIn: 'root'
})
export class HousingService {

  constructor(private http:HttpClient) { }

  getAllProperties(SellRent:number): Observable<IPropertyBase[]>{
    return this.http.get<IPropertyBase[]>('assets/data/properties.json').pipe(
      map(data => {
        const propertiesArray: Array<IPropertyBase> = [];
        for(const id in data){
            if(data.hasOwnProperty(id)&& data[id].SellRent === SellRent){
                propertiesArray.push(data[id]);
              }
            }
        return propertiesArray;
      }
    )
    )
  }
}
