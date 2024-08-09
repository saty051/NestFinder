import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm, NgModel } from '@angular/forms';
import { Router } from '@angular/router';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { IPropertyBase } from 'src/app/model/IPropertyBase';

@Component({
  selector: 'app-add-property',
  templateUrl: './add-property.component.html',
  styleUrls: ['./add-property.component.css']
})
export class AddPropertyComponent implements OnInit {


  @ViewChild('apform')
  addPropertyForm!: NgModel;
  @ViewChild('formTabs')
  formTabs?: TabsetComponent;

  // will come from master table of Database
  propertyTypes: Array<string> = ['House', 'Apartment', 'Duplex'];
  furnishTypes: Array<string> = ['Fully', 'Semi', 'Unfurnished'];
  propertyView: IPropertyBase = {
    Id: null,
    SellRent: null,
    Name: '',
    PType: null,
    FType: null,
    Price: null,
    BHK: null,
    City: '',
    RTM: null,
    BuiltArea: null
  };

  // Use Type Assertion : Tell TypeScript to treat propertyView as of type IProperty, even if it's initially an empty object.
  // This is known as type assertion.
  //propertyView = {} as IProperty;

  // Initialize `propertyView` Later : If you know that `propertyView` will be set later,
  // you can initialize it as `undefined` and add a check before passing it to the `app-property-card` component.
  //propertyView?: IProperty; // Initialized as undefined



  constructor(private router: Router) { }

  ngOnInit() {
    
  }

  onBack()
  {
    this.router.navigate(['/']);
  }

  OnSubmit(){
    console.log("Congrats");
    console.log("SellRent=" + this.addPropertyForm.value.BasicInfo.SellRent);
    console.log(this.addPropertyForm);
  }

  selectTab(tabId: number) {
    if (this.formTabs?.tabs[tabId]) {
      this.formTabs.tabs[tabId].active = true;
    }
  }
}
