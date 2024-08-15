
import { Component, OnInit } from '@angular/core';
import { HousingService } from 'src/app/services/housing.service';
import { IPropertyBase } from 'src/app/model/Ipropertybase';
import { Action } from 'rxjs/internal/scheduler/Action';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-property-list',
  templateUrl: './property-list.component.html',
  styleUrls: ['./property-list.component.css']
})
export class PropertyListComponent implements OnInit{
  properties! : IPropertyBase[];
  SellRent = 1;

  City = '';
  SearchCity = '';
  SortbyParam = '';
  SortOrder= 'asc';

  constructor(private route:ActivatedRoute,private housingService:HousingService){}

  ngOnInit(): void {
    if(this.route.snapshot.url.toString()){
      this.SellRent = 2; // Means we are on rent-property URL else we are on base URL
    }
    this.housingService.getAllProperties(this.SellRent).subscribe(
      data=>{this.properties = data;
        
      console.log(data);
      console.log(this.route.snapshot.url.toString());  
      },
      error=>{
        console.error(error);
      }
    );
  }
  onCityFilter(){
    this.SearchCity = this.City;
  }

  onCityFilterClear(){
    this.SearchCity = '';
    this.City= '';
  }

  onSortDirection(){
    if(this.SortOrder === 'desc'){
      this.SortOrder = 'asc';
    }
    else{
      this.SortOrder = 'desc';
    }
  }
}
