import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent {
  registrationForm!: FormGroup;
  constructor(private fb: FormBuilder){ }

  ngOnInit(){
    this.registrationForm = this.fb.group({
      username: ['',[Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmpassword: ['', [Validators.required]],
      mobile: ['', [Validators.required, Validators.maxLength(10)]]
    }, {validators: this.passwordMismatchValidator})
  }

  passwordMismatchValidator(fg: FormGroup): Validators| null{
    return fg.get('password')?.value === fg.get('confirmpassword')?.value ? null : {mismatch: true}
  }
// ------------------------------------
// Getter methods for all form Control
// ------------------------------------

  get username(){
    return this.registrationForm.get('username') as FormControl;
  }
  get email(){
    return this.registrationForm.get('email') as FormControl;
  }
  get password(){
    return this.registrationForm.get('password') as FormControl;
  }
  get confirmpassword(){
    return this.registrationForm.get('confirmpassword') as FormControl;
  }
  get mobile(){
    return this.registrationForm.get('mobile') as FormControl;
  }

  onSubmit(){
    console.log(this.registrationForm)
  }

}
