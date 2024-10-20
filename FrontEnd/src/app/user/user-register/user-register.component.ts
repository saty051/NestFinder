/* eslint-disable @angular-eslint/use-lifecycle-interface */
import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { UserForRegister } from 'src/app/model/user';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';


@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent {
  registrationForm!: FormGroup;
  user!: UserForRegister;
  userSubmitted!: boolean;

  constructor(
    private fb: FormBuilder, 
    private authService: AuthService, 
    private alertify: AlertifyService,
    private router: Router
  ) { }
  
  ngOnInit(){
    this.registrationForm = this.fb.group({
      username: ['',[Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmpassword: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required, Validators.maxLength(10)]],
      telegramId: ['', [Validators.required]]
    }, {validators: this.passwordMismatchValidator})
  }

  passwordMismatchValidator(fg: FormGroup): Validators| null{
    return fg.get('password')?.value === fg.get('confirmpassword')?.value ? null : {mismatch: true}
  }

  onSubmit(){
    console.log(this.registrationForm.value);
    this.userSubmitted = true;

    if(this.registrationForm.valid){
     // this.user = Object.assign(this.user,this.registrationForm.value);
      this.authService.registerUser(this.UserData()).subscribe(() =>
        {
          this.onReset();
          this.alertify.success('Congratulations, you are successfully registered!');
          this.router.navigate(['/user/login']);
        });
    }
  }

  onReset(){
    this.userSubmitted = false;
    this.registrationForm.reset();
  }

  UserData(): UserForRegister{
    return this.user = {
      userName: this.username.value,
      email: this.email.value,
      password: this.password.value,
      phoneNumber: this.phoneNumber.value,
      telegramId: this.telegramId.value
    };
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
  get phoneNumber(){
    return this.registrationForm.get('phoneNumber') as FormControl;
  }
  get telegramId(){
    return this.registrationForm.get('telegramId') as FormControl;
  }
}
