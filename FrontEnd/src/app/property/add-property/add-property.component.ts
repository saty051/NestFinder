import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm, NgModel } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-property',
  templateUrl: './add-property.component.html',
  styleUrls: ['./add-property.component.css']
})
export class AddPropertyComponent implements OnInit {


  @ViewChild('apform')
  addPropertyForm!: NgModel;
  constructor(private router: Router) { }

  ngOnInit() {
    
  }

  onBack()
  {
    this.router.navigate(['/']);
  }

  OnSubmit(){
    console.log("Congrats");
    console.log(this.addPropertyForm);
  }

}
