import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { IPropertyBase } from 'src/app/model/Ipropertybase';
import { Property } from 'src/app/model/property';
import { HousingService } from 'src/app/services/housing.service';
import { AlertifyService } from 'src/app/services/alertify.service';

@Component({
  selector: 'app-add-property',
  templateUrl: './add-property.component.html',
  styleUrls: ['./add-property.component.css']
})
export class AddPropertyComponent implements OnInit {
  @ViewChild('formTabs') formTabs!: TabsetComponent;
  addPropertyForm!: FormGroup;
  nextClicked!: boolean;
  property = new Property();

  // will come from masters
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

  constructor(private router: Router, private fb: FormBuilder, private housingService: HousingService, private alertify: AlertifyService) {}

  ngOnInit() {
    this.CreateAddPropertyForm();
    this.housingService.getAllCities().subscribe(data=>{
      console.log(data);
    })
  }

  CreateAddPropertyForm(){
    this.addPropertyForm = this.fb.group({
      BasicInfo: this.fb.group({
        SellRent: [null, Validators.required],
        BHK: [null, Validators.required],
        PType: [null, Validators.required],
        FType: [null, Validators.required],
        Name: [null, Validators.required],
        City: [null, Validators.required]
      }),
      PriceInfo: this.fb.group({
        Price: [null, Validators.required],
        BuiltArea: [null, Validators.required],
        CarpetArea: [null],
        Security: [null],
        Maintenance: [null]
      }),
      AddressInfo: this.fb.group({
        FloorNo: [null],
        TotalFloor: [null],
        Address: [null, Validators.required],
        LandMark: [null]
      }),
      OtherInfo: this.fb.group({
        RTM: [null, Validators.required],
        PossessionOn: [null],
        AOP: [null],
        Gated: [null],
        MainEntrance: [null],
        Description: [null]
      })
    });
  }

  //#region <Getter Methods>
  //#region <FormGroups
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

  //#endregion

  // #region <FormControl>
  get SellRent(){
    return this.BasicInfo.controls['SellRent'] as FormControl;
  }
  
  get BHK(){
    return this.BasicInfo.controls['BHK'] as FormControl;
  }
  
  get PType(){
    return this.BasicInfo.controls['PType'] as FormControl;
  }
  
  get FType(){
    return this.BasicInfo.controls['FType'] as FormControl;
  }
  
  get Name(){
    return this.BasicInfo.controls['Name'] as FormControl;
  }
  
  get City(){
    return this.BasicInfo.controls['City'] as FormControl;
  }
  
  get Price(){
    return this.PriceInfo.controls['Price'] as FormControl;
  }
  
  get BuiltArea(){
    return this.PriceInfo.controls['BuiltArea'] as FormControl;
  }
  
  get CarpetArea(){
    return this.PriceInfo.controls['CarpetArea'] as FormControl;
  }
  
  get Security(){
    return this.PriceInfo.controls['Security'] as FormControl;
  }
  
  get Maintenance(){
    return this.PriceInfo.controls['Maintenance'] as FormControl;
  }
  
  get FloorNo(){
    return this.AddressInfo.controls['FloorNo'] as FormControl;
  }
  
  get TotalFloor(){
    return this.AddressInfo.controls['TotalFoor'] as FormControl;
  }
  
  get Address(){
    return this.AddressInfo.controls['Address'] as FormControl;
  }
  
  get LandMark(){
    return this.AddressInfo.controls['LandMark'] as FormControl;
  }
  
  get RTM(){
    return this.OtherInfo.controls['RTM'] as FormControl;
  }
  
  get PossessionOn(){
    return this.OtherInfo.controls['PossessionOn'] as FormControl;
  }
  
  get AOP(){
    return this.OtherInfo.controls['AOP'] as FormControl;
  }
  
  get Gated(){
    return this.OtherInfo.controls['Gated'] as FormControl;
  }
  
  get MainEntrance(){
    return this.OtherInfo.controls['MainEntrance'] as FormControl;
  }
  
  get Description(){
    return this.OtherInfo.controls['Description'] as FormControl;
  }
  
  //#endregion

  allTabsValid(): boolean{
    if(this.BasicInfo.invalid){
      this.formTabs.tabs[0].active = true;
      return false;
    }

    if(this.PriceInfo.invalid){
      this.formTabs.tabs[1].active = true;
      return false;
    }

    if(this.AddressInfo.invalid){
      this.formTabs.tabs[2].active = true;
      return false;
    }

    if(this.OtherInfo.invalid){
      this.formTabs.tabs[3].active = true;
      return false;
    }
    return true;
  }
  onSubmit() {
    this.nextClicked = true;
    if (this.allTabsValid()) {
      this.mapProperty();
      this.housingService.addProperty(this.property);
      this.alertify.success("Congrats, your property listed successfully on our website");
      console.log(this.addPropertyForm.value);
    }
    else{
      this.alertify.error("Please review the form and provide all valid enteries");
    }

    if(this.SellRent.value==="2"){
      this.router.navigate(['/rent-property']);
    }
    else{
      this.router.navigate(['/']);
    }
  }

  mapProperty(): void {
    this.property.Id = this.housingService.newPropID();
    if (this.SellRent) this.property.SellRent = +this.SellRent.value || 0;
    if (this.BHK) this.property.BHK = this.BHK.value || 0;
    if (this.PType) this.property.PType = this.PType.value || '';
    if (this.Name) this.property.Name = this.Name.value || '';
    if (this.City) this.property.City = this.City.value || '';
    if (this.FType) this.property.FType = this.FType.value || '';
    if (this.Price) this.property.Price = this.Price.value || 0;
    if (this.Security) this.property.Security = this.Security.value || 0;
    if (this.Maintenance) this.property.Maintenance = this.Maintenance.value || 0;
    if (this.BuiltArea) this.property.BuiltArea = this.BuiltArea.value || 0;
    if (this.CarpetArea) this.property.CarpetArea = this.CarpetArea.value || 0;
    if (this.FloorNo) this.property.FloorNo = this.FloorNo.value || 0;
    if (this.TotalFloor) this.property.TotalFloor = this.TotalFloor.value || 0  ;
    if (this.Address) this.property.Address = this.Address.value || '';
    if (this.LandMark) this.property.Address2 = this.LandMark.value || '';
    if (this.RTM) this.property.RTM = this.RTM.value || '';
    if (this.AOP) this.property.AOP = this.AOP.value || '';
    if (this.Gated) this.property.Gated = this.Gated.value || '';
    if (this.MainEntrance) this.property.MainEntrance = this.MainEntrance.value || '';
    if (this.PossessionOn) this.property.PossessionOn = this.PossessionOn.value || '';
    if (this.Description) this.property.Description = this.Description.value || '';
    this.property.PostedOn = new Date().toString();
}


  selectTab(NextTabId: number, IsCurrentTabValid: boolean) {
    this.nextClicked = true;
    if (this.formTabs && IsCurrentTabValid) {
      this.formTabs.tabs[NextTabId].active = true;
    }
  }
}