/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { IPropertyBase } from 'src/app/model/Ipropertybase';
import { Property } from 'src/app/model/property';
import { HousingService } from 'src/app/services/housing.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { Ikeyvaluepair } from 'src/app/model/Ikeyvaluepair';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-add-property',
  templateUrl: './add-property.component.html',
  styleUrls: ['./add-property.component.css']
})
export class AddPropertyComponent implements OnInit {
  @ViewChild('formTabs', { static: false }) formTabs!: TabsetComponent;
  addPropertyForm!: FormGroup;
  nextClicked = false;
  property = new Property();

  // List of cities
  cityList: any[] = [];

  // Will come from masters
  propertyTypes!: Ikeyvaluepair[];
  furnishTypes!: Ikeyvaluepair[];

  propertyView: IPropertyBase = {
    id: null,
    sellRent: null,
    name: '',
    propertyType: null,
    furnishingType: null,
    price: null,
    bhk: null,
    city: '',
    readyToMove: false,
    builtArea: null
  };

  constructor(
    private datePipe: DatePipe,
    private router: Router,
    private fb: FormBuilder,
    private housingService: HousingService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    if(!localStorage.getItem('userName')){
      this.alertify.error('You must be logged in to add a property!');
      this.router.navigate(['/user/login']);
    }

    this.CreateAddPropertyForm();
    this.housingService.getAllCities().subscribe(data => {
      this.cityList = data;
    });

    this.housingService.getPropertyTypes().subscribe(data => {
      this.propertyTypes = data;
    });

    this.housingService.getfurnishingTypes().subscribe(data => {
      this.furnishTypes = data;
    });
  }

  CreateAddPropertyForm() {
    this.addPropertyForm = this.fb.group({
      BasicInfo: this.fb.group({
        SellRent: [1, Validators.required],
        BHK: [null, Validators.required],
        PType: [null, Validators.required],
        FType: [null, Validators.required],
        Name: [null, Validators.required],
        City: ['', Validators.required]
      }),
      PriceInfo: this.fb.group({
        Price: [null, Validators.required],
        BuiltArea: [null, Validators.required],
        CarpetArea: [null],
        Security: [0],
        Maintenance: [0]
      }),
      AddressInfo: this.fb.group({
        FloorNo: [null],
        TotalFloor: [null],
        Address: [null, Validators.required],
        LandMark: [null]
      }),
      OtherInfo: this.fb.group({
        RTM: [null, Validators.required],
        PossessionOn: [null, Validators.required],
        AOP: [null],
        Gated: [null],
        MainEntrance: [null],
        Description: [null]
      })
    });
  }

  get BasicInfo() {
    return this.addPropertyForm.get('BasicInfo') as FormGroup;
  }

  get PriceInfo() {
    return this.addPropertyForm.get('PriceInfo') as FormGroup;
  }

  get AddressInfo() {
    return this.addPropertyForm.get('AddressInfo') as FormGroup;
  }

  get OtherInfo() {
    return this.addPropertyForm.get('OtherInfo') as FormGroup;
  }

  // Form Control Getters (BasicInfo)
  get SellRent() { return this.BasicInfo.controls['SellRent'] as FormControl; }
  get BHK() { return this.BasicInfo.controls['BHK'] as FormControl; }
  get PType() { return this.BasicInfo.controls['PType'] as FormControl; }
  get FType() { return this.BasicInfo.controls['FType'] as FormControl; }
  get Name() { return this.BasicInfo.controls['Name'] as FormControl; }
  get City() { return this.BasicInfo.controls['City'] as FormControl; }

  // Form Control Getters (PriceInfo)
  get Price() { return this.PriceInfo.controls['Price'] as FormControl; }
  get BuiltArea() { return this.PriceInfo.controls['BuiltArea'] as FormControl; }
  get CarpetArea() { return this.PriceInfo.controls['CarpetArea'] as FormControl; }
  get Security() { return this.PriceInfo.controls['Security'] as FormControl; }
  get Maintenance() { return this.PriceInfo.controls['Maintenance'] as FormControl; }

  // Form Control Getters (AddressInfo)
  get FloorNo() { return this.AddressInfo.controls['FloorNo'] as FormControl; }
  get TotalFloor() { return this.AddressInfo.controls['TotalFloor'] as FormControl; }
  get Address() { return this.AddressInfo.controls['Address'] as FormControl; }
  get LandMark() { return this.AddressInfo.controls['LandMark'] as FormControl; }

  // Form Control Getters (OtherInfo)
  get RTM() { return this.OtherInfo.controls['RTM'] as FormControl; }
  get PossessionOn() { return this.OtherInfo.controls['PossessionOn'] as FormControl; }
  get AOP() { return this.OtherInfo.controls['AOP'] as FormControl; }
  get Gated() { return this.OtherInfo.controls['Gated'] as FormControl; }
  get MainEntrance() { return this.OtherInfo.controls['MainEntrance'] as FormControl; }
  get Description() { return this.OtherInfo.controls['Description'] as FormControl; }

  allTabsValid(): boolean {
    if (this.BasicInfo.invalid) {
      this.formTabs.tabs[0].active = true;
      return false;
    }

    if (this.PriceInfo.invalid) {
      this.formTabs.tabs[1].active = true;
      return false;
    }

    if (this.AddressInfo.invalid) {
      this.formTabs.tabs[2].active = true;
      return false;
    }

    if (this.OtherInfo.invalid) {
      this.formTabs.tabs[3].active = true;
      return false;
    }
    return true;
  }

  onSubmit() {
    this.nextClicked = true;
    if (this.allTabsValid()) {
      this.mapProperty();
      this.housingService.addProperty(this.property).subscribe({
        next: () => {
          this.alertify.success("Congrats, your property listed successfully on our website");
          console.log(this.addPropertyForm);
  
          if (this.SellRent.value === "2") {
            this.router.navigate(['/rent-property']);
          } else {
            this.router.navigate(['/']);
          }
        },
        error: (error) => {
          this.alertify.error('An error occurred while listing the property.');
          console.error('Error during property creation:', error);
        }
      });
    } else {
      this.alertify.error("Please review the form and provide all valid entries");
    }
  }
  

  mapProperty(): void {
    this.property.id = this.housingService.newPropID();
    this.property.sellRent = +this.SellRent.value || 0;
    this.property.bhk = this.BHK.value || 0;
    this.property.propertyTypeId = this.PType.value || 0;
    this.property.furnishingTypeId = this.FType.value || 0;
    this.property.name = this.Name.value || '';
    this.property.CityId = this.City.value || 0;
    this.property.price = this.Price.value || 0;
    this.property.security = this.Security.value || 0;
    this.property.maintenance = this.Maintenance.value || 0;
    this.property.builtArea = this.BuiltArea.value || 0;
    this.property.carpetArea = this.CarpetArea.value || 0;
    this.property.floorNo = this.FloorNo.value || 0;
    this.property.totalFloors = this.TotalFloor.value || 0;
    this.property.address = this.Address.value || '';
    this.property.address2 = this.LandMark.value || '';
    this.property.readyToMove = this.RTM.value || false;
    this.property.mainEntrance = this.MainEntrance.value || '';
    this.property.gated = this.Gated.value || false;
    this.property.estPossessionOn = this.datePipe.transform(this.PossessionOn.value, 'yyyy-MM-dd') || '';
    this.property.description = this.Description.value || '';
  }

  selectTab(NextTabId: number, IsCurrentTabValid: boolean) {
    this.nextClicked = true;
    if (this.formTabs && IsCurrentTabValid) {
      this.formTabs.tabs[NextTabId].active = true;
    }
  }
}
