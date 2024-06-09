import { Component } from '@angular/core';

@Component({
  selector: 'app-property-list',
  templateUrl: './property-list.component.html',
  styleUrls: ['./property-list.component.css']
})
export class PropertyListComponent {

  properties: Array<any> = [
    {
      "Id": 1,
      "Name": "Birla House",
      "Type": "House",
      "Price": 12000
    },
    {
      "Id": 2,
      "Name": "Sunset Villa",
      "Type": "Villa",
      "Price": 25000
    },
    {
      "Id": 3,
      "Name": "Maple Apartments",
      "Type": "Apartment",
      "Price": 15000
    },
    {
      "Id": 4,
      "Name": "Oakwood Cottage",
      "Type": "Cottage",
      "Price": 18000
    },
    {
      "Id": 5,
      "Name": "Seaside Bungalow",
      "Type": "Bungalow",
      "Price": 30000
    },
    {
      "Id": 6,
      "Name": "Mountain Retreat",
      "Type": "Retreat",
      "Price": 35000
    },
    {
      "Id": 7,
      "Name": "Urban Loft",
      "Type": "Loft",
      "Price": 20000
    }
  ]
}
