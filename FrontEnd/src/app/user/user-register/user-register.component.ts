/* eslint-disable @angular-eslint/use-lifecycle-interface */
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { UserForRegister } from 'src/app/model/user';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { SecurityQuestion } from 'src/app/Enums/security-question.enum';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent {
  registrationForm!: FormGroup;
  user!: UserForRegister;
  userSubmitted!: boolean;
  securityQuestions: { value: SecurityQuestion; label: string }[] = [
    { value: SecurityQuestion.WhatIsYourFavoriteColor, label: 'What is your favorite color?' },
    { value: SecurityQuestion.WhatCityWereYouBornIn, label: 'What city were you born in?' },
    { value: SecurityQuestion.WhatIsYourMotherMaidenName, label: 'What is your mother’s maiden name?' },
    { value: SecurityQuestion.WhatIsTheNameOfYourFirstPet, label: 'What is the name of your first pet?' },
    { value: SecurityQuestion.WhatIsYourFavoriteBook, label: 'What is your favorite book?' },
    { value: SecurityQuestion.WhatWasTheNameOfYourElementarySchool, label: 'What was the name of your elementary school?' },
    { value: SecurityQuestion.WhatIsYourFavoriteChildhoodMovie, label: 'What is your favorite childhood movie?' },
    { value: SecurityQuestion.WhatIsTheNameOfYourBestChildhoodFriend, label: 'What is the name of your best childhood friend?' },
    { value: SecurityQuestion.WhatWasTheMakeOfYourFirstCar, label: 'What was the make of your first car?' },
    { value: SecurityQuestion.WhatIsTheNameOfTheStreetYouGrewUpOn, label: 'What is the name of the street you grew up on?' }
  ];

  constructor(private fb: FormBuilder, private authService: AuthService, private alertify: AlertifyService){ }

  ngOnInit(){
    this.registrationForm = this.fb.group({
      username: ['',[Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmpassword: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      securityQuestion: ['', Validators.required],
      securityAnswer: ['', Validators.required]
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
      securityQuestion: this.securityQuestion.value as SecurityQuestion,
      securityAnswer: this.securityAnswer.value
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
  get securityQuestion(){
    return this.registrationForm.get('securityQuestion') as FormControl;
  }
  get securityAnswer(){
    return this.registrationForm.get('securityAnswer') as FormControl;
  }
}
